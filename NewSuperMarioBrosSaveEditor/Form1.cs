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

			powerupComboBox.Items.Add("Small");
			powerupComboBox.Items.Add("Super");
			powerupComboBox.Items.Add("Fire");
			powerupComboBox.Items.Add("Mini");
			powerupComboBox.Items.Add("Blue Shell");

			inventoryComboBox.Items.Add("Nothing");
			inventoryComboBox.Items.Add("Super Mushroom");
			inventoryComboBox.Items.Add("Fire Flower");
			inventoryComboBox.Items.Add("Blue Shell");
			inventoryComboBox.Items.Add("Mini Mushroom");
			inventoryComboBox.Items.Add("Mega Mushroom");

			saveButton.Enabled = false;
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
			powerupComboBox.Enabled = false;
			inventoryComboBox.Enabled = false;
			BSBPictureBox.Enabled = false;
			unlockLCheckBox.Enabled = false;
			unlockWCheckBox.Enabled = false;

			saveButton.Click += new System.EventHandler(SaveButtonClicked);
		}

		OpenFileDialog dlg = new OpenFileDialog();
		int fileIndex = 0;
		Bitmap[] BGs = { Properties.Resources.NSMB_BG1, Properties.Resources.NSMB_BG2, Properties.Resources.NSMB_BG3, Properties.Resources.NSMB_BG4, Properties.Resources.NSMB_BG5 };

		public ushort nsmbChecksum(byte[] data, int dataSize, int pos)
		{
			ushort checksum = 654;

			for (int i = 0; i < dataSize; ++i)
			{
				byte readByte = data[pos + i];
				checksum = Convert.ToUInt16(readByte ^ ((2 * checksum & 0xFFFE) | (checksum >> 15) & 1));
			}

			return checksum;
		}

		public byte[] recalculateSaveFileChecksums(byte[] savefile)
		{
			for (int baseData = 0; baseData <= 0x1000; baseData += 0x1000)
			{
				doChecksum(savefile, baseData + 0x00, 0xF4); //Header

				doChecksum(savefile, baseData + 0x100, 0x248); //Save 1
				doChecksum(savefile, baseData + 0x380, 0x248); //Save 2
				doChecksum(savefile, baseData + 0x600, 0x248); //Save 3

				doChecksum(savefile, baseData + 0x880, 0x14); //Footer
			}

			return savefile;
		}

		public void doChecksum(byte[] savefile, int checksumPos, int dataLen)
		{
			ushort checksum = nsmbChecksum(savefile, dataLen, checksumPos + 10);
			savefile[checksumPos] = Convert.ToByte(checksum & 0xFF);
			savefile[checksumPos + 1] = Convert.ToByte(checksum >> 8);
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
			if (radioButton2.Checked) { fileIndex = 0x280; }
			if (radioButton3.Checked) { fileIndex = 0x500; }
		}

		public void ReadPowerups()
		{
			RefreshFileIndex();

			BinaryReader bnr = new BinaryReader(File.OpenRead(dlg.FileName));
			bnr.BaseStream.Position = 0x13A + fileIndex;
			int pID = bnr.ReadByte();
			powerupComboBox.SelectedIndex = ((pID > 3) ? (pID - 1) : (pID));

			bnr.Close();
		}

		public void ReadInventory()
		{
			RefreshFileIndex();

			BinaryReader bnr = new BinaryReader(File.OpenRead(dlg.FileName));
			bnr.BaseStream.Position = 0x166 + fileIndex;
			inventoryComboBox.SelectedIndex = bnr.ReadByte();

			bnr.Close();
		}

		private void SaveButtonClicked(object sender, EventArgs e)
		{
			RefreshFileIndex();

			byte[] fileByteRead;

			using (FileStream fs = new FileStream(dlg.FileName, FileMode.Open, FileAccess.Read))
			{
				fileByteRead = File.ReadAllBytes(dlg.FileName);
			}

			BinaryWriter bnw = new BinaryWriter(new MemoryStream(fileByteRead));


			if (unlockLCheckBox.Checked)
			{
				int pos = 0x241 + fileIndex;
				for (int i = 0; i <= 0x114; i++)
				{
					bnw.BaseStream.Position = pos + i;
					bnw.Write(0xC0);
				}
			}

			if (unlockWCheckBox.Checked)
			{
				int pos = 0x16A + fileIndex;
				for (int i = 0; i <= 0x10; i++)
				{
					bnw.BaseStream.Position = pos + i;
					bnw.Write(0xFF);
				}
			}

			bnw.BaseStream.Position = 0x116 + fileIndex;
			int livesValue = Convert.ToInt32(livesNumUpDown.Value);
			bnw.Write(livesValue);
			
			bnw.BaseStream.Position = 0x11A + fileIndex;
			int coinsValue = Convert.ToInt32(coinsNumUpDown.Value);
			bnw.Write(coinsValue);
			
			bnw.BaseStream.Position = 0x122 + fileIndex;
			int starCoinValue = Convert.ToInt32(SCNumUpDown.Value);
			bnw.Write(starCoinValue);

			bnw.BaseStream.Position = 0x11E + fileIndex;
			int scoreValue = Convert.ToInt32(scoreNumUpDown.Value);
			bnw.Write(scoreValue);

			bnw.BaseStream.Position = 0x13A + fileIndex;
			bnw.Write(((powerupComboBox.SelectedIndex > 2) ? (powerupComboBox.SelectedIndex +1) : (powerupComboBox.SelectedIndex)));
			Console.WriteLine(((powerupComboBox.SelectedIndex > 2) ? (powerupComboBox.SelectedIndex + 1) : (powerupComboBox.SelectedIndex)));

			bnw.BaseStream.Position = 0x166 + fileIndex;
			bnw.Write(inventoryComboBox.SelectedIndex);

			bnw.BaseStream.Position = 0x142 + fileIndex;
			bnw.Write(BSBNumUpDown.Value - 1);

			BSBPictureBox.Image = BGs[(int)BSBNumUpDown.Value - 1];


			bnw.Close();

			recalculateSaveFileChecksums(fileByteRead);

			using (FileStream fsWrite = new FileStream(dlg.FileName, FileMode.Open, FileAccess.Write))
			{
				fsWrite.Write(fileByteRead, 0, fileByteRead.Length);
			}
		}

		private void button1_Click_1(object sender, EventArgs e)
		{
			dlg.ShowDialog();

			radioButton1.Enabled = true;
			radioButton2.Enabled = true;
			radioButton3.Enabled = true;

			labelLogs.Text = Path.GetFileName(dlg.FileName).ToString();
		}

		private void radioButton_CheckedChanged(object sender, EventArgs e)
		{
			RefreshFileIndex();

			saveButton.Enabled = true;
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
			powerupComboBox.Enabled = true;
			inventoryComboBox.Enabled = true;
			BSBPictureBox.Enabled = true;
			unlockLCheckBox.Enabled = true;
			unlockWCheckBox.Enabled = true;

			BinaryReader bnr = new BinaryReader(File.OpenRead(dlg.FileName));

			bnr.BaseStream.Position = 0x142 + fileIndex;
			BSBNumUpDown.Value = bnr.ReadByte() + 1;

			bnr.BaseStream.Position = 0x142 + fileIndex;
			BSBPictureBox.Image = BGs[bnr.ReadByte()];

			bnr.BaseStream.Position = 0x116 + fileIndex;
			livesNumUpDown.Value = bnr.ReadInt32();

			bnr.BaseStream.Position = 0x11A + fileIndex;
			coinsNumUpDown.Value = bnr.ReadInt32();

			bnr.BaseStream.Position = 0x122 + fileIndex;
			SCNumUpDown.Value = bnr.ReadInt32();

			bnr.BaseStream.Position = 0x11E + fileIndex;
			scoreNumUpDown.Value = bnr.ReadInt32();

			ReadPowerups();

			ReadInventory();

			bnr.Close();
		}
	}
}
