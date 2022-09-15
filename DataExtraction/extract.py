"""
This script expects to find a file called dump.bin, which is a memory dump of NSMB's main RAM while in the overworld. (US version) Alternatively, you could extract and decompress ARM9 overlay 8, and place the data in a dump.bin file according to the overlay's RAM location. (Note that address 0x00 in dump.bin corresponds to NDS's RAM address 0x0200_0000, since that's where main RAM begins.)

Data we need to collect:
1) The level names (e.g. 1-2, 5-Tower) for each world map node
2) Which nodes are levels / have star coins
3) The locations for each node
4) Which paths connect to each node
5) Path properties, such as direction, pipe/regular, sign, and if unlocked by secret goal
"""

import json

LevelIconNames = [
	'Start', '1', '2', '3', '4', '5', '6', '7', '8', 'A', 'B', 'C',
	'GhostHosue', 'Tower', 'Castle', 'Pipe', 'Cannon', 'RedMushroom',
	'GreenMushroom', 'MegaMushroom', 'dot', 'Tower2', 'BowserCastle',
	'PurpleMushroom',
]

MainRAMBase = 0x0200_0000
def ReadSInt32(fileStream, location):
	fileStream.seek(location - MainRAMBase)
	return int.from_bytes(fileStream.read(4), 'little', signed = True)
def ReadUInt32(fileStream, location):
	fileStream.seek(location - MainRAMBase)
	return int.from_bytes(fileStream.read(4), 'little')
def ReadUInt16(fileStream, location):
	fileStream.seek(location - MainRAMBase)
	return int.from_bytes(fileStream.read(2), 'little')

# Pointers, offsets, sizes
worldsInfoAddr = 0x020E79C4
nodeDataPtrOffset = 0x00
nodeDataLength = 0x0C
pathUnlockPtrOffset = 0x04
pathUnlockLength = 0x04
nodeMoreDataPtrOffset = 0x0C
nodeLocationLength = 0x18
nodeCountOffset = 0x20
pathCountOffset = 0x22
worldInfoLength = 0x28

class Connection:
	def __init__(self, bytes: 'list[int]'):
		self.destinationNodeId = bytes[0]
		self.pathIdInWorld = bytes[1]
		self.direction = \
			'Pipe'  if bytes[2] & 0x10 else \
			'Down'  if bytes[2] & 3 == 0 else \
			'Right' if bytes[2] & 3 == 1 else \
			'Up'    if bytes[2] & 3 == 2 else \
			'Left'
		self.isBackwards = bool(bytes[2] & 0x40)

class Node:
	def __init__(self, worldId: int, id: int, fileStream):
		self.worldId = worldId
		self.idInWorld = id
		bytes = fileStream.read(0xC)

		fileStream.seek(int.from_bytes(bytes[:4], 'little') - MainRAMBase)
		self.AssignPaths(fileStream)

		self.areaId = int.from_bytes(bytes[4:6], 'little')
		self.name = LevelIconNames[bytes[7]]

		self.hasStarCoins = bool(bytes[8] & 0x01)
		self.isFirstTower = bool(bytes[8] & 0x02)
		self.isCastle = bool(bytes[8] & 0x04)
		self.isVisible = bool(bytes[8] & 0x08)
		self.isLastLevelInWorld = bool(bytes[8] & 0x10)
		self.isSecondTower = bool(bytes[8] & 0x20)
		self.isBowserCastle = bool(bytes[8] & 0x40)
		self.is8DashCastle = bool(bytes[8] & 0x80)
		# Bytes 9-11 appear to be unused.

	# fileStream's position should be at the array of paths connecting this node
	def AssignPaths(self, fileStream):
		self.connections = []
		bytes = fileStream.read(4)
		while bytes[0] != 0xFF:
			self.connections.append(Connection(bytes))
			bytes = fileStream.read(4)
			if len(self.connections) > 4:
				raise Exception('Invalid data: too many paths connecting a node.')
	
	# fileStream's position should be at the struct pointed to at nodeMoreDataPtrOffset
	def LoadMoreData(self, fileStream):
		def ReadPathsByExit(fileStream):
			b = fileStream.read(4)
			index = b.find(0)
			if index == -1:
				return list(b)
			else:
				return list(b)[:index]
		self.pathsByNormalExit = ReadPathsByExit(fileStream)
		self.pathsBySecretExit = ReadPathsByExit(fileStream)
		self.zoomToNormalExit = fileStream.read(2)[0]
		self.zoomToSecretExit = fileStream.read(2)[0]
		self.location = []
		for _ in range(3):
			value = int.from_bytes(fileStream.read(4), 'little', signed=True)
			self.location.append(round(value / 32))

class Path:
	def __init__(self, worldId: int, id: int, bytes: 'list[int]'):
		self.worldId = worldId
		self.idInWorld = id
		self.animationId = bytes[0]
		self.cost = bytes[1]
		# Are these flags? They seem mutually exclusive.
		self.isUnlockedBySecretGoal = bool(bytes[2] & 0x01)
		self.isUnlockedBySign = bool(bytes[2] & 0x02)
		self.exists = not bool(bytes[2] & 0x40) # Idk
		self.isInvalid = bool(bytes[2] & 0x80) # ???

if __name__ == '__main__':
	# I do not know where this data is in-game.
	cannonDestinations = [4, 4, 5, 6, 7, 0, 0, 0]
	normalNextWorlds = [1, 2, 4, 4, 5, 7, 7, 0]
	secretNextWorlds = [0, 3, 0, 0, 6, 0, 0, 0]

	f = open('dump.bin', 'rb')

	worlds: list = []
	for world in range(8):
		worldInfoAddr = worldsInfoAddr + worldInfoLength * world

		nodes: 'list[Node]' = []
		paths: 'list[Path]' = []
		nodeCount = ReadUInt16(f, worldInfoAddr + nodeCountOffset)
		pathCount = ReadUInt16(f, worldInfoAddr + pathCountOffset)

		# Node data: connections to paths, level name, type
		nodeDataAddr = ReadUInt32(f, worldInfoAddr + nodeDataPtrOffset)
		for i in range(nodeCount):
			f.seek(nodeDataAddr - MainRAMBase)
			nodes.append(Node(world, i, f))
			nodeDataAddr += nodeDataLength
		# More data: locations, which paths/levels are unlocked by clearing
		nodeLocationAddr = ReadUInt32(f, worldInfoAddr + nodeMoreDataPtrOffset)
		f.seek(nodeLocationAddr - MainRAMBase)
		for node in nodes:
			node.LoadMoreData(f)

		# Paths
		pathUnlockAddr = ReadUInt32(f, worldInfoAddr + pathUnlockPtrOffset)
		invalidCount = 0
		f.seek(pathUnlockAddr - MainRAMBase)
		for i in range(pathCount):
			path = Path(world, i, f.read(pathUnlockLength))
			paths.append(path)
		
		worlds.append({
			'id': world,
			'nodes': nodes,
			'paths': paths,
			'cannonDestination': cannonDestinations[world],
			'normalNextWorld': normalNextWorlds[world],
			'secretNextWorld': secretNextWorlds[world],
		})
	
	f.close()

	f = open('data.json', 'w')
	f.write(json.dumps(worlds, indent = '\t', default=vars))
	f.close()
