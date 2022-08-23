namespace NewSuperMarioBrosSaveEditor
{
	static class Util
	{
		public static ushort Checksum(byte[] data, int beginIndex, int dataSize)
		{
			ushort checksum = 654;

			for (int i = 0; i < dataSize; i++)
			{
				byte readByte = data[beginIndex + i];
				checksum = (ushort)(readByte ^ ((2 * checksum & 0xFFFE) | (checksum >> 15) & 1));
			}

			return checksum;
		}
	}
}
