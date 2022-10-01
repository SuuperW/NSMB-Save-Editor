using Microsoft.VisualStudio.TestTools.UnitTesting;
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
		private SaveFileWithWorlds file;

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
			file = new SaveFileWithWorlds(saveFile, worlds);
		}

		[TestMethod]
		public void TestSomeInitialUnlocks()
		{
			// Some nodes
			assert(saveFile.IsNodeCompleted(0, 0));
			assert(!saveFile.IsNodeCompleted(0, 1));
			assert(saveFile.IsNodeCompleted(7, 0));
			assert(!saveFile.IsNodeCompleted(7, 1));
			// Some paths
			assert(saveFile.IsPathUnlocked(0, 0));
			assert(!saveFile.IsPathUnlocked(0, 1));
			assert(saveFile.IsPathUnlocked(7, 0));
			assert(!saveFile.IsPathUnlocked(7, 1));
			// Brand-new file doesn't have world 1 unlocked
			assert((saveFile.GetWorldFlags(0) & SaveFile.WorldFlags.AllForUnlocked) == 0);
		}

		[TestMethod]
		public void TestClearingW11()
		{
			file.PerformNodeAction(0, 1, completeAll);
			assert(saveFile.IsNodeCompleted(0, 1));
			assert(saveFile.IsPathUnlocked(0, 1));
			// Path to purple mushroom
			assert(!saveFile.IsPathUnlocked(0, 21));
			// Star coins
			assert((saveFile.GetNodeFlags(0, 1) & SaveFile.NodeFlags.AllStarCoins) == SaveFile.NodeFlags.AllStarCoins);
			assert(saveFile.StarCoins == 3);
			// We should also verify that it sets the other flag, because 8-Castle bridge
			assert((saveFile.GetNodeFlags(0, 1) & SaveFile.NodeFlags.OtherCompleted) == SaveFile.NodeFlags.OtherCompleted);
		}

		[TestMethod]
		public void TestClearingWithMultiplePaths()
		{
			file.PerformNodeAction(0, 2, completeAll);
			// 1-2 node
			assert(saveFile.IsNodeCompleted(0, 2));
			// Path to 1-3
			assert(saveFile.IsPathUnlocked(0, 2));
			// Path to 1-MushroomHouse, then Tower
			assert(saveFile.IsPathUnlocked(0, 0x0A));
			assert(saveFile.IsPathUnlocked(0, 0x16));
			assert(saveFile.IsPathUnlocked(0, 0x04));
			// star coins
			assert(saveFile.StarCoins == 3);
		}

		[TestMethod]
		public void TestUnclearW11()
		{
			file.PerformNodeAction(0, 1, completeAll);
			file.PerformNodeAction(0, 1, uncompleteAll);
			// node
			assert(!saveFile.IsNodeCompleted(0, 1));
			// path
			assert(!saveFile.IsPathUnlocked(0, 1));
			// Star coins
			assert((saveFile.GetNodeFlags(0, 1) & SaveFile.NodeFlags.AllStarCoins) == 0);
			assert(saveFile.StarCoins == 0);
		}

		[TestMethod]
		public void TestClearNodeWithSign()
		{
			// A path with a sign on it should not be unlocked when the relevant level is completed
			file.PerformNodeAction(0, 3, completeAll);
			assert(!saveFile.IsPathUnlocked(0, 0x0B));
			assert(saveFile.StarCoins == 3);
			assert(saveFile.SpentStarCoins == 0);
		}

		[TestMethod]
		public void TestUnclearLevelWhenAnotherLevelHasUnlockedSamePath()
		{
			file.PerformNodeAction(0, 3, completeNormal);
			file.PerformNodeAction(0, 2, completeSecret);
			file.PerformNodeAction(0, 2, uncompleteSecret);
			// Path from 1-dot to 1-Tower should remain unlocked
			assert(saveFile.IsPathUnlocked(0, 4));
		}

		[TestMethod]
		public void TestClearingTower()
		{
			// The secret goal should not set world flags
			file.PerformNodeAction(0, 4, completeSecret);
			assert((saveFile.GetWorldFlags(0) & SaveFile.WorldFlags.AllForTower) == 0);
			// But the normal goal should
			file.PerformNodeAction(0, 4, completeNormal);
			assert((saveFile.GetWorldFlags(0) & SaveFile.WorldFlags.AllForTower) == SaveFile.WorldFlags.AllForTower);
		}

		[TestMethod]
		public void TestClearingW1Castle()
		{
			// We should unlock world 2, and have world 1 flags set
			file.PerformNodeAction(0, 7, completeNormal);
			assert((saveFile.GetWorldFlags(1) & SaveFile.WorldFlags.AllForUnlocked) == SaveFile.WorldFlags.AllForUnlocked);
			assert((saveFile.GetWorldFlags(0) & SaveFile.WorldFlags.AllForCastle) == SaveFile.WorldFlags.AllForCastle);
			assert((saveFile.GetWorldFlags(0) & SaveFile.WorldFlags.AllBowserJuniorCutscenes) == SaveFile.WorldFlags.AllBowserJuniorCutscenes);
			assert((saveFile.GetWorldFlags(0) & SaveFile.WorldFlags.ExitWorldCutscene) == SaveFile.WorldFlags.ExitWorldCutscene);
		}

		[TestMethod]
		public void TestUnclearingTower()
		{
			file.PerformNodeAction(0, 4, completeAll);
			// After uncompleting secret, tower flags should still be set
			file.PerformNodeAction(0, 4, uncompleteSecret);
			assert((saveFile.GetWorldFlags(0) & SaveFile.WorldFlags.AllForTower) == SaveFile.WorldFlags.AllForTower);
			// After uncompleting normal, tower flags should not be set
			file.PerformNodeAction(0, 4, uncompleteNormal);
			assert((saveFile.GetWorldFlags(0) & SaveFile.WorldFlags.AllForTower) == 0);
		}

		[TestMethod]
		public void TestUnclearingW1Castle()
		{
			// Clear 1-Tower so ensure that unclearing 1-Castle is checking that completion
			file.PerformNodeAction(0, 4, completeNormal);
			file.PerformNodeAction(0, 7, completeNormal);
			file.PerformNodeAction(0, 7, uncompleteNormal);
			assert((saveFile.GetWorldFlags(1) & SaveFile.WorldFlags.AllForUnlocked) == 0);
			assert((saveFile.GetWorldFlags(0) & SaveFile.WorldFlags.AllForCastle) == 0);
			assert((saveFile.GetWorldFlags(0) & SaveFile.WorldFlags.ExitWorldCutscene) == 0);
			assert((saveFile.GetWorldFlags(0) & SaveFile.WorldFlags.AllForTower) == SaveFile.WorldFlags.AllForTower);
		}

		[TestMethod]
		public void TestClearingCannon()
		{
			file.PerformNodeAction(0, 9, completeNormal);
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
			file.PerformNodeAction(0, 9, completeNormal);
			file.PerformNodeAction(0, 9, uncompleteNormal);
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

		[TestMethod]
		public void TestUnlockingSecretWorld()
		{
			// When a normal world is unlocked, all previous worlds have cutscene flags set.
			// When a secret world is unlocked, it should be the same except for the corresponding normal world.
			file.PerformNodeAction(1, 0x0A, completeSecret);
			// World 4 should be unlocked
			assert((saveFile.GetWorldFlags(3) & SaveFile.WorldFlags.AllForUnlocked) == SaveFile.WorldFlags.AllForUnlocked);
			// World 3 should have no flags
			assert(saveFile.GetWorldFlags(2) == 0);
		}

		[TestMethod]
		public void TestUnclearingCastleNormalWhenCannonHasAlsoUnlockedWorld()
		{
			// Unlock world 6 by cannon
			file.PerformNodeAction(2, 0x11, completeNormal);
			// Unlock world 6 by castle, then unclear castle
			file.PerformNodeAction(4, 0x0B, completeNormal);
			file.PerformNodeAction(4, 0x0B, uncompleteNormal);
			// World 6 should remain unlocked
			assert((saveFile.GetWorldFlags(5) & SaveFile.WorldFlags.AllForUnlocked) == SaveFile.WorldFlags.AllForUnlocked);
		}

		[TestMethod]
		public void TestUnclearingCannonWhenCastleHasAlsoUnlockedWorld()
		{
			// Unlock world 6 by castle
			file.PerformNodeAction(4, 0x0B, completeNormal);
			// Unlock world 6 by cannon, then unclear cannon
			file.PerformNodeAction(2, 0x11, completeNormal);
			file.PerformNodeAction(2, 0x11, uncompleteNormal);
			// World 6 should remain unlocked
			assert((saveFile.GetWorldFlags(5) & SaveFile.WorldFlags.AllForUnlocked) == SaveFile.WorldFlags.AllForUnlocked);
		}

		[TestMethod]
		public void TestUnclearingCastleSecretWhenCannonHasAlsoUnlockedWorld()
		{
			// Unlock world 7 by cannon
			file.PerformNodeAction(3, 0x10, completeNormal);
			// Unlock world 7 by castle, then unclear castle
			file.PerformNodeAction(4, 0x0B, completeSecret);
			file.PerformNodeAction(4, 0x0B, uncompleteSecret);
			// World 7 should remain unlocked
			assert((saveFile.GetWorldFlags(6) & SaveFile.WorldFlags.AllForUnlocked) == SaveFile.WorldFlags.AllForUnlocked);
		}

		[TestMethod]
		public void TestUnclearingCannonWhenCastleHasAlsoUnlockedWorldSecret()
		{
			// Unlock world 7 by castle
			file.PerformNodeAction(4, 0x0B, completeSecret);
			assert((saveFile.GetWorldFlags(6) & SaveFile.WorldFlags.AllForUnlocked) == SaveFile.WorldFlags.AllForUnlocked);
			// Unlock world 7 by cannon, then unclear cannon
			file.PerformNodeAction(3, 0x10, completeNormal);
			file.PerformNodeAction(3, 0x10, uncompleteNormal);
			// World 7 should remain unlocked
			assert((saveFile.GetWorldFlags(6) & SaveFile.WorldFlags.AllForUnlocked) == SaveFile.WorldFlags.AllForUnlocked);
		}

		[TestMethod]
		public void TestNoStarCoinsFromEmptyNode()
		{
			file.PerformNodeAction(0, 0x0E, completeAll);
			assert(saveFile.StarCoins == 0);
		}

		[TestMethod]
		public void TestOpeningSignPathSpendsStarCoins()
		{
			saveFile.SetPathUnlocked(0, 0x0B, true);
			file.PerformSaveFileLoadCalculations();
			assert(saveFile.SpentStarCoins == 5);
		}

		[TestMethod]
		public void TestUnlockingBackgroundSpendsStarCoins()
		{
			saveFile.BackgroundsBought = SaveFile.BackgroundPurchases.BlueBricks;
			file.PerformSaveFileLoadCalculations();
			assert(saveFile.SpentStarCoins == 20);
		}

		[TestMethod]
		public void TestClearingW52Secret()
		{
			// The connection from the 2nd pipe to 5-3 is considered "backwards".
			file.PerformNodeAction(4, 3, completeSecret);
			assert(saveFile.IsPathUnlocked(4, 11));
			assert(saveFile.IsPathUnlocked(4, 15));
			assert(saveFile.IsPathUnlocked(4, 25));
		}

		[TestMethod]
		public void TestClearingW5Castle()
		{
			file.PerformNodeAction(4, 11, completeNormal);
			assert(!saveFile.IsPathUnlocked(4, 0x18));
		}

		[TestMethod]
		public void TestPerformingActionDoesNotChangeAction()
		{
			file.PerformNodeAction(0, 1, completeAll);
			assert(completeAll.SecretExit);
		}

		[TestMethod]
		public void TestClear100HasThreeStars()
		{
			file.Clear100();
			assert(file.file.PlayerHasSeenCredits);
			assert(file.file.SecondStar);
			assert(file.file.ThirdStar);
		}

		[TestMethod]
		public void TestClearBowserCastleUnlocksPurpleMushroom()
		{
			file.PerformNodeAction(7, 12, completeNormal);
			assert(file.file.IsPathUnlocked(0, 21));
		}
	}
}
