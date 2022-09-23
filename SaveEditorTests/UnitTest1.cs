using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NewSuperMarioBrosSaveEditor;

namespace SaveEditorTests
{
	[TestClass]
	public class UnitTest1
	{
		private MemoryStream ms = new MemoryStream(File.ReadAllBytes("Saves/New.sav"));

		private SaveFile saveFile;

		[TestInitialize]
		public void PreTest()
		{
			saveFile = SaveFile.FromSav(ms, 0);
		}

		[TestMethod]
		public void TestSomeInitialUnlocks()
		{
			// Some nodes
			Assert.IsTrue(saveFile.IsNodeCompleted(0, 0));
			Assert.IsTrue(!saveFile.IsNodeCompleted(0, 1));
			// Some paths
			Assert.IsTrue(saveFile.IsPathUnlocked(0, 0));
			Assert.IsTrue(!saveFile.IsPathUnlocked(0, 1));
			// Brand-new file doesn't have world 1 unlocked
			Assert.IsTrue((saveFile.GetWorldFlags(0) & SaveFile.WorldFlags.AllForUnlocked) == 0);
		}
	}
}
