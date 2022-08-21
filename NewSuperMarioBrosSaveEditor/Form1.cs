using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace NewSuperMarioBrosSaveEditor
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			dlg.Filter = "NSMB Savefile (*.sav)|*.sav";

			saveBtn.Enabled = false;
			radioButton1.Enabled = false;
			radioButton2.Enabled = false;
			radioButton3.Enabled = false;
			labelLives.Enabled = false;
			labelCoins.Enabled = false;
			labelSC.Enabled = false;
			labelScore.Enabled = false;
			labelPowerup.Enabled = false;
			labelInventory.Enabled = false;
			labelBSB.Enabled = false;
			livesNumUpDown.Enabled = false;
			coinsNumUpDown.Enabled = false;
			SCNumUpDown.Enabled = false;
			scoreNumUpDown.Enabled = false;
			BSBNumUpDown.Enabled = false;
			powerupCbx.Enabled = false;
			inventoryCbx.Enabled = false;
			BSBPictureBox.Enabled = false;
			unlockLCheckBox.Enabled = false;
			unlockWCheckBox.Enabled = false;
		}

		OpenFileDialog dlg = new OpenFileDialog();
		int fileIndex = 0;
		Bitmap[] BGs = {
			Properties.Resources.NSMB_BG1,
			Properties.Resources.NSMB_BG2,
			Properties.Resources.NSMB_BG3,
			Properties.Resources.NSMB_BG4,
			Properties.Resources.NSMB_BG5,
		};

		private byte[] headerData;
		private byte[] footerData;
		private byte[][] filesData;

		public ushort nsmbChecksum(byte[] data, int dataSize, int pos)
		{
			ushort checksum = 654;

			for (int i = 0; i < dataSize; i++)
			{
				byte readByte = data[pos + i];
				checksum = (ushort)(readByte ^ ((2 * checksum & 0xFFFE) | (checksum >> 15) & 1));
			}

			return checksum;
		}

		public byte[] recalculateSaveFileChecksums(byte[] savefile)
		{
			doChecksum(savefile, 0x00, 0xF4); //Header

			doChecksum(savefile, 0x100, 0x248); //Save 1
			doChecksum(savefile, 0x380, 0x248); //Save 2
			doChecksum(savefile, 0x600, 0x248); //Save 3

			doChecksum(savefile, 0x880, 0x14); //Footer

			return savefile;
		}

		public void doChecksum(byte[] savefile, int checksumPos, int dataLen)
		{
			ushort checksum = nsmbChecksum(savefile, dataLen, checksumPos + 10);
			savefile[checksumPos] = (byte)(checksum & 0xFF);
			savefile[checksumPos + 1] = (byte)(checksum >> 8);
		}

		public void UncheckFileButtons()
		{
			radioButton1.Checked = false;
			radioButton2.Checked = false;
			radioButton3.Checked = false;
		}

		public void RefreshFileIndex()
		{
			if (radioButton1.Checked) { fileIndex = 0; }
			if (radioButton2.Checked) { fileIndex = 1; }
			if (radioButton3.Checked) { fileIndex = 2; }
		}

		public void ReadPowerups()
		{
			RefreshFileIndex();

			int pID = filesData[fileIndex][0x3A];
			powerupCbx.SelectedIndex = ((pID > 3) ? (pID - 1) : (pID));
		}

		public void ReadInventory()
		{
			RefreshFileIndex();

			inventoryCbx.SelectedIndex = filesData[fileIndex][0x66];
		}

		private void saveBtn_Clicked(object sender, EventArgs e)
		{
			RefreshFileIndex();

			BinaryWriter bnw = new BinaryWriter(new MemoryStream(filesData[fileIndex]));


			if (unlockLCheckBox.Checked)
			{
				int pos = 0x141;
				for (int i = 0; i <= 0x114; i++)
				{
					bnw.BaseStream.Position = pos + i;
					bnw.Write(0xC0);
				}
			}

			if (unlockWCheckBox.Checked)
			{
				int pos = 0x6A;
				for (int i = 0; i <= 0x10; i++)
				{
					bnw.BaseStream.Position = pos + i;
					bnw.Write(0xFF);
				}
			}


			bnw.BaseStream.Position = 0x16;
			int livesValue = (int)(livesNumUpDown.Value);

			bnw.Write(livesValue);
			
			bnw.BaseStream.Position = 0x1A;
			int coinsValue = (int)(coinsNumUpDown.Value);
			bnw.Write(coinsValue);
			
			bnw.BaseStream.Position = 0x22;
			int starCoinValue = (int)(SCNumUpDown.Value);
			bnw.Write(starCoinValue);

			bnw.BaseStream.Position = 0x1E;
			int scoreValue = (int)(scoreNumUpDown.Value);
			bnw.Write(scoreValue);

			bnw.BaseStream.Position = 0x3A;
			bnw.Write(((powerupCbx.SelectedIndex > 2) ? (powerupCbx.SelectedIndex +1) : (powerupCbx.SelectedIndex)));
			Console.WriteLine(((powerupCbx.SelectedIndex > 2) ? (powerupCbx.SelectedIndex + 1) : (powerupCbx.SelectedIndex)));

			bnw.BaseStream.Position = 0x66;
			bnw.Write(inventoryCbx.SelectedIndex);

			bnw.BaseStream.Position = 0x42;
			bnw.Write(BSBNumUpDown.Value - 1);

			BSBPictureBox.Image = BGs[(int)BSBNumUpDown.Value - 1];


			bnw.Close();

			byte[] fileByteRead = File.ReadAllBytes(dlg.FileName);
			Array.Copy(filesData[fileIndex], 0, fileByteRead, 0x100 + fileIndex * 0x280, filesData[fileIndex].Length);
			recalculateSaveFileChecksums(fileByteRead);

			using (FileStream fsWrite = new FileStream(dlg.FileName, FileMode.Open, FileAccess.Write))
			{
				fsWrite.Write(fileByteRead, 0, fileByteRead.Length);
			}
		}

		private void openBtn_Clicked(object sender, EventArgs e)
		{
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				if (File.Exists(dlg.FileName))
				{
					radioButton1.Enabled = true;
					radioButton2.Enabled = true;
					radioButton3.Enabled = true;

					labelLogs.Text = Path.GetFileName(dlg.FileName).ToString();

					using (FileStream fs = new FileStream(dlg.FileName, FileMode.Open, FileAccess.Read))
					{
						headerData = new byte[0xF4];
						fs.Read(headerData, 0, 0xF4);

						filesData = new byte[3][];
						fs.Seek(0x100, SeekOrigin.Begin);
						for (int i = 0; i < filesData.Length; i++)
						{
							filesData[i] = new byte[0x248];
							fs.Read(filesData[i], 0, 0x248);
							fs.Seek(0x280 - 0x248, SeekOrigin.Current);
						}

						footerData = new byte[0x14];
						fs.Read(footerData, 0, 0x14);
					}
				}
				else
					MessageBox.Show("File does not exist.");
			}
		}

		private void radioButton_CheckedChanged(object sender, EventArgs e)
		{
			RefreshFileIndex();

			saveBtn.Enabled = true;
			labelLives.Enabled = true;
			labelCoins.Enabled = true;
			labelSC.Enabled = true;
			labelScore.Enabled = true;
			labelPowerup.Enabled = true;
			labelInventory.Enabled = true;
			labelBSB.Enabled = true;
			livesNumUpDown.Enabled = true;
			coinsNumUpDown.Enabled = true;
			SCNumUpDown.Enabled = true;
			scoreNumUpDown.Enabled = true;
			BSBNumUpDown.Enabled = true;
			powerupCbx.Enabled = true;
			inventoryCbx.Enabled = true;
			BSBPictureBox.Enabled = true;
			unlockLCheckBox.Enabled = true;
			unlockWCheckBox.Enabled = true;

			BinaryReader bnr = new BinaryReader(new MemoryStream(filesData[fileIndex]));

			bnr.BaseStream.Position = 0x42;
			BSBNumUpDown.Value = bnr.ReadByte() + 1;

			bnr.BaseStream.Position = 0x42;
			BSBPictureBox.Image = BGs[bnr.ReadByte()];

			bnr.BaseStream.Position = 0x16;
			livesNumUpDown.Value = bnr.ReadInt32();

			bnr.BaseStream.Position = 0x1A;
			coinsNumUpDown.Value = bnr.ReadInt32();

			bnr.BaseStream.Position = 0x22;
			SCNumUpDown.Value = bnr.ReadInt32();

			bnr.BaseStream.Position = 0x1E;
			scoreNumUpDown.Value = bnr.ReadInt32();

			ReadPowerups();

			ReadInventory();

			bnr.Close();
		}
	}
}
