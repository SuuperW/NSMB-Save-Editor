﻿namespace NewSuperMarioBrosSaveEditor
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
			this.inventoryCbx = new System.Windows.Forms.ComboBox();
			this.BSBNumUpDown = new System.Windows.Forms.NumericUpDown();
			this.scoreNumUpDown = new System.Windows.Forms.NumericUpDown();
			this.powerupCbx = new System.Windows.Forms.ComboBox();
			this.coinsNumUpDown = new System.Windows.Forms.NumericUpDown();
			this.livesNumUpDown = new System.Windows.Forms.NumericUpDown();
			this.radioButton1 = new System.Windows.Forms.RadioButton();
			this.radioButton2 = new System.Windows.Forms.RadioButton();
			this.radioButton3 = new System.Windows.Forms.RadioButton();
			this.fileSelectPnl = new System.Windows.Forms.Panel();
			this.newFileChk = new System.Windows.Forms.CheckBox();
			this.fileDataPnl = new System.Windows.Forms.Panel();
			this.unlockEverythingBtn = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.backgroundsChk = new System.Windows.Forms.CheckedListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.worldNum = new System.Windows.Forms.NumericUpDown();
			this.BSBPictureBox = new System.Windows.Forms.PictureBox();
			this.starCoinCountsLbl = new System.Windows.Forms.Label();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.label2 = new System.Windows.Forms.Label();
			this.nodeClickCbx = new System.Windows.Forms.ComboBox();
			this.doubleClickNodeCbx = new System.Windows.Forms.CheckBox();
			this.overworldViewer1 = new NewSuperMarioBrosSaveEditor.OverworldViewer();
			((System.ComponentModel.ISupportInitialize)(this.BSBNumUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.scoreNumUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.coinsNumUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.livesNumUpDown)).BeginInit();
			this.fileSelectPnl.SuspendLayout();
			this.fileDataPnl.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.worldNum)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.BSBPictureBox)).BeginInit();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// labelBSB
			// 
			this.labelBSB.AutoSize = true;
			this.labelBSB.Location = new System.Drawing.Point(156, 57);
			this.labelBSB.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelBSB.Name = "labelBSB";
			this.labelBSB.Size = new System.Drawing.Size(141, 13);
			this.labelBSB.TabIndex = 45;
			this.labelBSB.Text = "Bottom Screen Background:";
			// 
			// labelInventory
			// 
			this.labelInventory.AutoSize = true;
			this.labelInventory.Location = new System.Drawing.Point(156, 33);
			this.labelInventory.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelInventory.Name = "labelInventory";
			this.labelInventory.Size = new System.Drawing.Size(77, 13);
			this.labelInventory.TabIndex = 44;
			this.labelInventory.Text = "Inventory Item:";
			// 
			// labelPowerup
			// 
			this.labelPowerup.AutoSize = true;
			this.labelPowerup.Location = new System.Drawing.Point(156, 8);
			this.labelPowerup.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelPowerup.Name = "labelPowerup";
			this.labelPowerup.Size = new System.Drawing.Size(52, 13);
			this.labelPowerup.TabIndex = 43;
			this.labelPowerup.Text = "Powerup:";
			// 
			// labelScore
			// 
			this.labelScore.AutoSize = true;
			this.labelScore.Location = new System.Drawing.Point(11, 57);
			this.labelScore.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelScore.Name = "labelScore";
			this.labelScore.Size = new System.Drawing.Size(38, 13);
			this.labelScore.TabIndex = 42;
			this.labelScore.Text = "Score:";
			// 
			// labelSC
			// 
			this.labelSC.AutoSize = true;
			this.labelSC.Location = new System.Drawing.Point(11, 81);
			this.labelSC.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelSC.Name = "labelSC";
			this.labelSC.Size = new System.Drawing.Size(138, 13);
			this.labelSC.TabIndex = 40;
			this.labelSC.Text = "Star coins collected (spent):";
			// 
			// labelCoins
			// 
			this.labelCoins.AutoSize = true;
			this.labelCoins.Location = new System.Drawing.Point(11, 33);
			this.labelCoins.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelCoins.Name = "labelCoins";
			this.labelCoins.Size = new System.Drawing.Size(36, 13);
			this.labelCoins.TabIndex = 39;
			this.labelCoins.Text = "Coins:";
			// 
			// labelLives
			// 
			this.labelLives.AutoSize = true;
			this.labelLives.Location = new System.Drawing.Point(11, 8);
			this.labelLives.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelLives.Name = "labelLives";
			this.labelLives.Size = new System.Drawing.Size(35, 13);
			this.labelLives.TabIndex = 38;
			this.labelLives.Text = "Lives:";
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
			this.inventoryCbx.Location = new System.Drawing.Point(237, 29);
			this.inventoryCbx.Margin = new System.Windows.Forms.Padding(2);
			this.inventoryCbx.Name = "inventoryCbx";
			this.inventoryCbx.Size = new System.Drawing.Size(102, 21);
			this.inventoryCbx.TabIndex = 35;
			this.inventoryCbx.SelectedIndexChanged += new System.EventHandler(this.fileModified);
			// 
			// BSBNumUpDown
			// 
			this.BSBNumUpDown.Location = new System.Drawing.Point(300, 56);
			this.BSBNumUpDown.Margin = new System.Windows.Forms.Padding(2);
			this.BSBNumUpDown.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
			this.BSBNumUpDown.Name = "BSBNumUpDown";
			this.BSBNumUpDown.Size = new System.Drawing.Size(39, 20);
			this.BSBNumUpDown.TabIndex = 33;
			this.BSBNumUpDown.ValueChanged += new System.EventHandler(this.BSBNumUpDown_ValueChanged);
			// 
			// scoreNumUpDown
			// 
			this.scoreNumUpDown.Location = new System.Drawing.Point(73, 54);
			this.scoreNumUpDown.Margin = new System.Windows.Forms.Padding(2);
			this.scoreNumUpDown.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
			this.scoreNumUpDown.Name = "scoreNumUpDown";
			this.scoreNumUpDown.Size = new System.Drawing.Size(79, 20);
			this.scoreNumUpDown.TabIndex = 32;
			this.scoreNumUpDown.ThousandsSeparator = true;
			this.scoreNumUpDown.ValueChanged += new System.EventHandler(this.fileModified);
			// 
			// powerupCbx
			// 
			this.powerupCbx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.powerupCbx.FormattingEnabled = true;
			this.powerupCbx.Items.AddRange(new object[] {
            "Small",
            "Big",
            "Fire",
            "Mega",
            "Mini",
            "Blue Shell"});
			this.powerupCbx.Location = new System.Drawing.Point(237, 5);
			this.powerupCbx.Margin = new System.Windows.Forms.Padding(2);
			this.powerupCbx.Name = "powerupCbx";
			this.powerupCbx.Size = new System.Drawing.Size(102, 21);
			this.powerupCbx.TabIndex = 31;
			this.powerupCbx.SelectedIndexChanged += new System.EventHandler(this.fileModified);
			// 
			// coinsNumUpDown
			// 
			this.coinsNumUpDown.Location = new System.Drawing.Point(73, 30);
			this.coinsNumUpDown.Margin = new System.Windows.Forms.Padding(2);
			this.coinsNumUpDown.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
			this.coinsNumUpDown.Name = "coinsNumUpDown";
			this.coinsNumUpDown.Size = new System.Drawing.Size(79, 20);
			this.coinsNumUpDown.TabIndex = 28;
			this.coinsNumUpDown.ValueChanged += new System.EventHandler(this.fileModified);
			// 
			// livesNumUpDown
			// 
			this.livesNumUpDown.Location = new System.Drawing.Point(73, 6);
			this.livesNumUpDown.Margin = new System.Windows.Forms.Padding(2);
			this.livesNumUpDown.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
			this.livesNumUpDown.Name = "livesNumUpDown";
			this.livesNumUpDown.Size = new System.Drawing.Size(79, 20);
			this.livesNumUpDown.TabIndex = 27;
			this.livesNumUpDown.ValueChanged += new System.EventHandler(this.fileModified);
			// 
			// radioButton1
			// 
			this.radioButton1.AutoSize = true;
			this.radioButton1.Checked = true;
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
			this.radioButton3.Text = "File 3";
			this.radioButton3.UseVisualStyleBackColor = true;
			this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
			// 
			// fileSelectPnl
			// 
			this.fileSelectPnl.Controls.Add(this.newFileChk);
			this.fileSelectPnl.Controls.Add(this.radioButton1);
			this.fileSelectPnl.Controls.Add(this.radioButton2);
			this.fileSelectPnl.Controls.Add(this.radioButton3);
			this.fileSelectPnl.Dock = System.Windows.Forms.DockStyle.Top;
			this.fileSelectPnl.Enabled = false;
			this.fileSelectPnl.Location = new System.Drawing.Point(0, 24);
			this.fileSelectPnl.Name = "fileSelectPnl";
			this.fileSelectPnl.Size = new System.Drawing.Size(482, 37);
			this.fileSelectPnl.TabIndex = 53;
			// 
			// newFileChk
			// 
			this.newFileChk.AutoSize = true;
			this.newFileChk.Location = new System.Drawing.Point(217, 13);
			this.newFileChk.Name = "newFileChk";
			this.newFileChk.Size = new System.Drawing.Size(64, 17);
			this.newFileChk.TabIndex = 55;
			this.newFileChk.Text = "New file";
			this.newFileChk.UseVisualStyleBackColor = true;
			this.newFileChk.CheckedChanged += new System.EventHandler(this.newFileChk_CheckedChanged);
			// 
			// fileDataPnl
			// 
			this.fileDataPnl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.fileDataPnl.Controls.Add(this.unlockEverythingBtn);
			this.fileDataPnl.Controls.Add(this.label3);
			this.fileDataPnl.Controls.Add(this.backgroundsChk);
			this.fileDataPnl.Controls.Add(this.label1);
			this.fileDataPnl.Controls.Add(this.overworldViewer1);
			this.fileDataPnl.Controls.Add(this.labelLives);
			this.fileDataPnl.Controls.Add(this.livesNumUpDown);
			this.fileDataPnl.Controls.Add(this.coinsNumUpDown);
			this.fileDataPnl.Controls.Add(this.labelBSB);
			this.fileDataPnl.Controls.Add(this.powerupCbx);
			this.fileDataPnl.Controls.Add(this.labelInventory);
			this.fileDataPnl.Controls.Add(this.scoreNumUpDown);
			this.fileDataPnl.Controls.Add(this.labelPowerup);
			this.fileDataPnl.Controls.Add(this.worldNum);
			this.fileDataPnl.Controls.Add(this.BSBNumUpDown);
			this.fileDataPnl.Controls.Add(this.labelScore);
			this.fileDataPnl.Controls.Add(this.BSBPictureBox);
			this.fileDataPnl.Controls.Add(this.starCoinCountsLbl);
			this.fileDataPnl.Controls.Add(this.labelSC);
			this.fileDataPnl.Controls.Add(this.inventoryCbx);
			this.fileDataPnl.Controls.Add(this.labelCoins);
			this.fileDataPnl.Enabled = false;
			this.fileDataPnl.Location = new System.Drawing.Point(0, 60);
			this.fileDataPnl.Name = "fileDataPnl";
			this.fileDataPnl.Size = new System.Drawing.Size(482, 312);
			this.fileDataPnl.TabIndex = 54;
			// 
			// unlockEverythingBtn
			// 
			this.unlockEverythingBtn.Location = new System.Drawing.Point(98, 119);
			this.unlockEverythingBtn.Name = "unlockEverythingBtn";
			this.unlockEverythingBtn.Size = new System.Drawing.Size(110, 23);
			this.unlockEverythingBtn.TabIndex = 57;
			this.unlockEverythingBtn.Text = "Unlock Everything";
			this.unlockEverythingBtn.UseVisualStyleBackColor = true;
			this.unlockEverythingBtn.Click += new System.EventHandler(this.unlockEverythingBtn_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(307, 129);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(61, 13);
			this.label3.TabIndex = 56;
			this.label3.Text = "Purchased:";
			// 
			// backgroundsChk
			// 
			this.backgroundsChk.FormattingEnabled = true;
			this.backgroundsChk.Items.AddRange(new object[] {
            "Blue Bricks",
            "Stars",
            "Mario",
            "1-1"});
			this.backgroundsChk.Location = new System.Drawing.Point(372, 110);
			this.backgroundsChk.Name = "backgroundsChk";
			this.backgroundsChk.Size = new System.Drawing.Size(101, 34);
			this.backgroundsChk.TabIndex = 55;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(11, 124);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(38, 13);
			this.label1.TabIndex = 54;
			this.label1.Text = "World:";
			// 
			// worldNum
			// 
			this.worldNum.Location = new System.Drawing.Point(54, 122);
			this.worldNum.Margin = new System.Windows.Forms.Padding(2);
			this.worldNum.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
			this.worldNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.worldNum.Name = "worldNum";
			this.worldNum.Size = new System.Drawing.Size(39, 20);
			this.worldNum.TabIndex = 33;
			this.worldNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.worldNum.ValueChanged += new System.EventHandler(this.worldNum_ValueChanged);
			// 
			// BSBPictureBox
			// 
			this.BSBPictureBox.BackgroundImage = global::NewSuperMarioBrosSaveEditor.Properties.Resources.NSMB_BG1;
			this.BSBPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.BSBPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.BSBPictureBox.Location = new System.Drawing.Point(343, 5);
			this.BSBPictureBox.Margin = new System.Windows.Forms.Padding(2);
			this.BSBPictureBox.Name = "BSBPictureBox";
			this.BSBPictureBox.Size = new System.Drawing.Size(130, 101);
			this.BSBPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.BSBPictureBox.TabIndex = 34;
			this.BSBPictureBox.TabStop = false;
			// 
			// starCoinCountsLbl
			// 
			this.starCoinCountsLbl.AutoSize = true;
			this.starCoinCountsLbl.Location = new System.Drawing.Point(153, 81);
			this.starCoinCountsLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.starCoinCountsLbl.Name = "starCoinCountsLbl";
			this.starCoinCountsLbl.Size = new System.Drawing.Size(28, 13);
			this.starCoinCountsLbl.TabIndex = 40;
			this.starCoinCountsLbl.Text = "0 (0)";
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.aboutToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(482, 24);
			this.menuStrip1.TabIndex = 55;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.openToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
			this.openToolStripMenuItem.Text = "&Open...";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openBtn_Clicked);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Enabled = false;
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
			this.saveToolStripMenuItem.Text = "&Save";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveBtn_Clicked);
			// 
			// saveAsToolStripMenuItem
			// 
			this.saveAsToolStripMenuItem.Enabled = false;
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
			this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
			this.saveAsToolStripMenuItem.Text = "Save &as...";
			this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
			this.aboutToolStripMenuItem.Text = "&About";
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 379);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(70, 13);
			this.label2.TabIndex = 56;
			this.label2.Text = "Click level to:";
			// 
			// nodeClickCbx
			// 
			this.nodeClickCbx.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.nodeClickCbx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.nodeClickCbx.FormattingEnabled = true;
			this.nodeClickCbx.Items.AddRange(new object[] {
            "Clear normal goal",
            "Clear secret goal",
            "100% clear"});
			this.nodeClickCbx.Location = new System.Drawing.Point(87, 376);
			this.nodeClickCbx.Name = "nodeClickCbx";
			this.nodeClickCbx.Size = new System.Drawing.Size(108, 21);
			this.nodeClickCbx.TabIndex = 57;
			this.nodeClickCbx.SelectedIndexChanged += new System.EventHandler(this.nodeClickCbx_SelectedIndexChanged);
			// 
			// doubleClickNodeCbx
			// 
			this.doubleClickNodeCbx.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.doubleClickNodeCbx.AutoSize = true;
			this.doubleClickNodeCbx.Location = new System.Drawing.Point(201, 378);
			this.doubleClickNodeCbx.Name = "doubleClickNodeCbx";
			this.doubleClickNodeCbx.Size = new System.Drawing.Size(118, 17);
			this.doubleClickNodeCbx.TabIndex = 58;
			this.doubleClickNodeCbx.Text = "require double-click";
			this.doubleClickNodeCbx.UseVisualStyleBackColor = true;
			this.doubleClickNodeCbx.CheckedChanged += new System.EventHandler(this.doubleClickNodeCbx_CheckedChanged);
			// 
			// overworldViewer1
			// 
			this.overworldViewer1.AllWorlds = null;
			this.overworldViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.overworldViewer1.BackColor = System.Drawing.SystemColors.ControlDark;
			this.overworldViewer1.Location = new System.Drawing.Point(0, 147);
			this.overworldViewer1.Name = "overworldViewer1";
			this.overworldViewer1.SaveFile = null;
			this.overworldViewer1.Size = new System.Drawing.Size(482, 165);
			this.overworldViewer1.TabIndex = 53;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(482, 402);
			this.Controls.Add(this.doubleClickNodeCbx);
			this.Controls.Add(this.nodeClickCbx);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.fileSelectPnl);
			this.Controls.Add(this.fileDataPnl);
			this.Controls.Add(this.menuStrip1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.Margin = new System.Windows.Forms.Padding(2);
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.Text = "NSMB Save Editor";
			((System.ComponentModel.ISupportInitialize)(this.BSBNumUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.scoreNumUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.coinsNumUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.livesNumUpDown)).EndInit();
			this.fileSelectPnl.ResumeLayout(false);
			this.fileSelectPnl.PerformLayout();
			this.fileDataPnl.ResumeLayout(false);
			this.fileDataPnl.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.worldNum)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.BSBPictureBox)).EndInit();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
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
        private System.Windows.Forms.ComboBox inventoryCbx;
        private System.Windows.Forms.PictureBox BSBPictureBox;
        private System.Windows.Forms.NumericUpDown BSBNumUpDown;
        private System.Windows.Forms.NumericUpDown scoreNumUpDown;
        private System.Windows.Forms.ComboBox powerupCbx;
        private System.Windows.Forms.NumericUpDown coinsNumUpDown;
        private System.Windows.Forms.NumericUpDown livesNumUpDown;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
		private System.Windows.Forms.Panel fileSelectPnl;
		private System.Windows.Forms.Panel fileDataPnl;
		private System.Windows.Forms.Label label1;
		private OverworldViewer overworldViewer1;
		private System.Windows.Forms.NumericUpDown worldNum;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox nodeClickCbx;
		private System.Windows.Forms.CheckBox doubleClickNodeCbx;
		private System.Windows.Forms.CheckBox newFileChk;
		private System.Windows.Forms.Label starCoinCountsLbl;
		private System.Windows.Forms.CheckedListBox backgroundsChk;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button unlockEverythingBtn;
	}
}

