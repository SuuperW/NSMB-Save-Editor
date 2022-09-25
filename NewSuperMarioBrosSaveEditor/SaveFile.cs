using System;
using System.IO;

namespace NewSuperMarioBrosSaveEditor
{
	/// <summary>
	/// Represents an in-game "file". That is, one of the three files that the user can select to start the game.
	/// </summary>
	public class SaveFile
	{
		// Note: We keep the entire data buffer, including checksum and the c-string identifier "Mario2d".
		// The game's structure does not include these. So, we add 0xA to all our offsets.
		// Data indexes have been informed by https://nsmbhd.net/thread/3835-save-file-structure-haxx/.
		public static int SIZE = 0x252;

		private ushort Checksum
		{
			set => BitConverter.GetBytes(value).CopyTo(data, 0);
		}
		public bool ChecksumWasValid { get; private set; }
		// However, we use a different offset since we include the checksum and "Mario2d" string.
		public bool Initialized
		{
			get => BitConverter.ToUInt32(data, 0x004 + 0xA) != 0;
			set => BitConverter.GetBytes(value ? 1 : 0).CopyTo(data, 0x004 + 0xA);
		}

		public bool IsNewFile
		{
			get => data[0x004 + 0xA] == 0;
			set => data[0x004 + 0xA] = (byte)(value ? 0 : 1);
		}

		/// <summary>
		/// Data about file progress, which is mostly but not all re-calculated upon loading the file.
		/// Known flags & values have their own properties. Unknown bits, big endian: 4A FF 07 00
		/// Thus, many of the flags here wouldn't actually do anything if we set them here.
		/// </summary>
		public uint StarsByFileSelect
		{
			get => BitConverter.ToUInt32(data, 0x008 + 0xA);
			set => BitConverter.GetBytes(value).CopyTo(data, 0x008 + 0xA);
		}
		/// <summary>
		/// The allows the user to save at any time in the overworld, is required for the file to show any stars on file select screen, and gives 1 star.
		/// </summary>
		public bool PlayerHasSeenCredits
		{
			get => (data[0x008 + 0xA] & 0x20) != 0;
			set => data[0x008 + 0xA] = (byte)((data[0x008 + 0xA] & ~0x20) | (value ? 0x20 : 0));
		}
		/// <summary>
		/// The second star is awarded when all (star coin containing) levels have been cleared.
		/// Secret exits are not required.
		/// </summary>
		public bool SecondStar
		{
			get => (data[0x008 + 0xA] & 0x01) != 0;
			set => data[0x008 + 0xA] = (byte)((data[0x008 + 0xA] & ~0x01) | (value ? 0x01 : 0));
		}
		/// <summary>
		/// The third star is awarded when all star coins have been collected and spent.
		/// The game will re-calculate this after loading the file, so if you only set this you will lose the star upon re-saving the file.
		/// </summary>
		public bool ThirdStar
		{
			get => (data[0x008 + 0xA] & 0x10) != 0;
			set => data[0x008 + 0xA] = (byte)((data[0x008 + 0xA] & ~0x10) | (value ? 0x10 : 0));
		}
		public bool AllStarCoinsSpentFlag
		{
			get => (data[0x008 + 0xA] & 0x80) != 0;
			set => data[0x008 + 0xA] = (byte)((data[0x008 + 0xA] & ~0x80) | (value ? 0x80 : 0));
		}
		/// <summary>
		/// The last overworld background must be unlocked. (by completing all levels?)
		/// The game re-calculates this when the file is loaded, so there's no point in setting it.
		/// </summary>
		public bool RetroBackgroundUnlocked
		{
			get => (data[0x008 + 0xA] & 0x02) != 0;
		}
		public BackgroundPurchases BackgroundsBought
		{
			get => (BackgroundPurchases)data[0x00A + 0xA] & BackgroundPurchases.All;
			set => data[0x00A + 0xA] = (byte)(((BackgroundPurchases)data[0x00A + 0xA] & ~BackgroundPurchases.All) | value);
		}
		[Flags]
		public enum BackgroundPurchases
		{
			First = 0x08,
			BlueBricks = 0x08,
			Stars = 0x10,
			Mario = 0x20,
			Retro = 0x40,
			All = 0x78,
		}
		public WorldPathHighlightsEnum WorldPathHighlights
		{
			get => (WorldPathHighlightsEnum)BitConverter.ToUInt16(data, 0x00A + 0xA) & WorldPathHighlightsEnum.All;
			set
			{
				uint oldValue = BitConverter.ToUInt16(data, 0x00A + 0xA) & ~(uint)WorldPathHighlightsEnum.All;
				BitConverter.GetBytes((ushort)((uint)value | oldValue)).CopyTo(data, 0x00A + 0xA);
			}
		}
		[Flags]
		public enum WorldPathHighlightsEnum
		{
			W1toW2 = 0x0080,
			W2toW3 = 0x0100,
			W2toW4 = 0x0200,
			W3toW5 = 0x0400,
			W4toW5 = 0x0800,
			W5toW6 = 0x1000,
			W5toW7 = 0x2000,
			W6toW8 = 0x4000,
			W7toW8 = 0x8000,
			All = 0xFF80,
		}

