using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace NewSuperMarioBrosSaveEditor
{
	public class WorldCollection
	{
		private List<World> worlds = new List<World>();
		public World this[int index] => worlds[index];
		public int Count => worlds.Count;

		public static explicit operator WorldCollection(JArray j)
		{
			WorldCollection wc = new WorldCollection();
			foreach (JToken w in j)
				wc.worlds.Add((World)w);

			return wc;
		}

		/// <summary>
		/// A normal exit is a flagpole with a black flag, or normal completion of a tower/castle.
		/// This method will return false if the level does not have a normal exit.
		/// </summary>
		public bool IsNormalExitComplete(SaveFile saveFile, int worldId, int nodeId)
		{
			// We will check if the paths that the level unlocks via a normal exit are unlocked.
			// Or, if it is the last level of the world, check if the world that it unlocks is unlocked.
			OverworldNode node = worlds[worldId].nodes[nodeId];
			if (!node.UnlocksALevel)
			{
				if (!node.isLastLevelInWorld)
					return false;

				if (node.isBowserCastle)
					return saveFile.IsNodeCompleted(worldId, nodeId);
				else
					return (saveFile.GetWorldFlags(worldId) & SaveFile.WorldFlags.CastleCompleted) != 0;
			}

			for (int i = 0; i < node.pathsByNormalExit.Count; i++)
			{
				if (!saveFile.IsPathUnlocked(worldId, node.pathsByNormalExit[i]))
					return false;
			}
			return true;
		}
		/// <summary>
		/// A secret exit is a flagpole with a red flag, or mini-Mario completion of castles that have a mini exit.
		/// This method will return false if the level does not have a secret exit.
		/// </summary>
		public bool IsSecretExitComplete(SaveFile saveFile, int worldId, int nodeId)
		{
			OverworldNode node = worlds[worldId].nodes[nodeId];
			if (!node.HasSecretPaths)
			{
				if (!node.isLastLevelInWorld)
					return false;
				
				return worlds[worldId].secretNextWorld != 0 &&
					(saveFile.GetWorldFlags(worldId) & SaveFile.WorldFlags.CastleCompleted) != 0;
			}

			for (int i = 0; i < node.pathsBySecretExit.Count; i++)
			{
				if (!saveFile.IsPathUnlocked(worldId, node.pathsBySecretExit[i]))
					return false;
			}
			return true;
		}

		public void UnlockWorld(SaveFile saveFile, int id, bool unlocked)
		{
			if (unlocked)
			{
				ushort worldFlags = saveFile.GetWorldFlags(id);
				worldFlags |= SaveFile.WorldFlags.AllForUnlocked;
				saveFile.SetWorldFlags(id, worldFlags);

				// Unlocking a world also sets cutscene flags for previous worlds.
				int lastWorldToSetCutsceneses = id - 1;
				// Optional worlds do not set cutscene flags for the other optional world.
				if (id == 3 || id == 6)
					lastWorldToSetCutsceneses--;
				for (int i = 0; i <= lastWorldToSetCutsceneses; i++)
				{
					worldFlags = saveFile.GetWorldFlags(i);
					worldFlags |= SaveFile.WorldFlags.AllBowserJuniorCutscenes;
					saveFile.SetWorldFlags(i, worldFlags);
				}
			}
			else
			{
				// We might not want to actually lock it, if a cannon that leads here is still clear.
				// So what we actually will do is inspect the save file to determine if it should be locked.
				bool remainUnlocked = false;
				for (int i = 0; i < worlds.Count; i++)
				{
					if (worlds[i].cannonDestination == id)
					{
						int cannonId = worlds[i].nodes.FindIndex((n) => n.name == "Cannon");
						if (saveFile.IsNodeCompleted(id, cannonId))
						{
							remainUnlocked = true;
							break;
						}
					}
					// The game doesn't distinguish between these.
					// TODO: Handle this correctly when the user un-completes only normal or only secret castle.
					if (worlds[i].normalNextWorld == id || worlds[i].secretNextWorld == id)
					{
						ushort flags = saveFile.GetWorldFlags(i);
						if ((flags & SaveFile.WorldFlags.CastleCompleted) != 0)
						{
							remainUnlocked = true;
							break;
						}
					}
				}
				if (!remainUnlocked)
				{
					// This may not be a good idea
					//saveFile.ResetAllNodesAndPathsInWorld(id);
					// Now, has this world been skipped by a cannon?
					ushort nextWorldFlags = id + 1 < worlds.Count ? saveFile.GetWorldFlags(id + 1) : (ushort)0;
					bool wasSkipped = (nextWorldFlags & SaveFile.WorldFlags.CutsceneEnter) != 0;
					// If it was, we keep the cutscene flags that were set by skipping it.
					if (wasSkipped)
						saveFile.SetWorldFlags(id, SaveFile.WorldFlags.AllBowserJuniorCutscenes);
					else
						saveFile.SetWorldFlags(id, 0);
				}
			}
		}

		public void PerformNodeAction(SaveFile saveFile, int worldId, int nodeId, CompletionAction action)
		{
			UnlockPaths(saveFile, worldId, nodeId, action);

			OverworldNode node = worlds[worldId].nodes[nodeId];

			// Will the node be completed after this action?
			bool nodeIsCompleted = action.Complete ||
				(IsNormalExitComplete(saveFile, worldId, nodeId) && !action.NormalExit) ||
				(IsSecretExitComplete(saveFile, worldId, nodeId) && !action.SecretExit);

			byte nodeFlags = saveFile.GetNodeFlags(worldId, nodeId);
			if (nodeIsCompleted)
			{
				nodeFlags |= SaveFile.NodeFlags.Completed;
				if (action.StarCoins)
					nodeFlags |= SaveFile.NodeFlags.AllStarCoins;
			}
			else
				nodeFlags = 0;
			saveFile.SetNodeFlags(worldId, nodeId, nodeFlags);

			// For special levels, set world flags
			// If secretExit it set, check if the node has a secret goal
			action.SecretExit = action.SecretExit && (!node.pathsByNormalExit.SequenceEqual(node.pathsBySecretExit));

			// Towers, Castle
			ushort worldFlags = saveFile.GetWorldFlags(worldId);
			ushort newFlags = 0;
			if (action.NormalExit)
			{
				if (node.isFirstTower)
					newFlags |= SaveFile.WorldFlags.AllForTower;
				else if (node.isCastle)
					newFlags |= SaveFile.WorldFlags.AllForCastle;
				else if (node.isSecondTower)
					newFlags |= SaveFile.WorldFlags.AllForTower2;
			}
			if (action.Complete)
				worldFlags |= newFlags;
			else
				worldFlags &= (ushort)~newFlags;

			// End of the world
			if (node.isLastLevelInWorld)
			{
				if (nodeIsCompleted)
					worldFlags |= SaveFile.WorldFlags.ExitWorldCutscene;
				else
					worldFlags &= (ushort)~SaveFile.WorldFlags.ExitWorldCutscene;
			}
			saveFile.SetWorldFlags(worldId, worldFlags);

			// World unlocks are last, because they'll also modify flags for the current world.
			if (node.name == "Cannon")
			{
				if (action.NormalExit)
					UnlockWorld(saveFile, worlds[worldId].cannonDestination, action.Complete);
				// TODO: Handle re-locking worlds.
			}
			else if (node.isLastLevelInWorld)
			{
				if (!node.isBowserCastle)
				{
					if (action.NormalExit)
						UnlockWorld(saveFile, worlds[worldId].normalNextWorld, action.Complete);
					if (action.SecretExit)
						UnlockWorld(saveFile, worlds[worldId].secretNextWorld, action.Complete);
					// TODO: Handle re-locking worlds.
				}
				else
					saveFile.PlayerHasSeenCredits = action.Complete;
			}
		}
		private void UnlockPaths(SaveFile saveFile, int worldId, int id, CompletionAction action, bool pathsOnly = false)
		{
			List<OverworldNode> nodes = worlds[worldId].nodes;
			List<OverworldPath> paths = worlds[worldId].paths;

			// We have to look at the node's connections, and not just the paths that it unlocks.
			// This is so that we can follow those connections to update intermediate nodes.

			// For regular levels, we want to use only the connections that it unlocks.
			// But otherwise, we want to use all connections.
			IEnumerable<OverworldNode.Connection> connections = nodes[id].connections
				.Where((c) => !c.isBackwards);
			if (nodes[id].UnlocksALevel)
			{
				IEnumerable<int> pathsToUnlock = action.NormalExit ? nodes[id].pathsByNormalExit : new List<int>();
				if (action.SecretExit) pathsToUnlock = pathsToUnlock.Union(nodes[id].pathsBySecretExit);
				connections = connections.Where((c) => pathsToUnlock.Contains(c.pathIdInWorld));
			}

			foreach (OverworldNode.Connection connection in connections)
			{
				int pathId = connection.pathIdInWorld;
				OverworldPath path = paths[pathId];
				// Lock signs, but don't unlock them
				if (!action.Complete || !path.isUnlockedBySign)
				{
					// Un/lock this path.
					saveFile.SetPathUnlocked(worldId, pathId, action.Complete);
					// Where does it lead?
					int destinationId = connection.destinationNodeId;
					OverworldNode destination = nodes[destinationId];
					// If it leads to a main level, we're done. (has star coins should work)
					// If it leads to a non-playable level, un/lock paths from that one.
					if (!destination.hasStarCoins)
					{
						// We also need to make sure we're not locking a path that a different level unlocked.
						// E.g., un-completing 1-3 while 1-2 secret goal is cleared should not lock 1-Tower.
						bool unlockNext = true;
						if (!action.Complete)
						{
							List<OverworldNode.Connection> destConnections = destination.connections;
							unlockNext = !destConnections
								.Where((dc) => dc.isBackwards)
								.Any((dc) => (saveFile.GetPathFlags(worldId, dc.pathIdInWorld) & SaveFile.PathFlags.Unlocked) != 0);
						}

						if (unlockNext)
						{
							// We will only unlock paths if we're passing through a mushroom house.
							UnlockPaths(saveFile, worldId, destinationId, action, destination.name != "dot");
						}
					}
				}
			}
		}

	}
}
