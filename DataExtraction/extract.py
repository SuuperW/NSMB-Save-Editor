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
nodeLocationPtrOffset = 0x0C
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
		self.location = []
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

	# fileStream's position should be at the array of paths connecting this node
	def AssignPaths(self, fileStream):
		self.connections = []
		bytes = fileStream.read(4)
		while bytes[0] != 0xFF:
			self.connections.append(Connection(bytes))
			bytes = fileStream.read(4)
			if len(self.connections) > 4:
				raise Exception('Invalid data: too many paths connecting a node.')

class Path:
	def __init__(self, worldId: int, id: int, bytes: 'list[int]'):
		self.worldId = worldId
		self.idInWorld = id
		self.animationId = bytes[0]
		self.cost = bytes[1]
		# Are these flags? They seem mutually exclusive.
		self.isUnlockedBySecretGoal = bool(bytes[2] & 0x01)
		self.isUnlockedBySign = bool(bytes[2] & 0x02)
		self.exists = bool(bytes[2] & 0x40) # Idk
		self.isInvalid = bool(bytes[2] & 0x80) # ???

if __name__ == '__main__':
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
		# Node locations
		nodeLocationAddr = ReadUInt32(f, worldInfoAddr + nodeLocationPtrOffset)
		for node in nodes:
			for i in range(3):
				value = ReadSInt32(f, nodeLocationAddr + 0x0C + i * 4)
				node.location.append(round(value / 32))
			nodeLocationAddr += nodeLocationLength

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
		})

	f.close()

	f = open('data.json', 'w')
	f.write(json.dumps(worlds, indent = '\t', default=vars))
	f.close()