		public int Lives
		{
			get => BitConverter.ToInt32(data, 0x00C + 0xA);
			set => BitConverter.GetBytes(value).CopyTo(data, 0x00C + 0xA);
		}
		public int Coins
		{
			get => BitConverter.ToInt32(data, 0x010 + 0xA);
			set => BitConverter.GetBytes(value).CopyTo(data, 0x010 + 0xA);
		}
		public int Score
		{
			get => BitConverter.ToInt32(data, 0x014 + 0xA);
			set => BitConverter.GetBytes(value).CopyTo(data, 0x014 + 0xA);
		}
		// Setting this makes no sense. The value is re-calculated when the file is loaded.
		public int StarCoins
		{
			get => BitConverter.ToInt32(data, 0x018 + 0xA);
			set => BitConverter.GetBytes(value).CopyTo(data, 0x018 + 0xA);
		}
		// Setting this makes no sense. The value is re-calculated when the file is loaded.
		public int SpentStarCoins
		{
			get => BitConverter.ToInt32(data, 0x01C + 0xA);
			set => BitConverter.GetBytes(value).CopyTo(data, 0x01C + 0xA);
		}
		public int WorldId
		{
			get => BitConverter.ToInt32(data, 0x020 + 0xA);
			set => BitConverter.GetBytes(value).CopyTo(data, 0x020 + 0xA);
		}
		// TODO: 0x024 + 0xA unknown
		public int LevelIdByWorld
		{
			get => BitConverter.ToInt32(data, 0x028 + 0xA);
			set => BitConverter.GetBytes(value).CopyTo(data, 0x028 + 0xA);
		}
		// TODO: missing stuff
		public int CurrentPowerup
		{
			get => BitConverter.ToInt32(data, 0x030 + 0xA);
			set => BitConverter.GetBytes(value).CopyTo(data, 0x030 + 0xA);
		}
		// TODO: 0x034 + 0xA unknown
		public byte OverworldBackground
		{
			get => data[0x038 + 0xA];
			set => data[0x038 + 0xA] = value;
		}
		// TODO: missing stuff
		public int Inventory
		{
			get => BitConverter.ToInt32(data, 0x05C + 0xA);
			set => BitConverter.GetBytes(value).CopyTo(data, 0x05C + 0xA);
		}
		public ushort GetWorldFlags(int index)
		{
			if (index >= 0 && index < 8)
				return BitConverter.ToUInt16(data, 0x060 + 0xA + index * 2);
			else
				throw new IndexOutOfRangeException();
		}
		public void SetWorldFlags(int index, ushort flags)
		{
			if (index >= 0 && index < 8)
				BitConverter.GetBytes(flags).CopyTo(data, 0x060 + 0xA + index * 2);
			else
				throw new IndexOutOfRangeException();
		}
		/// <summary>
		/// There are 0x19 per world, though not all worlds actually have 0x19 nodes.
		/// </summary>
		public byte GetNodeFlags(int index)
		{
			if (index >= 0 && index < 0xC8)
				return data[0x70 + 0xA + index];
			else
				throw new IndexOutOfRangeException();
		}
		public byte GetNodeFlags(int world, int index) => GetNodeFlags(0x19 * world + index);
		public void SetNodeFlags(int index, byte flags)
		{
			if (index >= 0 && index < 0xC8)
				data[0x70 + 0xA + index] = flags;
			else
				throw new IndexOutOfRangeException();
		}
		public void SetNodeFlags(int world, int index, byte flags) => SetNodeFlags(0x19 * world + index, flags);
		// Note: The link above says this begins at 0x137. It's off by one.
		// There are 0x1E per world, though not all worlds actually have 0x1E paths.
		public byte GetPathFlags(int index)
		{
			if (index >= 0 && index < 0xF0)
				return data[0x138 + 0xA + index];
			else
				throw new IndexOutOfRangeException();
		}
		public byte GetPathFlags(int world, int index) => GetPathFlags(0x1E * world + index);
		public void SetPathFlags(int index, byte flags)
		{
			if (index >= 0 && index < 0xF0)
				data[0x138 + 0xA + index] = flags;
			else
				throw new IndexOutOfRangeException();
		}
		public void SetPathFlags(int world, int index, byte flags) => SetPathFlags(0x1E * world + index, flags);
		// TODO: What's after this?
		/*0x00 - 0x04   "7000" in ASCII. (7001 in NewerDS)
0x24 - 0x28   Unknown.
0x2C - 0x30   Unknown.
0x3C - 0x40   Current world possible duplicate.This value changes when 0x20 changes, and cannot be edited because 0x20 automatically sets it back to the previous value.
0x40 - 0x44   Unknown.
0x44 - 0x48   Unknown.
0x48 - 0x4C   Unknown.
0x4C - 0x50   Unknown.
0x50 - 0x54   Unknown.
0x54 - 0x58   Unknown.
0x58 - 0x5C   Unknown.
0x70 - 0x137  StarCoins[200]
*/

