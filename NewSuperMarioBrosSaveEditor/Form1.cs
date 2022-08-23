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
			dlg.Filter = "Raw savefile (*.sav), BizHawk save (*.SaveRAM)|*.sav;*.SaveRAM";
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

		private SaveFile[] files;

		public void UncheckFileButtons()
		{
			fileSelectPnl.Enabled = false;
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

			int pID = files[fileIndex].CurrentPowerup;
			powerupCbx.SelectedIndex = ((pID > 3) ? (pID - 1) : (pID));
		}

		public void ReadInventory()
		{
			RefreshFileIndex();

			inventoryCbx.SelectedIndex = files[fileIndex].Inventory;
		}

		private void saveBtn_Clicked(object sender, EventArgs e)
		{
			RefreshFileIndex();

			SaveFile file = files[fileIndex];

			// Quick 'unlock all' levels/worlds
			if (unlockLCheckBox.Checked)
			{
				for (int i = 0; i < 0xE4; i++)
					file.SetLevelFlags(i, 0xC0);
			}
			if (unlockWCheckBox.Checked)
			{
				for (int i = 0; i < 8; i++)
					file.SetWorldFlags(i, 0x43);
			}

			// Set other file data
			file.Lives = (int)(livesNumUpDown.Value);
			file.Coins = (int)(coinsNumUpDown.Value);
			file.StarCoins = (int)(SCNumUpDown.Value);
			file.Score = (int)(scoreNumUpDown.Value);
			file.CurrentPowerup = powerupCbx.SelectedIndex > 2 ? powerupCbx.SelectedIndex + 1 : powerupCbx.SelectedIndex;
			file.Inventory = inventoryCbx.SelectedIndex;
			file.OverworldBackground = (byte)BSBNumUpDown.Value;

			// Copy data into the file
			byte[] fileByteRead = File.ReadAllBytes(dlg.FileName);
			Array.Copy(file.GetData(), 0, fileByteRead, 0x100 + fileIndex * 0x280, SaveFile.SIZE);

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
					fileSelectPnl.Enabled = true;

					labelLogs.Text = Path.GetFileName(dlg.FileName).ToString();

					using (FileStream fs = new FileStream(dlg.FileName, FileMode.Open, FileAccess.Read))
					{
						files = new SaveFile[3];
						for (int i = 0; i < files.Length; i++)
						{
							files[i] = SaveFile.FromSav(fs, i);
							if (!files[i].ChecksumWasValid)
								MessageBox.Show("The checksum for file " + (i + 1).ToString() + " was invalid.");
						}
					}
				}
				else
				{
					fileSelectPnl.Enabled = false;
					fileDataPnl.Enabled = false;
					MessageBox.Show("File does not exist.");
				}
			}
		}

		private void radioButton_CheckedChanged(object sender, EventArgs e)
		{
			saveBtn.Enabled = true;
			fileDataPnl.Enabled = true;

			RefreshFileIndex();
			SaveFile file = files[fileIndex];

			BSBNumUpDown.Value = file.OverworldBackground;
			BSBPictureBox.Image = BGs[file.OverworldBackground];

			livesNumUpDown.Value = file.Lives;
			coinsNumUpDown.Value = file.Coins;
			SCNumUpDown.Value = file.StarCoins;
			scoreNumUpDown.Value = file.Score;

			ReadPowerups();
			ReadInventory();
		}
	}
}
