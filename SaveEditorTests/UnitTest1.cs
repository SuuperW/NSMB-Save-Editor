﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

using Newtonsoft.Json.Linq;

using NewSuperMarioBrosSaveEditor;

namespace SaveEditorTests
{
	[TestClass]
	public class UnitTest1
	{
		private void assert(bool condition) => Assert.IsTrue(condition);

		private MemoryStream ms = new MemoryStream(File.ReadAllBytes("Saves/New.sav"));

		private SaveFile saveFile;
		private WorldCollection worlds;

		private CompletionAction completeAll = new CompletionAction()
		{
			Complete = true,
			NormalExit = true,
			SecretExit = true,
			StarCoins = true,
		};
		private CompletionAction completeNormal = new CompletionAction()
		{
			Complete = true,
			NormalExit = true,
			SecretExit = false,
			StarCoins = false,
		};
		private CompletionAction completeSecret = new CompletionAction()
		{
			Complete = true,
			NormalExit = false,
			SecretExit = true,
			StarCoins = false,
		};
		private CompletionAction uncompleteAll = new CompletionAction()
		{
			Complete = false,
			NormalExit = true,
			SecretExit = true,
			StarCoins = true,
		};
		private CompletionAction uncompleteNormal = new CompletionAction()
		{
			Complete = false,
			NormalExit = true,
			SecretExit = false,
			StarCoins = false,
		};
		private CompletionAction uncompleteSecret = new CompletionAction()
		{
			Complete = false,
			NormalExit = false,
			SecretExit = true,
			StarCoins = false,
		};

		public UnitTest1()
		{
			JArray jArray = JArray.Parse(File.ReadAllText("data.json"));
			worlds = (WorldCollection)jArray;
		}

		[TestInitialize]
		public void PreTest()
		{
			saveFile = SaveFile.FromSav(ms, 0);
		}

		[TestMethod]
		public void TestSomeInitialUnlocks()
		{
			// Some nodes
			assert(saveFile.IsNodeCompleted(0, 0));
			assert(!saveFile.IsNodeCompleted(0, 1));
			// Some paths
			assert(saveFile.IsPathUnlocked(0, 0));
			assert(!saveFile.IsPathUnlocked(0, 1));
			// Brand-new file doesn't have world 1 unlocked
			assert((saveFile.GetWorldFlags(0) & SaveFile.WorldFlags.AllForUnlocked) == 0);
		}

		[TestMethod]
		public void TestClearingW11()
		{
			worlds.PerformNodeAction(saveFile, 0, 1, completeAll);
			assert(saveFile.IsNodeCompleted(0, 1));
			assert(saveFile.IsPathUnlocked(0, 1));
			// Path to purple mushroom
			assert(!saveFile.IsPathUnlocked(0, 21));
			// Star coins
			assert((saveFile.GetNodeFlags(0, 1) & SaveFile.NodeFlags.AllStarCoins) == SaveFile.NodeFlags.AllStarCoins);
		}

		[TestMethod]
		public void TestClearingWithMultiplePaths()
		{
			worlds.PerformNodeAction(saveFile, 0, 2, completeAll);
			// 1-2 node
			assert(saveFile.IsNodeCompleted(0, 2));
			// Path to 1-3
			assert(saveFile.IsPathUnlocked(0, 2));
			// Path to 1-MushroomHouse, then Tower
			assert(saveFile.IsPathUnlocked(0, 0x0A));
			assert(saveFile.IsPathUnlocked(0, 0x16));
			assert(saveFile.IsPathUnlocked(0, 0x04));
		}

		[TestMethod]
		public void TestUnclearW11()
		{
			worlds.PerformNodeAction(saveFile, 0, 1, completeAll);
			worlds.PerformNodeAction(saveFile, 0, 1, uncompleteAll);
			// node
			assert(!saveFile.IsNodeCompleted(0, 1));
			// path
			assert(!saveFile.IsPathUnlocked(0, 1));
			// Star coins
			assert((saveFile.GetNodeFlags(0, 1) & SaveFile.NodeFlags.AllStarCoins) == 0);
		}

		[TestMethod]
		public void TestClearNodeWithSign()
		{
			// A path with a sign on it should not be unlocked when the relevant level is completed
			worlds.PerformNodeAction(saveFile, 0, 3, completeAll);
			assert(!saveFile.IsPathUnlocked(0, 0x0B));	
		}

		[TestMethod]
		public void TestUnclearLevelWhenAnotherLevelHasUnlockedSamePath()
		{
			worlds.PerformNodeAction(saveFile, 0, 3, completeNormal);
			worlds.PerformNodeAction(saveFile, 0, 2, completeSecret);
			worlds.PerformNodeAction(saveFile, 0, 2, uncompleteSecret);
			// Path from 1-dot to 1-Tower should remain unlocked
			assert(saveFile.IsPathUnlocked(0, 4));
		}

		[TestMethod]
		public void TestClearingTower()
		{
			// The secret goal should not set world flags
			worlds.PerformNodeAction(saveFile, 0, 4, completeSecret);
			assert((saveFile.GetWorldFlags(0) & SaveFile.WorldFlags.AllForTower) == 0);
			// But the normal goal should
			worlds.PerformNodeAction(saveFile, 0, 4, completeNormal);
			assert((saveFile.GetWorldFlags(0) & SaveFile.WorldFlags.AllForTower) == SaveFile.WorldFlags.AllForTower);
		}

