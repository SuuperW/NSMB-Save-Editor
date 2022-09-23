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

		private CompletionAction completeAll = new CompletionAction()
		{
			Complete = true,
			NormalExit = true,
			SecretExit = true,
			StarCoins = true,
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
		}
	}
}
