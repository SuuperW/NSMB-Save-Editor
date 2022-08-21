namespace NewSuperMarioBrosSaveEditor
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.labelBSB = new System.Windows.Forms.Label();
			this.labelInventory = new System.Windows.Forms.Label();
			this.labelPowerup = new System.Windows.Forms.Label();
			this.labelScore = new System.Windows.Forms.Label();
			this.labelSC = new System.Windows.Forms.Label();
			this.labelCoins = new System.Windows.Forms.Label();
			this.labelLives = new System.Windows.Forms.Label();
			this.saveBtn = new System.Windows.Forms.Button();
			this.openBtn = new System.Windows.Forms.Button();
			this.inventoryCbx = new System.Windows.Forms.ComboBox();
			this.BSBPictureBox = new System.Windows.Forms.PictureBox();
			this.BSBNumUpDown = new System.Windows.Forms.NumericUpDown();
			this.scoreNumUpDown = new System.Windows.Forms.NumericUpDown();
			this.powerupCbx = new System.Windows.Forms.ComboBox();
			this.SCNumUpDown = new System.Windows.Forms.NumericUpDown();
			this.coinsNumUpDown = new System.Windows.Forms.NumericUpDown();
			this.livesNumUpDown = new System.Windows.Forms.NumericUpDown();
			this.radioButton1 = new System.Windows.Forms.RadioButton();
			this.radioButton2 = new System.Windows.Forms.RadioButton();
			this.radioButton3 = new System.Windows.Forms.RadioButton();
			this.labelLogs = new System.Windows.Forms.Label();
			this.labelCredits = new System.Windows.Forms.Label();
			this.unlockLCheckBox = new System.Windows.Forms.CheckBox();
			this.unlockWCheckBox = new System.Windows.Forms.CheckBox();
			this.fileSelectPnl = new System.Windows.Forms.Panel();
			this.fileDataPnl = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.BSBPictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.BSBNumUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.scoreNumUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.SCNumUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.coinsNumUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.livesNumUpDown)).BeginInit();
			this.fileSelectPnl.SuspendLayout();
			this.fileDataPnl.SuspendLayout();
			this.SuspendLayout();
			// 
			// labelBSB
			// 
			this.labelBSB.AutoSize = true;
			this.labelBSB.Location = new System.Drawing.Point(22, 192);
			this.labelBSB.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelBSB.Name = "labelBSB";
			this.labelBSB.Size = new System.Drawing.Size(141, 13);
			this.labelBSB.TabIndex = 45;
			this.labelBSB.Text = "Bottom Screen Background:";
			// 
			// labelInventory
			// 
			this.labelInventory.AutoSize = true;
			this.labelInventory.Location = new System.Drawing.Point(22, 158);
			this.labelInventory.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelInventory.Name = "labelInventory";
			this.labelInventory.Size = new System.Drawing.Size(77, 13);
			this.labelInventory.TabIndex = 44;
			this.labelInventory.Text = "Inventory Item:";
			// 
			// labelPowerup
			// 
			this.labelPowerup.AutoSize = true;
			this.labelPowerup.Location = new System.Drawing.Point(22, 135);
			this.labelPowerup.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelPowerup.Name = "labelPowerup";
			this.labelPowerup.Size = new System.Drawing.Size(52, 13);
			this.labelPowerup.TabIndex = 43;
			this.labelPowerup.Text = "Powerup:";
			// 
			// labelScore
			// 
			this.labelScore.AutoSize = true;
			this.labelScore.Location = new System.Drawing.Point(22, 64);
			this.labelScore.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelScore.Name = "labelScore";
			this.labelScore.Size = new System.Drawing.Size(38, 13);
			this.labelScore.TabIndex = 42;
			this.labelScore.Text = "Score:";
			// 
			// labelSC
			// 
			this.labelSC.AutoSize = true;
			this.labelSC.Location = new System.Drawing.Point(22, 45);
			this.labelSC.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelSC.Name = "labelSC";
			this.labelSC.Size = new System.Drawing.Size(58, 13);
			this.labelSC.TabIndex = 40;
			this.labelSC.Text = "Star Coins:";
			// 
			// labelCoins
			// 
			this.labelCoins.AutoSize = true;
			this.labelCoins.Location = new System.Drawing.Point(22, 26);
			this.labelCoins.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelCoins.Name = "labelCoins";
			this.labelCoins.Size = new System.Drawing.Size(36, 13);
			this.labelCoins.TabIndex = 39;
			this.labelCoins.Text = "Coins:";
			// 
			// labelLives
			// 
			this.labelLives.AutoSize = true;
			this.labelLives.Location = new System.Drawing.Point(22, 6);
			this.labelLives.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelLives.Name = "labelLives";
			this.labelLives.Size = new System.Drawing.Size(35, 13);
			this.labelLives.TabIndex = 38;
			this.labelLives.Text = "Lives:";
			// 
			// saveBtn
			// 
			this.saveBtn.Enabled = false;
			this.saveBtn.Location = new System.Drawing.Point(122, 374);
			this.saveBtn.Margin = new System.Windows.Forms.Padding(2);
			this.saveBtn.Name = "saveBtn";
			this.saveBtn.Size = new System.Drawing.Size(72, 20);
			this.saveBtn.TabIndex = 37;
			this.saveBtn.Text = "Save";
			this.saveBtn.UseVisualStyleBackColor = true;
			this.saveBtn.Click += new System.EventHandler(this.saveBtn_Clicked);
			// 
			// openBtn
			// 
			this.openBtn.Location = new System.Drawing.Point(47, 374);
			this.openBtn.Margin = new System.Windows.Forms.Padding(2);
			this.openBtn.Name = "openBtn";
			this.openBtn.Size = new System.Drawing.Size(72, 20);
			this.openBtn.TabIndex = 36;
			this.openBtn.Text = "Browse...";
			this.openBtn.UseVisualStyleBackColor = true;
			this.openBtn.Click += new System.EventHandler(this.openBtn_Clicked);
			// 
			// inventoryCbx
			// 
			this.inventoryCbx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.inventoryCbx.FormattingEnabled = true;
			this.inventoryCbx.Items.AddRange(new object[] {
            "Empty",
            "Mushroom",
            "Fire Flower",
            "Blue Shell",
            "Mini Mushroom",
            "Mega Mushroom"});
			this.inventoryCbx.Location = new System.Drawing.Point(128, 156);
			this.inventoryCbx.Margin = new System.Windows.Forms.Padding(2);
			this.inventoryCbx.Name = "inventoryCbx";
			this.inventoryCbx.Size = new System.Drawing.Size(102, 21);
			this.inventoryCbx.TabIndex = 35;
			// 
			// BSBPictureBox
			// 
			this.BSBPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.BSBPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.BSBPictureBox.Location = new System.Drawing.Point(56, 223);
			this.BSBPictureBox.Margin = new System.Windows.Forms.Padding(2);
			this.BSBPictureBox.Name = "BSBPictureBox";
			this.BSBPictureBox.Size = new System.Drawing.Size(130, 101);
			this.BSBPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.BSBPictureBox.TabIndex = 34;
			this.BSBPictureBox.TabStop = false;
			// 
			// BSBNumUpDown
			// 
			this.BSBNumUpDown.Location = new System.Drawing.Point(166, 191);
			this.BSBNumUpDown.Margin = new System.Windows.Forms.Padding(2);
			this.BSBNumUpDown.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.BSBNumUpDown.Name = "BSBNumUpDown";
			this.BSBNumUpDown.Size = new System.Drawing.Size(63, 20);
			this.BSBNumUpDown.TabIndex = 33;
			this.BSBNumUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// scoreNumUpDown
			// 
			this.scoreNumUpDown.Location = new System.Drawing.Point(128, 63);
			this.scoreNumUpDown.Margin = new System.Windows.Forms.Padding(2);
			this.scoreNumUpDown.Maximum = new decimal(new int[] {
            9999950,
            0,
            0,
            0});
			this.scoreNumUpDown.Name = "scoreNumUpDown";
			this.scoreNumUpDown.Size = new System.Drawing.Size(100, 20);
			this.scoreNumUpDown.TabIndex = 32;
			this.scoreNumUpDown.ThousandsSeparator = true;
			// 
			// powerupCbx
			// 
			this.powerupCbx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.powerupCbx.FormattingEnabled = true;
			this.powerupCbx.Items.AddRange(new object[] {
            "Small",
            "Big",
            "Fire",
            "Mini",
            "Blue Shell"});
			this.powerupCbx.Location = new System.Drawing.Point(128, 134);
			this.powerupCbx.Margin = new System.Windows.Forms.Padding(2);
			this.powerupCbx.Name = "powerupCbx";
			this.powerupCbx.Size = new System.Drawing.Size(102, 21);
			this.powerupCbx.TabIndex = 31;
			// 
			// SCNumUpDown
			// 
			this.SCNumUpDown.Location = new System.Drawing.Point(128, 44);
			this.SCNumUpDown.Margin = new System.Windows.Forms.Padding(2);
			this.SCNumUpDown.Maximum = new decimal(new int[] {
            240,
            0,
            0,
            0});
			this.SCNumUpDown.Name = "SCNumUpDown";
			this.SCNumUpDown.Size = new System.Drawing.Size(100, 20);
			this.SCNumUpDown.TabIndex = 29;
			// 
			// coinsNumUpDown
			// 
			this.coinsNumUpDown.Location = new System.Drawing.Point(128, 25);
			this.coinsNumUpDown.Margin = new System.Windows.Forms.Padding(2);
			this.coinsNumUpDown.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
			this.coinsNumUpDown.Name = "coinsNumUpDown";
			this.coinsNumUpDown.Size = new System.Drawing.Size(100, 20);
			this.coinsNumUpDown.TabIndex = 28;
			// 
			// livesNumUpDown
			// 
			this.livesNumUpDown.Location = new System.Drawing.Point(128, 6);
			this.livesNumUpDown.Margin = new System.Windows.Forms.Padding(2);
			this.livesNumUpDown.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
			this.livesNumUpDown.Name = "livesNumUpDown";
			this.livesNumUpDown.Size = new System.Drawing.Size(100, 20);
			this.livesNumUpDown.TabIndex = 27;
			// 
			// radioButton1
			// 
			this.radioButton1.AutoSize = true;
			this.radioButton1.Location = new System.Drawing.Point(47, 12);
			this.radioButton1.Margin = new System.Windows.Forms.Padding(2);
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.Size = new System.Drawing.Size(50, 17);
			this.radioButton1.TabIndex = 46;
			this.radioButton1.TabStop = true;
			this.radioButton1.Text = "File 1";
			this.radioButton1.UseVisualStyleBackColor = true;
			this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
			// 
			// radioButton2
			// 
			this.radioButton2.AutoSize = true;
			this.radioButton2.Location = new System.Drawing.Point(98, 12);
			this.radioButton2.Margin = new System.Windows.Forms.Padding(2);
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.Size = new System.Drawing.Size(50, 17);
			this.radioButton2.TabIndex = 47;
			this.radioButton2.TabStop = true;
			this.radioButton2.Text = "File 2";
			this.radioButton2.UseVisualStyleBackColor = true;
			this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
			// 
			// radioButton3
			// 
			this.radioButton3.AutoSize = true;
			this.radioButton3.Location = new System.Drawing.Point(149, 12);
			this.radioButton3.Margin = new System.Windows.Forms.Padding(2);
			this.radioButton3.Name = "radioButton3";
			this.radioButton3.Size = new System.Drawing.Size(50, 17);
			this.radioButton3.TabIndex = 48;
			this.radioButton3.TabStop = true;
			this.radioButton3.Text = "File 3";
			this.radioButton3.UseVisualStyleBackColor = true;
			this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
			// 
			// labelLogs
			// 
			this.labelLogs.AutoSize = true;
			this.labelLogs.Location = new System.Drawing.Point(6, 411);
			this.labelLogs.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelLogs.Name = "labelLogs";
			this.labelLogs.Size = new System.Drawing.Size(73, 13);
			this.labelLogs.TabIndex = 49;
			this.labelLogs.Text = "No file open...";
			// 
			// labelCredits
			// 
			this.labelCredits.AutoSize = true;
			this.labelCredits.Location = new System.Drawing.Point(141, 411);
			this.labelCredits.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelCredits.Name = "labelCredits";
			this.labelCredits.Size = new System.Drawing.Size(107, 13);
			this.labelCredits.TabIndex = 50;
			this.labelCredits.Text = "v1.01 by newluigidev";
			// 
			// unlockLCheckBox
			// 
			this.unlockLCheckBox.AutoSize = true;
			this.unlockLCheckBox.Location = new System.Drawing.Point(25, 111);
			this.unlockLCheckBox.Margin = new System.Windows.Forms.Padding(2);
			this.unlockLCheckBox.Name = "unlockLCheckBox";
			this.unlockLCheckBox.Size = new System.Drawing.Size(107, 17);
			this.unlockLCheckBox.TabIndex = 51;
			this.unlockLCheckBox.Text = "Unlock all Levels";
			this.unlockLCheckBox.UseVisualStyleBackColor = true;
			// 
			// unlockWCheckBox
			// 
			this.unlockWCheckBox.AutoSize = true;
			this.unlockWCheckBox.Location = new System.Drawing.Point(25, 88);
			this.unlockWCheckBox.Margin = new System.Windows.Forms.Padding(2);
			this.unlockWCheckBox.Name = "unlockWCheckBox";
			this.unlockWCheckBox.Size = new System.Drawing.Size(109, 17);
			this.unlockWCheckBox.TabIndex = 52;
			this.unlockWCheckBox.Text = "Unlock all Worlds";
			this.unlockWCheckBox.UseVisualStyleBackColor = true;
			// 
			// fileSelectPnl
			// 
			this.fileSelectPnl.Controls.Add(this.radioButton1);
			this.fileSelectPnl.Controls.Add(this.radioButton2);
			this.fileSelectPnl.Controls.Add(this.radioButton3);
			this.fileSelectPnl.Dock = System.Windows.Forms.DockStyle.Top;
			this.fileSelectPnl.Enabled = false;
			this.fileSelectPnl.Location = new System.Drawing.Point(0, 0);
			this.fileSelectPnl.Name = "fileSelectPnl";
			this.fileSelectPnl.Size = new System.Drawing.Size(251, 37);
			this.fileSelectPnl.TabIndex = 53;
			// 
			// fileDataPnl
			// 
			this.fileDataPnl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.fileDataPnl.Controls.Add(this.unlockWCheckBox);
			this.fileDataPnl.Controls.Add(this.labelLives);
			this.fileDataPnl.Controls.Add(this.unlockLCheckBox);
			this.fileDataPnl.Controls.Add(this.livesNumUpDown);
			this.fileDataPnl.Controls.Add(this.coinsNumUpDown);
			this.fileDataPnl.Controls.Add(this.SCNumUpDown);
			this.fileDataPnl.Controls.Add(this.labelBSB);
			this.fileDataPnl.Controls.Add(this.powerupCbx);
			this.fileDataPnl.Controls.Add(this.labelInventory);
			this.fileDataPnl.Controls.Add(this.scoreNumUpDown);
			this.fileDataPnl.Controls.Add(this.labelPowerup);
			this.fileDataPnl.Controls.Add(this.BSBNumUpDown);
			this.fileDataPnl.Controls.Add(this.labelScore);
			this.fileDataPnl.Controls.Add(this.BSBPictureBox);
			this.fileDataPnl.Controls.Add(this.labelSC);
			this.fileDataPnl.Controls.Add(this.inventoryCbx);
			this.fileDataPnl.Controls.Add(this.labelCoins);
			this.fileDataPnl.Enabled = false;
			this.fileDataPnl.Location = new System.Drawing.Point(0, 36);
			this.fileDataPnl.Name = "fileDataPnl";
			this.fileDataPnl.Size = new System.Drawing.Size(251, 333);
			this.fileDataPnl.TabIndex = 54;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(251, 428);
			this.Controls.Add(this.labelCredits);
			this.Controls.Add(this.labelLogs);
			this.Controls.Add(this.saveBtn);
			this.Controls.Add(this.openBtn);
			this.Controls.Add(this.fileSelectPnl);
			this.Controls.Add(this.fileDataPnl);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(2);
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.Text = "NSMBDS SE";
			((System.ComponentModel.ISupportInitialize)(this.BSBPictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.BSBNumUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.scoreNumUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.SCNumUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.coinsNumUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.livesNumUpDown)).EndInit();
			this.fileSelectPnl.ResumeLayout(false);
			this.fileSelectPnl.PerformLayout();
			this.fileDataPnl.ResumeLayout(false);
			this.fileDataPnl.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelBSB;
        private System.Windows.Forms.Label labelInventory;
        private System.Windows.Forms.Label labelPowerup;
        private System.Windows.Forms.Label labelScore;
        private System.Windows.Forms.Label labelSC;
        private System.Windows.Forms.Label labelCoins;
        private System.Windows.Forms.Label labelLives;
        private System.Windows.Forms.Button saveBtn;
        private System.Windows.Forms.Button openBtn;
        private System.Windows.Forms.ComboBox inventoryCbx;
        private System.Windows.Forms.PictureBox BSBPictureBox;
        private System.Windows.Forms.NumericUpDown BSBNumUpDown;
        private System.Windows.Forms.NumericUpDown scoreNumUpDown;
        private System.Windows.Forms.ComboBox powerupCbx;
        private System.Windows.Forms.NumericUpDown SCNumUpDown;
        private System.Windows.Forms.NumericUpDown coinsNumUpDown;
        private System.Windows.Forms.NumericUpDown livesNumUpDown;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.Label labelLogs;
        private System.Windows.Forms.Label labelCredits;
        private System.Windows.Forms.CheckBox unlockLCheckBox;
        private System.Windows.Forms.CheckBox unlockWCheckBox;
		private System.Windows.Forms.Panel fileSelectPnl;
		private System.Windows.Forms.Panel fileDataPnl;
	}
}

