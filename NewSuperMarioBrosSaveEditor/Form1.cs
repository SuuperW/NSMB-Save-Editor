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
			overworldViewer1.LocksChanged += UpdateControlsBySaveFile;
			JArray jArray = JArray.Parse(File.ReadAllText("data.json"));
			overworldViewer1.SetWorldCollection((WorldCollection)jArray);

			nodeClickCbx.SelectedIndex = 2;
		}

		private SaveFile[] files = null;
		private int fileIndex = -1;

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
			UpdateSaveFileByControls();			

			// Copy data into the file
			using (FileStream fs = new FileStream(savFileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
			{
				byte[] fileBytes = new byte[fs.Length];
				fs.Read(fileBytes, 0, fileBytes.Length);
				for (int i = 0; i < files.Length; i++)
					Array.Copy(files[i].GetData(), 0, fileBytes, 0x100 + i * 0x280, SaveFile.SIZE);
				fs.Seek(0, SeekOrigin.Begin);
				fs.Write(fileBytes, 0, fileBytes.Length);
			}

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

					// DeSmuME doesn't like to share. (Specifying FileShare.ReadWrite allows us to open the file.)
					using (FileStream fs = new FileStream(savFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
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
					worldNum_ValueChanged(null, null); // will update the viewer's display

					openingFile = false;
				}
				else
				{
					fileSelectPnl.Enabled = false;
					fileDataPnl.Enabled = false;
					this.Text = WindowTitle;
					MessageBox.Show("File does not exist.");
				}
			}
		}

		private void radioButton_CheckedChanged(object sender, EventArgs e)
		{
			// Keep any changes to previously-selected file
			if (fileIndex != -1 && sender != null && !(sender as RadioButton).Checked)
				UpdateSaveFileByControls();

			// Update selected file
			if      (radioButton1.Checked) { fileIndex = 0; }
			else if (radioButton2.Checked) { fileIndex = 1; }
			else if (radioButton3.Checked) { fileIndex = 2; }

			// Display this file's data
			if (sender == null || (sender as RadioButton).Checked)
			{
				openingFile = true;
				UpdateControlsBySaveFile();
				openingFile = false;
			}
		}
		private void UpdateSaveFileByControls()
		{
			SaveFile file = files[fileIndex];

			file.Lives = (int)livesNumUpDown.Value;
			file.Coins = (int)coinsNumUpDown.Value;
			file.Score = (int)scoreNumUpDown.Value;
			file.CurrentPowerup = powerupCbx.SelectedIndex > 2 ? powerupCbx.SelectedIndex + 1 : powerupCbx.SelectedIndex;
			file.Inventory = inventoryCbx.SelectedIndex;

			file.OverworldBackground = (byte)BSBNumUpDown.Value;
			uint nextFlag = (uint)SaveFile.BackgroundPurchases.First;
			uint purchased = 0;
			for (int i = 0; i < 4; i++)
			{
				if (backgroundsChk.GetItemChecked(i))
					purchased |= nextFlag;
				nextFlag <<= 1;
			}
			file.BackgroundsBought = (SaveFile.BackgroundPurchases)purchased;

		}
		private void UpdateControlsBySaveFile()
		{
			SaveFile file = files[fileIndex];

			BSBNumUpDown.Value = file.OverworldBackground;
			BSBPictureBox.Image = BGs[file.OverworldBackground];
			uint nextFlag = (uint)SaveFile.BackgroundPurchases.First;
			for (int i = 0; i < 4; i++)
			{
				backgroundsChk.SetItemChecked(i, ((uint)files[fileIndex].BackgroundsBought & nextFlag) != 0);
				nextFlag <<= 1;
			}

			livesNumUpDown.Value = file.Lives;
			coinsNumUpDown.Value = file.Coins;
			scoreNumUpDown.Value = file.Score;

			powerupCbx.SelectedIndex = files[fileIndex].CurrentPowerup;
			inventoryCbx.SelectedIndex = files[fileIndex].Inventory;

			overworldViewer1.ApplySave(files[fileIndex]);

			starCoinCountsLbl.Text = files[fileIndex].StarCoins.ToString() + " (" + files[fileIndex].SpentStarCoins + ")";

			newFileChk.Checked = file.IsNewFile;
			fileDataPnl.Enabled = !newFileChk.Checked;
		}

		private void worldNum_ValueChanged(object sender, EventArgs e)
		{
			overworldViewer1.LoadOverworld((int)worldNum.Value - 1);
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

		private void newFileChk_CheckedChanged(object sender, EventArgs e)
		{
			files[fileIndex].IsNewFile = newFileChk.Checked;
			fileDataPnl.Enabled = !newFileChk.Checked;
			fileModified(sender, e);
		}

		private void backgroundsChk_ItemCheck(object sender, ItemCheckEventArgs e)
		{
		}
	}
}
