using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

using Newtonsoft.Json.Linq;

namespace NewSuperMarioBrosSaveEditor
{
	public partial class Form1 : Form
	{
		private static string WindowTitle = "NSMB Save Editor";
		private static string FileFilter = "All supported types|*.sav;*.SaveRAM;*.dsv|" +
			"Raw savefile (*.sav)|*.sav|" +
			"BizHawk save (*.SaveRAM)|*.SaveRAM|" +
			"DeSmuME save (*.dsv)|*.dsv";

		public Form1()
		{
			InitializeComponent();
			overworldViewer1.LocksChanged += () => fileModified(overworldViewer1, null);
			nodeClickCbx.SelectedIndex = 2;
		}

		private SaveFile[] files = null;
		private int fileIndex = 0;

		private string savFileName = null;

		Bitmap[] BGs = {
			Properties.Resources.NSMB_BG1,
			Properties.Resources.NSMB_BG2,
			Properties.Resources.NSMB_BG3,
			Properties.Resources.NSMB_BG4,
			Properties.Resources.NSMB_BG5,
		};


		private void saveBtn_Clicked(object sender, EventArgs e)
		{
			SaveFile file = files[fileIndex];

			// Quick 'unlock all' levels/worlds
			if (unlockLCheckBox.Checked)
			{
				for (int i = 0; i < 0xE4; i++)
					file.SetPathFlags(i, 0xC0);
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
			byte[] fileByteRead = File.ReadAllBytes(savFileName);
			Array.Copy(file.GetData(), 0, fileByteRead, 0x100 + fileIndex * 0x280, SaveFile.SIZE);

			using (FileStream fsWrite = new FileStream(savFileName, FileMode.Open, FileAccess.Write))
				fsWrite.Write(fileByteRead, 0, fileByteRead.Length);

			// Remove asterisk from title
			this.Text = WindowTitle + " - " + Path.GetFileName(savFileName);
		}
		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog dlg = new SaveFileDialog
			{
				Filter = FileFilter
			};

			if (dlg.ShowDialog() == DialogResult.OK)
			{
				savFileName = dlg.FileName;
				this.Text = WindowTitle + " - " + Path.GetFileName(savFileName);
				saveBtn_Clicked(sender, e);
			}
		}
		private void openBtn_Clicked(object sender, EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog
			{
				Filter = FileFilter
			};

			if (dlg.ShowDialog() == DialogResult.OK)
			{
				if (File.Exists(dlg.FileName))
				{
					openingFile = true;

					fileSelectPnl.Enabled = true;
					saveToolStripMenuItem.Enabled = true;
					
					saveAsToolStripMenuItem.Enabled = true;

					savFileName = dlg.FileName;
					this.Text = WindowTitle + " - " + Path.GetFileName(savFileName);

					using (FileStream fs = new FileStream(savFileName, FileMode.Open, FileAccess.Read))
					{
						files = new SaveFile[3];
						for (int i = 0; i < files.Length; i++)
						{
							files[i] = SaveFile.FromSav(fs, i);
							if (!files[i].ChecksumWasValid)
								MessageBox.Show("The checksum for file " + (i + 1).ToString() + " was invalid.");
						}
					}

					radioButton_CheckedChanged(null, null);
					worldNum_ValueChanged(null, null);

					openingFile = false;
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
			fileDataPnl.Enabled = true;

			if      (radioButton1.Checked) { fileIndex = 0; }
			else if (radioButton2.Checked) { fileIndex = 1; }
			else if (radioButton3.Checked) { fileIndex = 2; }
			SaveFile file = files[fileIndex];

			BSBNumUpDown.Value = file.OverworldBackground;
			BSBPictureBox.Image = BGs[file.OverworldBackground];

			livesNumUpDown.Value = file.Lives;
			coinsNumUpDown.Value = file.Coins;
			SCNumUpDown.Value = file.StarCoins;
			scoreNumUpDown.Value = file.Score;

			powerupCbx.SelectedIndex = files[fileIndex].CurrentPowerup;
			inventoryCbx.SelectedIndex = files[fileIndex].Inventory;

			overworldViewer1.ApplySave(files[fileIndex]);
		}

		private void worldNum_ValueChanged(object sender, EventArgs e)
		{
			JToken jArray = JToken.Parse(File.ReadAllText("data.json"));
			overworldViewer1.LoadOverworld((JObject)jArray[(int)worldNum.Value - 1]);
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show("New Super Mario Bros. Save Editor\n" +
				"\n" +
				"Credits:\n" +
				"Dev: newluigidev, RedStoneMatt, Suuper\n" +
				"Special thanks: RoadRunnerWMC, shibboleet, RicBent\n" +
				"\n" +
				"See README file for more details.\n" + 
				"\n" +
				"Version: 0.1 (beta)"
			);
		}

		private bool openingFile = false;
		private void fileModified(object sender, EventArgs e)
		{
			if (!openingFile)
				this.Text = WindowTitle + " - *" + Path.GetFileName(savFileName);
		}

		private void nodeClickCbx_SelectedIndexChanged(object sender, EventArgs e) =>
			overworldViewer1.NodeClickAction = (OverworldViewer.NodeAction)nodeClickCbx.SelectedIndex;
		private void doubleClickNodeCbx_CheckedChanged(object sender, EventArgs e) =>
			overworldViewer1.NodeActionOnDoubleClickOnly = doubleClickNodeCbx.Checked;

	}
}
