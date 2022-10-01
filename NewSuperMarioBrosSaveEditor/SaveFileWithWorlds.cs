using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewSuperMarioBrosSaveEditor
{
	public class SaveFileWithWorlds
	{
		public SaveFile file;
		public WorldCollection worlds;

		public SaveFileWithWorlds(SaveFile file, WorldCollection worlds)
		{
			this.file = file;
			this.worlds = worlds;
		}

		/// <summary>
		/// NSMB sets various save file properties after a file is loaded. This method does that.
		/// </summary>
		public void PerformSaveFileLoadCalculations()
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
					if ((file.GetWorldFlags(i) & SaveFile.WorldFlags.CastleCompleted) != 0
						&& (file.GetWorldFlags(worlds[i].normalNextWorld) & SaveFile.WorldFlags.Unlocked) != 0)
					{
						highlightFlags |= nextFlag;
					}
					nextFlag <<= 1;
				}
				if (worlds[i].secretNextWorld != 0)
				{
					// Note: NSMB actually checks if the castle node is completed, not world flags.
					// But checking world flags is easier and should give the same results.
					if ((file.GetWorldFlags(i) & SaveFile.WorldFlags.CastleCompleted) != 0
						&& (file.GetWorldFlags(worlds[i].secretNextWorld) & SaveFile.WorldFlags.Unlocked) != 0)
					{
						highlightFlags |= nextFlag;
					}
					nextFlag <<= 1;
				}
			}
			file.WorldPathHighlights = (SaveFile.WorldPathHighlightsEnum)highlightFlags;

			// Star coins
			int starCoinsCollected = 0;
			for (int iw = 0; iw < worlds.Count; iw++)
			{
				World world = worlds[iw];
				for (int i = 0; i < world.nodes.Count; i++)
				{
					OverworldNode node = world.nodes[i];
					if (node.hasStarCoins)
						starCoinsCollected += file.CountStarCoins(iw, i);
				}
			}
			file.StarCoins = starCoinsCollected;

			// Star coins spent
			int starCoinsSpent = 0;
			for (int iw = 0; iw < worlds.Count; iw++)
			{
				World world = worlds[iw];
				for (int i = 0; i < world.paths.Count; i++)
				{
					OverworldPath path = world.paths[i];
					if (path.isUnlockedBySign && file.IsPathUnlocked(iw, i))
						starCoinsSpent += path.cost;
				}
			}
			if (file.BackgroundsBought.HasFlag(SaveFile.BackgroundPurchases.BlueBricks)) starCoinsSpent += 20;
			if (file.BackgroundsBought.HasFlag(SaveFile.BackgroundPurchases.Stars)) starCoinsSpent += 20;
			if (file.BackgroundsBought.HasFlag(SaveFile.BackgroundPurchases.Mario)) starCoinsSpent += 20;
			if (file.BackgroundsBought.HasFlag(SaveFile.BackgroundPurchases.Retro)) starCoinsSpent += 20;
			file.SpentStarCoins = starCoinsSpent;
		}

		/// <summary>
		/// A normal exit is a flagpole with a black flag, or normal completion of a tower/castle.
		/// This method will return false if the level does not have a normal exit.
		/// </summary>
		public bool IsNormalExitComplete(int worldId, int nodeId)
		{
			// We will check if the paths that the level unlocks via a normal exit are unlocked.
			// Or, if it is the last level of the world, check if the world that it unlocks is unlocked.
			OverworldNode node = worlds[worldId].nodes[nodeId];
			if (!node.UnlocksALevel)
			{
				if (!node.isLastLevelInWorld)
					return false;

				if (node.isBowserCastle)
					return file.IsNodeCompleted(worldId, nodeId);
				else
					return (file.GetWorldFlags(worldId) & SaveFile.WorldFlags.CastleCompleted) != 0;
			}

			for (int i = 0; i < node.pathsByNormalExit.Count; i++)
			{
				if (!file.IsPathUnlocked(worldId, node.pathsByNormalExit[i]))
					return false;
			}
			return true;
		}
		/// <summary>
		/// A secret exit is a flagpole with a red flag, or mini-Mario completion of castles that have a mini exit.
		/// This method will return false if the level does not have a secret exit.
		/// </summary>
		public bool IsSecretExitComplete(int worldId, int nodeId)
		{
			OverworldNode node = worlds[worldId].nodes[nodeId];
			if (!node.HasSecretPaths)
			{
				if (!node.isLastLevelInWorld)
					return false;

				return worlds[worldId].secretNextWorld != 0 &&
					(file.GetWorldFlags(worldId) & SaveFile.WorldFlags.CastleCompleted) != 0;
			}

			for (int i = 0; i < node.pathsBySecretExit.Count; i++)
			{
				if (!file.IsPathUnlocked(worldId, node.pathsBySecretExit[i]))
					return false;
			}
			return true;
		}

		public void UnlockWorld(int id)
		{
			ushort worldFlags = file.GetWorldFlags(id);
			worldFlags |= SaveFile.WorldFlags.AllForUnlocked;
			file.SetWorldFlags(id, worldFlags);

			// Unlocking a world also sets cutscene flags for previous worlds.
			int lastWorldToSetCutsceneses = id - 1;
			// Optional worlds do not set cutscene flags for the other optional world.
			if (id == 3 || id == 6)
				lastWorldToSetCutsceneses--;
			for (int i = 0; i <= lastWorldToSetCutsceneses; i++)
			{
				worldFlags = file.GetWorldFlags(i);
				worldFlags |= SaveFile.WorldFlags.AllBowserJuniorCutscenes;
				file.SetWorldFlags(i, worldFlags);
			}
		}
		/// <summary>
		/// This method relies on some flags of other worlds already being set correctly.
		/// There is no way to know whether world 7 was unlocked via world 5 secret exit or only by world 4 cannon.
		/// </summary>
		private void UpdateWorldFlagsAfterClearingNode(int id, CompletionAction action = null)
		{
			// The only flag we will not potentially change here is the one for the mushroom house at the start of the world.
			ushort flags = (ushort)(file.GetWorldFlags(id) & SaveFile.WorldFlags.FireworksHouse);

			// Should the world be unlocked?
			bool wasUnlocked = (file.GetWorldFlags(id) & SaveFile.WorldFlags.Unlocked) != 0;
			bool unlock = ShouldWorldBeUnlocked(id, action);

			// Was a later world unlocked?
			bool wasLaterUnlocked = (file.GetWorldFlags(id) & SaveFile.WorldFlags.CutsceneUnused) != 0;
			bool isLaterUnlocked = false;
			if (worlds[id].normalNextWorld != 0)
			{
				ushort nextWorldFlags = file.GetWorldFlags(worlds[id].normalNextWorld);
				isLaterUnlocked = (nextWorldFlags & SaveFile.WorldFlags.Unlocked) != 0
					|| (nextWorldFlags & SaveFile.WorldFlags.CutsceneUnused) != 0;
			}
			if (!isLaterUnlocked && worlds[id].secretNextWorld != 0)
			{
				ushort nextWorldFlags = file.GetWorldFlags(worlds[id].secretNextWorld);
				isLaterUnlocked = (nextWorldFlags & SaveFile.WorldFlags.Unlocked) != 0
					|| (nextWorldFlags & SaveFile.WorldFlags.CutsceneUnused) != 0;
			}
			if (isLaterUnlocked)
				flags |= SaveFile.WorldFlags.AllBowserJuniorCutscenes;

			if (unlock)
			{
				UnlockWorld(id); // unlocks any previous worlds
				flags |= SaveFile.WorldFlags.AllForUnlocked;
				// Which levels in this world are completed?
				for (int i = 0; i < worlds[id].nodes.Count; i++)
				{
					OverworldNode node = worlds[id].nodes[i];
					if (node.isFirstTower)
					{
						if (IsNormalExitComplete(id, i))
							flags |= SaveFile.WorldFlags.AllForTower;
					}
					else if (node.isSecondTower)
					{
						if (IsNormalExitComplete(id, i))
							flags |= SaveFile.WorldFlags.AllForTower2;
					}
					else if (node.isCastle)
					{
						if (file.IsNodeCompleted(id, i))
							flags |= SaveFile.WorldFlags.AllForCastle;
					}
					else if (node.name == "Cannon")
					{
						if (file.IsNodeCompleted(id, i))
							flags |= SaveFile.WorldFlags.AllBowserJuniorCutscenes;
					}
					if (node.isLastLevelInWorld)
					{
						bool completed = file.IsNodeCompleted(id, i);
						// World 8 is a special case
						if (node.isBowserCastle)
							file.PlayerHasSeenCredits = completed;
						else if (completed)
							flags |= SaveFile.WorldFlags.ExitWorldCutscene;
					}
				}
			}

			file.SetWorldFlags(id, flags);

			// Previous world's flags will need to be updated, as there is a flag that is only ever set when a later world is unlocked.
			// (And we can't just clear that one because other flags are set at the same time, which may not otherwise be set.)
			if (!unlock && (wasUnlocked || (wasLaterUnlocked && !isLaterUnlocked)))
			{
				for (int i = id - 1; i >= 0; i--)
				{
					if (worlds[i].normalNextWorld == id)
						UpdateWorldFlagsAfterClearingNode(i);
					else if (worlds[i].secretNextWorld == id)
						UpdateWorldFlagsAfterClearingNode(i);
				}
			}
		}
		/// <summary>
		/// The action given should be for a castle that can unlock this world with the given action's normal/secret settings.
		/// </summary>
		private bool ShouldWorldBeUnlocked(int id, CompletionAction action)
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
					if (file.IsNodeCompleted(i, cannonId))
						return true;
				}
				if (worlds[i].normalNextWorld == id || worlds[i].secretNextWorld == id)
					previousWorlds.Add(i);
			}

			// If there are no completed castles that can unlock this world, lock.
			if (!previousWorlds.Any((w) => (file.GetWorldFlags(w) & SaveFile.WorldFlags.CastleCompleted) != 0))
				return false;
			// Else, if there is a completed castle that can only unlock this world, unlock.
			if (previousWorlds.Any((w) =>
				worlds[w].secretNextWorld == 0 && // No world has only secret exit, so if there's no secret there's just one exit.
				(file.GetWorldFlags(w) & SaveFile.WorldFlags.CastleCompleted) == 0
			))
				return true;
			// Else, if we have an action, use that.
			if (action != null)
				return action.Complete;
			// Else, keep current status. There's no way to really know what it should be.
			return (file.GetWorldFlags(id) & SaveFile.WorldFlags.Unlocked) != 0;
		}

		public void PerformNodeAction(int worldId, int nodeId, CompletionAction action)
		{
			UnlockPaths(worldId, nodeId, action);

			OverworldNode node = worlds[worldId].nodes[nodeId];

			// Will the node be completed after this action?
			bool nodeIsCompleted = action.Complete ||
				(IsNormalExitComplete(worldId, nodeId) && !action.NormalExit) ||
				(IsSecretExitComplete(worldId, nodeId) && !action.SecretExit);

			byte nodeFlags = file.GetNodeFlags(worldId, nodeId);
			if (nodeIsCompleted)
			{
				nodeFlags |= SaveFile.NodeFlags.AllForCompleted;
				if (action.StarCoins)
					nodeFlags |= SaveFile.NodeFlags.AllStarCoins;
			}
			else
				nodeFlags = 0;
			file.SetNodeFlags(worldId, nodeId, nodeFlags);

			// For special levels, set world flags
			// If secretExit it set, check if the node has a secret goal
			action.SecretExit = action.SecretExit && worlds[worldId].NodeHasSecretExit(node);

			// Towers only set flags for normal exit
			ushort worldFlags = file.GetWorldFlags(worldId);
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
			file.SetWorldFlags(worldId, worldFlags);

			// World unlocks are last, because they'll also modify flags for the current world.
			if (node.name == "Cannon")
			{
				if (action.NormalExit)
					UpdateWorldFlagsAfterClearingNode(worlds[worldId].cannonDestination);
			}
			else if (node.isLastLevelInWorld)
			{
				if (!node.isBowserCastle)
				{
					if (action.NormalExit)
						UpdateWorldFlagsAfterClearingNode(worlds[worldId].normalNextWorld, action);
					if (action.SecretExit)
						UpdateWorldFlagsAfterClearingNode(worlds[worldId].secretNextWorld, action);
				}
				else
					file.PlayerHasSeenCredits = action.Complete;
			}

			PerformSaveFileLoadCalculations();
		}
		private void UnlockPaths(int worldId, int id, CompletionAction action, bool pathsOnly = false)
		{
			List<OverworldNode> nodes = worlds[worldId].nodes;
			List<OverworldPath> paths = worlds[worldId].paths;

			IEnumerable<int> pathsToUnlock = action.NormalExit ? nodes[id].pathsByNormalExit : new List<int>();
			if (action.SecretExit) pathsToUnlock = pathsToUnlock.Union(nodes[id].pathsBySecretExit);

			foreach (int pid in pathsToUnlock)
			{
				OverworldPath path = paths[pid];
				// Lock signs, but don't unlock them
				if (!action.Complete || !path.isUnlockedBySign)
				{
					// Unlock this path, but don't lock if another level has unlocked it.
					if (action.Complete || !nodes.Any((n) => {
						// We will assume that every level exit that unlocks a path has at least one unique path. (That is, a path that no other exit unlocks.)
						// So, a path has been unlocked by a level if all paths that it unlocks are unlocked.
						if (n.idInWorld == id)
							return false;
						if (n.pathsByNormalExit.Contains(pid) && n.pathsByNormalExit.All((p) => paths[p].isUnlockedBySign || file.IsPathUnlocked(worldId, p)))
							return true;
						if (n.pathsBySecretExit.Contains(pid) && n.pathsBySecretExit.All((p) => paths[p].isUnlockedBySign || file.IsPathUnlocked(worldId, p)))
							return true;
						return false;
					}))
						file.SetPathUnlocked(worldId, pid, action.Complete);
				}
			}
		}


	}
}