		[TestMethod]
		public void TestClearingW1Castle()
		{
			// We should unlock world 2, and have world 1 flags set
			worlds.PerformNodeAction(saveFile, 0, 7, completeNormal);
			assert((saveFile.GetWorldFlags(1) & SaveFile.WorldFlags.AllForUnlocked) == SaveFile.WorldFlags.AllForUnlocked);
			assert((saveFile.GetWorldFlags(0) & SaveFile.WorldFlags.AllForCastle) == SaveFile.WorldFlags.AllForCastle);
			assert((saveFile.GetWorldFlags(0) & SaveFile.WorldFlags.AllBowserJuniorCutscenes) == SaveFile.WorldFlags.AllBowserJuniorCutscenes);
			assert((saveFile.GetWorldFlags(0) & SaveFile.WorldFlags.ExitWorldCutscene) == SaveFile.WorldFlags.ExitWorldCutscene);
		}

		[TestMethod]
		public void TestUnclearingTower()
		{
			worlds.PerformNodeAction(saveFile, 0, 4, completeAll);
			// After uncompleting secret, tower flags should still be set
			worlds.PerformNodeAction(saveFile, 0, 4, uncompleteSecret);
			assert((saveFile.GetWorldFlags(0) & SaveFile.WorldFlags.AllForTower) == SaveFile.WorldFlags.AllForTower);
			// After uncompleting normal, tower flags should not be set
			worlds.PerformNodeAction(saveFile, 0, 4, uncompleteNormal);
			assert((saveFile.GetWorldFlags(0) & SaveFile.WorldFlags.AllForTower) == 0);
		}

		[TestMethod]
		public void TestUnclearingW1Castle()
		{
			// Clear 1-Tower so ensure that unclearing 1-Castle is checking that completion
			worlds.PerformNodeAction(saveFile, 0, 4, completeNormal);
			worlds.PerformNodeAction(saveFile, 0, 7, completeNormal);
			worlds.PerformNodeAction(saveFile, 0, 7, uncompleteNormal);
			assert((saveFile.GetWorldFlags(1) & SaveFile.WorldFlags.AllForUnlocked) == 0);
			assert((saveFile.GetWorldFlags(0) & SaveFile.WorldFlags.AllForCastle) == 0);
			assert((saveFile.GetWorldFlags(0) & SaveFile.WorldFlags.ExitWorldCutscene) == 0);
			assert((saveFile.GetWorldFlags(0) & SaveFile.WorldFlags.AllForTower) == SaveFile.WorldFlags.AllForTower);
		}

		[TestMethod]
		public void TestClearingCannon()
		{
			worlds.PerformNodeAction(saveFile, 0, 9, completeNormal);
			// World 5 should be unlocked
			assert((saveFile.GetWorldFlags(4) & SaveFile.WorldFlags.AllForUnlocked) == SaveFile.WorldFlags.AllForUnlocked);
			// Worlds before that should have cutscene flags set
			assert((saveFile.GetWorldFlags(3) & SaveFile.WorldFlags.AllBowserJuniorCutscenes) == SaveFile.WorldFlags.AllBowserJuniorCutscenes);
			assert((saveFile.GetWorldFlags(2) & SaveFile.WorldFlags.AllBowserJuniorCutscenes) == SaveFile.WorldFlags.AllBowserJuniorCutscenes);
			assert((saveFile.GetWorldFlags(1) & SaveFile.WorldFlags.AllBowserJuniorCutscenes) == SaveFile.WorldFlags.AllBowserJuniorCutscenes);
			assert((saveFile.GetWorldFlags(0) & SaveFile.WorldFlags.AllBowserJuniorCutscenes) == SaveFile.WorldFlags.AllBowserJuniorCutscenes);
		}

		[TestMethod]
		public void TestUnclearingCannon()
		{
			worlds.PerformNodeAction(saveFile, 0, 9, completeNormal);
			worlds.PerformNodeAction(saveFile, 0, 9, uncompleteNormal);
			// World 5 should be locked
			assert((saveFile.GetWorldFlags(4) & SaveFile.WorldFlags.AllForUnlocked) == 0);
			// Worlds before that should have cutscene flags set
			assert((saveFile.GetWorldFlags(3) & SaveFile.WorldFlags.AllBowserJuniorCutscenes) == 0);
			assert((saveFile.GetWorldFlags(2) & SaveFile.WorldFlags.AllBowserJuniorCutscenes) == 0);
			assert((saveFile.GetWorldFlags(1) & SaveFile.WorldFlags.AllBowserJuniorCutscenes) == 0);
			assert((saveFile.GetWorldFlags(0) & SaveFile.WorldFlags.BowserCompletedUnused) == 0);
			// But world 1 should still have entrance cutscene set
			assert((saveFile.GetWorldFlags(0) & SaveFile.WorldFlags.CutsceneEnter) == SaveFile.WorldFlags.CutsceneEnter);
		}
	}
}
