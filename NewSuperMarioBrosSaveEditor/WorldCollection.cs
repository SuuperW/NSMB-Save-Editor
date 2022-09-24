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
		/// NSMB sets various save file properties after a file is loaded. This method does that.
		/// </summary>
		public void PerformSaveFileLoadCalculations(SaveFile saveFile)
		{
			// Bottom screen, highlight lines between worlds
			uint highlightFlags = 0;
			uint nextFlag = (uint)SaveFile.WorldPathHighlightsEnum.W1toW2;
			for (int i = 0; i < worlds.Count; i++)
			{
				if (worlds[i].normalNextWorld != 0)
				{
					// Note: NSMB actually checks if the castle node is completed, not world flags.
					// But checking world flags is easier and should give the same results.
					if ((saveFile.GetWorldFlags(i) & SaveFile.WorldFlags.CastleCompleted) != 0
						&& (saveFile.GetWorldFlags(worlds[i].normalNextWorld) & SaveFile.WorldFlags.Unlocked) != 0)
					{
						highlightFlags |= nextFlag;
					}
					nextFlag <<= 1;
				}
				if (worlds[i].secretNextWorld != 0)
				{
					// Note: NSMB actually checks if the castle node is completed, not world flags.
					// But checking world flags is easier and should give the same results.
					if ((saveFile.GetWorldFlags(i) & SaveFile.WorldFlags.CastleCompleted) != 0
						&& (saveFile.GetWorldFlags(worlds[i].secretNextWorld) & SaveFile.WorldFlags.Unlocked) != 0)
					{
						highlightFlags |= nextFlag;
					}
					nextFlag <<= 1;
				}
			}
			saveFile.WorldPathHighlights = (SaveFile.WorldPathHighlightsEnum)highlightFlags;

			// Star coins
			int starCoinsCollected = 0;
			for (int iw = 0; iw < worlds.Count; iw++)
			{
				World world = worlds[iw];
				for (int i = 0; i < world.nodes.Count; i++)
				{
					OverworldNode node = world.nodes[i];
					if (node.hasStarCoins)
						starCoinsCollected += saveFile.CountStarCoins(iw, i);
				}
			}
			saveFile.StarCoins = starCoinsCollected;

			// Star coins spent
			int starCoinsSpent = 0;
			for (int iw = 0; iw < worlds.Count; iw++)
			{
				World world = worlds[iw];
				for (int i = 0; i < world.paths.Count; i++)
				{
					OverworldPath path = world.paths[i];
					if (path.isUnlockedBySign && saveFile.IsPathUnlocked(iw, i))
						starCoinsSpent += path.cost;
				}
			}
			if (saveFile.BlueBricksBackgroundBought) starCoinsSpent += 20;
			if (saveFile.MarioBackgroundBought) starCoinsSpent += 20;
			if (saveFile.StarsBackgroundBought) starCoinsSpent += 20;
			if (saveFile.RetroBackgroundBought) starCoinsSpent += 20;
			saveFile.SpentStarCoins = starCoinsSpent;
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

		public void UnlockWorld(SaveFile saveFile, int id)
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
		/// <summary>
		/// This method relies on some flags of other worlds already being set correctly.
		/// There is no way to know whether world 7 was unlocked via world 5 secret exit or only by world 4 cannon.
		/// </summary>
		private void UpdateWorldFlagsAfterClearingNode(SaveFile saveFile, int id, CompletionAction action = null)
		{
			// The only flag we will not potentially change here is the one for the mushroom house at the start of the world.
			ushort flags = (ushort)(saveFile.GetWorldFlags(id) & SaveFile.WorldFlags.FireworksHouse);

			// Should the world be unlocked?
			bool wasUnlocked = (saveFile.GetWorldFlags(id) & SaveFile.WorldFlags.Unlocked) != 0;
			bool unlock = ShouldWorldBeUnlocked(saveFile, id, action);

			// Was a later world unlocked?
			bool wasLaterUnlocked = (saveFile.GetWorldFlags(id) & SaveFile.WorldFlags.CutsceneUnused) != 0;
			bool isLaterUnlocked = false;
			if (worlds[id].normalNextWorld != 0)
			{
				ushort nextWorldFlags = saveFile.GetWorldFlags(worlds[id].normalNextWorld);
				isLaterUnlocked = (nextWorldFlags & SaveFile.WorldFlags.Unlocked) != 0
					|| (nextWorldFlags & SaveFile.WorldFlags.CutsceneUnused) != 0;
			}
			if (!isLaterUnlocked && worlds[id].secretNextWorld != 0)
			{
				ushort nextWorldFlags = saveFile.GetWorldFlags(worlds[id].secretNextWorld);
				isLaterUnlocked = (nextWorldFlags & SaveFile.WorldFlags.Unlocked) != 0
					|| (nextWorldFlags & SaveFile.WorldFlags.CutsceneUnused) != 0;
			}
			if (isLaterUnlocked)
				flags |= SaveFile.WorldFlags.AllBowserJuniorCutscenes;

			if (unlock)
			{
				UnlockWorld(saveFile, id); // unlocks any previous worlds
				flags |= SaveFile.WorldFlags.AllForUnlocked;
				// Which levels in this world are completed?
				for (int i = 0; i < worlds[id].nodes.Count; i++)
				{
					OverworldNode node = worlds[id].nodes[i];
					if (node.isFirstTower)
					{
						if (IsNormalExitComplete(saveFile, id, i))
							flags |= SaveFile.WorldFlags.AllForTower;
					}
					else if (node.isSecondTower)
					{
						if (IsNormalExitComplete(saveFile, id, i))
							flags |= SaveFile.WorldFlags.AllForTower2;
					}
					else if (node.isCastle)
					{
						if (saveFile.IsNodeCompleted(id, i))
							flags |= SaveFile.WorldFlags.AllForCastle;
					}
					else if (node.name == "Cannon")
					{
						if (saveFile.IsNodeCompleted(id, i))
							flags |= SaveFile.WorldFlags.AllBowserJuniorCutscenes;
					}
					if (node.isLastLevelInWorld)
					{
						bool completed = saveFile.IsNodeCompleted(id, i);
						// World 8 is a special case
						if (node.isBowserCastle)
							saveFile.PlayerHasSeenCredits = completed;
						else if (completed)
							flags |= SaveFile.WorldFlags.ExitWorldCutscene;
					}
				}
			}

			saveFile.SetWorldFlags(id, flags);

			// Previous world's flags will need to be updated, as there is a flag that is only ever set when a later world is unlocked.
			// (And we can't just clear that one because other flags are set at the same time, which may not otherwise be set.)
			if (!unlock && (wasUnlocked || (wasLaterUnlocked && !isLaterUnlocked)))
			{
				for (int i = id - 1; i >= 0; i--)
				{
					if (worlds[i].normalNextWorld == id)
						UpdateWorldFlagsAfterClearingNode(saveFile, i);
					else if (worlds[i].secretNextWorld == id)
						UpdateWorldFlagsAfterClearingNode(saveFile, i);
				}
			}
		}
		/// <summary>
		/// The action given should be for a castle that can unlock this world with the given action's normal/secret settings.
		/// </summary>
		private bool ShouldWorldBeUnlocked(SaveFile saveFile, int id, CompletionAction action)
		{
			if (id == 0 || (action != null && action.Complete))
				return true;

			List<int> previousWorlds = new List<int>();

			// If a cannon leading here is cleared, it should be unlocked.
			for (int i = 0; i < id; i++)
			{
				if (worlds[i].cannonDestination == id)
				{
					int cannonId = worlds[i].nodes.FindIndex((n) => n.name == "Cannon");
					if (saveFile.IsNodeCompleted(i, cannonId))
						return true;
				}
				if (worlds[i].normalNextWorld == id || worlds[i].secretNextWorld == id)
					previousWorlds.Add(i);
			}

			// If there are no completed castles that can unlock this world, lock.
			if (!previousWorlds.Any((w) => (saveFile.GetWorldFlags(w) & SaveFile.WorldFlags.CastleCompleted) != 0))
				return false;
			// Else, if there is a completed castle that can only unlock this world, unlock.
			if (previousWorlds.Any((w) =>
				worlds[w].secretNextWorld == 0 && // No world has only secret exit, so if there's no secret there's just one exit.
				(saveFile.GetWorldFlags(w) & SaveFile.WorldFlags.CastleCompleted) == 0
			))
				return true;
			// Else, if we have an action, use that.
			if (action != null)
				return action.Complete;
			// Else, keep current status. There's no way to really know what it should be.
			return (saveFile.GetWorldFlags(id) & SaveFile.WorldFlags.Unlocked) != 0;
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
			action.SecretExit = action.SecretExit && worlds[worldId].NodeHasSecretExit(node);

			// Towers only set flags for normal exit
			ushort worldFlags = saveFile.GetWorldFlags(worldId);
			ushort newFlags = 0;
			if (action.NormalExit)
			{
				if (node.isFirstTower)
					newFlags |= SaveFile.WorldFlags.AllForTower;
				else if (node.isSecondTower)
					newFlags |= SaveFile.WorldFlags.AllForTower2;
			}
			if (action.Complete)
				worldFlags |= newFlags;
			else
				worldFlags &= (ushort)~newFlags;

			// Castles set flags for either exit
			newFlags = 0;
			if (node.isCastle)
				newFlags |= SaveFile.WorldFlags.AllForCastle;
			if (node.isLastLevelInWorld)
				newFlags |= SaveFile.WorldFlags.ExitWorldCutscene;
			if (nodeIsCompleted)
				worldFlags |= newFlags;
			else
				worldFlags &= (ushort)~newFlags;
			saveFile.SetWorldFlags(worldId, worldFlags);

			// World unlocks are last, because they'll also modify flags for the current world.
			if (node.name == "Cannon")
			{
				if (action.NormalExit)
					UpdateWorldFlagsAfterClearingNode(saveFile, worlds[worldId].cannonDestination);
			}
			else if (node.isLastLevelInWorld)
			{
				if (!node.isBowserCastle)
				{
					if (action.NormalExit)
						UpdateWorldFlagsAfterClearingNode(saveFile, worlds[worldId].normalNextWorld, action);
					if (action.SecretExit)
						UpdateWorldFlagsAfterClearingNode(saveFile, worlds[worldId].secretNextWorld, action);
				}
				else
					saveFile.PlayerHasSeenCredits = action.Complete;
			}

			PerformSaveFileLoadCalculations(saveFile);
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