		private byte[] data = new byte[0x252];
		
		public static class NodeFlags
		{
			public static byte StarCoin1 = 0x01;
			public static byte StarCoin2 = 0x02;
			public static byte StarCoin3 = 0x04;
			public static byte AllStarCoins = 0x07;
			// NSMB will set this flag upon loading the world if the level is completed.
			// There's at least one case where it matters if it is set before loading the world:
			//	The bridge in world 8 will only be visible if this flag was already set.
			public static byte OtherCompleted = 0x40;
			public static byte Completed = 0x80;

			public static byte AllForCompleted = 0xC0;
		}
		public static class PathFlags
		{
			public static byte Unlocked = 0xC0;
		}
		public static class WorldFlags
		{
			public static ushort Visited = 0x0001;
			public static ushort CutsceneEnter = 0x0002;
			public static ushort CutsceneTower = 0x0004;
			public static ushort CutsceneCastle = 0x0008;
			public static ushort CutsceneTower2 = 0x0010;
			public static ushort CutsceneUnused = 0x0020;
			public static ushort Unlocked = 0x0040;
			public static ushort ExitWorldCutscene = 0x0080;
			public static ushort TowerCompleted = 0x0100;
			public static ushort CastleCompleted = 0x0200;
			public static ushort Tower2Completed = 0x0400;
			public static ushort BowserCompletedUnused = 0x0800;
			public static ushort FireworksHouse = 0x1000;

			public static ushort AllForUnlocked = 0x0043;
			public static ushort AllForTower = 0x0104;
			public static ushort AllForCastle = 0x0208;
			public static ushort AllForTower2 = 0x0410;
			public static ushort AllBowserJuniorCutscenes = 0x003E;
		}

		private SaveFile() { }

		/// <summary>
		/// Create a SaveFile object from the raw bytes from the game.
		/// </summary>
		/// <param name="stream">A stream that contains the game's entire save data.</param>
		/// <param name="fileIndex">Which file to get. 0-2, or 3-5 to read backup data.</param>
		public static SaveFile FromSav(Stream stream, int fileIndex)
		{
			SaveFile saveFile = new SaveFile();

			int streamIndex;
			if (fileIndex >= 0 && fileIndex <= 2)
				streamIndex = 0x100 + fileIndex * 0x280;
			else if (fileIndex >= 3 && fileIndex <= 5)
				streamIndex = 0x1100 + (fileIndex - 3) * 0x280;
			else
				throw new ArgumentOutOfRangeException("fileIndex must be between 0 and 5, inclusive.");

			// Wrap stream in a BinaryReader so we can get 16/32-bit ints
			stream.Seek(streamIndex, SeekOrigin.Begin);
			BinaryReader reader = new BinaryReader(stream);

			ushort checksum = reader.ReadUInt16();

			// Next bytes must equal 4D 61 72 69 6F 32 64 00 37 30 30 30 ("Mario2d" with null terminator and "7000" w/o)
			if (reader.ReadUInt32() != 0x6972614D ||
				reader.ReadUInt32() != 0x0064326F ||
				reader.ReadUInt32() != 0x30303037)
			{
				throw new InvalidDataException("The given data is not an NSMB save file.");
			}

			// Copy data
			stream.Seek(streamIndex, SeekOrigin.Begin);
			stream.Read(saveFile.data, 0, saveFile.data.Length);

			// Validate checksum
			saveFile.ChecksumWasValid = checksum == Util.Checksum(saveFile.data, 10, 0x248);

			return saveFile;
		}


		public byte[] GetData()
		{
			// Checksum calculation begins at "7000"
			Checksum = Util.Checksum(data, 10, 0x248);
			return data.Clone() as byte[];
		}

		public bool IsPathUnlocked(int world, int index) => (GetPathFlags(world, index) & PathFlags.Unlocked) != 0;
		public void SetPathUnlocked(int world, int index, bool value) => SetPathFlags(world, index, value ? PathFlags.Unlocked : (byte)0);

		public bool IsNodeCompleted(int world, int index) => (GetNodeFlags(world, index) & NodeFlags.Completed) != 0;
		private int[] starCoinCounts = new int[] { 0, 1, 1, 2, 1, 2, 2, 3 };
		public int CountStarCoins(int world, int index) => starCoinCounts[GetNodeFlags(world, index) & NodeFlags.AllStarCoins];

		public void ResetAllNodesAndPathsInWorld(int worldId)
		{
			// nodes
			Array.Clear(data, 0x070 + 0xA + 0x19 * worldId, 0x19);
			// paths
			Array.Clear(data, 0x138 + 0xA + 0x1E * worldId, 0x1E);
		}
	}

}
