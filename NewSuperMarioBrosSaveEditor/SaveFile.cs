using System;
using System.IO;

namespace NewSuperMarioBrosSaveEditor
{
	/// <summary>
	/// Represents an in-game "file". That is, one of the three files that the user can select to start the game.
	/// </summary>
	class SaveFile
	{
		public static int SIZE = 0x252;

		private ushort Checksum
		{
			set => BitConverter.GetBytes(value).CopyTo(data, 0);
		}
		public bool ChecksumWasValid { get; private set; }
		// Data indexes have been informed by https://nsmbhd.net/thread/3835-save-file-structure-haxx/.
		// However, we use a different offset since we include the checksum and "Mario2d" string.
		public bool Initialized
		{
			get => BitConverter.ToUInt32(data, 0x00E) != 0;
			set => BitConverter.GetBytes(value ? 1 : 0).CopyTo(data, 0x00E);
		}
		public uint StarsByFileSelect
		{
			get => BitConverter.ToUInt32(data, 0x012);
			set => BitConverter.GetBytes(value).CopyTo(data, 0x012);
		}
		public int Lives
		{
			get => BitConverter.ToInt32(data, 0x016);
			set => BitConverter.GetBytes(value).CopyTo(data, 0x016);
		}
		public int Coins
		{
			get => BitConverter.ToInt32(data, 0x01A);
			set => BitConverter.GetBytes(value).CopyTo(data, 0x01A);
		}
		public int Score
		{
			get => BitConverter.ToInt32(data, 0x01E);
			set => BitConverter.GetBytes(value).CopyTo(data, 0x01E);
		}
		// Setting this makes no sense. The value is re-calculated when the file is loaded.
		public int StarCoins
		{
			get => BitConverter.ToInt32(data, 0x022);
			set => BitConverter.GetBytes(value).CopyTo(data, 0x022);
		}
		// Setting this makes no sense. The value is re-calculated when the file is loaded.
		public int SpentStarCoins
		{
			get => BitConverter.ToInt32(data, 0x026);
			set => BitConverter.GetBytes(value).CopyTo(data, 0x026);
		}
		// TODO: missing stuff
		public int CurrentPowerup
		{
			get => BitConverter.ToInt32(data, 0x03A);
			set => BitConverter.GetBytes(value).CopyTo(data, 0x03A);
		}
		public byte OverworldBackground
		{
			get => data[0x42];
			set => data[0x42] = value;
		}
		// TODO: missing stuff
		public int Inventory
		{
			get => BitConverter.ToInt32(data, 0x066);
			set => BitConverter.GetBytes(value).CopyTo(data, 0x066);
		}
		public ushort GetWorldFlags(int index)
		{
			if (index >= 0 && index < 8)
				return BitConverter.ToUInt16(data, 0x6A + index * 2);
			else
				throw new IndexOutOfRangeException();
		}
		public void SetWorldFlags(int index, ushort flags)
		{
			if (index >= 0 && index < 8)
				BitConverter.GetBytes(flags).CopyTo(data, 0x6A + index * 2);
			else
				throw new IndexOutOfRangeException();
		}
		// TODO: Star coin flags
		public byte GetLevelFlags(int index)
		{
			if (index >= 0 && index < 0xE4)
				return data[0x142 + index];
			else
				throw new IndexOutOfRangeException();
		}
		public void SetLevelFlags(int index, byte flags)
		{
			if (index >= 0 && index < 0xE4)
				data[0x142 + index] = flags;
			else
				throw new IndexOutOfRangeException();
		}
		// TODO: What's after this?
		/*0x00 - 0x04   "7000" in ASCII. (7001 in NewerDS)
0x20 - 0x24   Current world.Setting byte 2 of this field will play the warp animation.
0x24 - 0x28   Unknown.
0x28 - 0x2C   Current world map node.
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
	}
}
