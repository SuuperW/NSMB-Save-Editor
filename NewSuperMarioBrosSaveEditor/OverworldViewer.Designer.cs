namespace NewSuperMarioBrosSaveEditor
{
	partial class OverworldViewer
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.paddingLbl = new System.Windows.Forms.Label();
			this.starCoin1Pbx = new System.Windows.Forms.PictureBox();
			this.starCoin2Pbx = new System.Windows.Forms.PictureBox();
			this.starCoin3Pbx = new System.Windows.Forms.PictureBox();
			this.mainPanel = new System.Windows.Forms.Panel();
			this.nodeNameLbl = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.starCoin1Pbx)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.starCoin2Pbx)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.starCoin3Pbx)).BeginInit();
			this.mainPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// paddingLbl
			// 
			this.paddingLbl.Location = new System.Drawing.Point(3, 0);
			this.paddingLbl.Name = "paddingLbl";
			this.paddingLbl.Size = new System.Drawing.Size(0, 0);
			this.paddingLbl.TabIndex = 0;
			// 
			// starCoin1Pbx
			// 
			this.starCoin1Pbx.BackgroundImage = global::NewSuperMarioBrosSaveEditor.Properties.Resources.StarCoin;
			this.starCoin1Pbx.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.starCoin1Pbx.Location = new System.Drawing.Point(3, 22);
			this.starCoin1Pbx.Margin = new System.Windows.Forms.Padding(2);
			this.starCoin1Pbx.Name = "starCoin1Pbx";
			this.starCoin1Pbx.Size = new System.Drawing.Size(16, 16);
			this.starCoin1Pbx.TabIndex = 1;
			this.starCoin1Pbx.TabStop = false;
			this.starCoin1Pbx.Tag = 0;
			this.starCoin1Pbx.Click += new System.EventHandler(this.starCoinPbx_Click);
			// 
			// starCoin2Pbx
			// 
			this.starCoin2Pbx.BackgroundImage = global::NewSuperMarioBrosSaveEditor.Properties.Resources.StarCoin;
			this.starCoin2Pbx.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.starCoin2Pbx.Location = new System.Drawing.Point(23, 22);
			this.starCoin2Pbx.Margin = new System.Windows.Forms.Padding(2);
			this.starCoin2Pbx.Name = "starCoin2Pbx";
			this.starCoin2Pbx.Size = new System.Drawing.Size(16, 16);
			this.starCoin2Pbx.TabIndex = 1;
			this.starCoin2Pbx.TabStop = false;
			this.starCoin2Pbx.Tag = 1;
			this.starCoin2Pbx.Click += new System.EventHandler(this.starCoinPbx_Click);
			// 
			// starCoin3Pbx
			// 
			this.starCoin3Pbx.BackgroundImage = global::NewSuperMarioBrosSaveEditor.Properties.Resources.StarCoin;
			this.starCoin3Pbx.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.starCoin3Pbx.Location = new System.Drawing.Point(43, 22);
			this.starCoin3Pbx.Margin = new System.Windows.Forms.Padding(2);
			this.starCoin3Pbx.Name = "starCoin3Pbx";
			this.starCoin3Pbx.Size = new System.Drawing.Size(16, 16);
			this.starCoin3Pbx.TabIndex = 1;
			this.starCoin3Pbx.TabStop = false;
			this.starCoin3Pbx.Tag = 2;
			this.starCoin3Pbx.Click += new System.EventHandler(this.starCoinPbx_Click);
			// 
			// mainPanel
			// 
			this.mainPanel.AutoScroll = true;
			this.mainPanel.Controls.Add(this.paddingLbl);
			this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainPanel.Location = new System.Drawing.Point(0, 0);
			this.mainPanel.Name = "mainPanel";
			this.mainPanel.Size = new System.Drawing.Size(380, 150);
			this.mainPanel.TabIndex = 2;
			// 
			// nodeNameLbl
			// 
			this.nodeNameLbl.AutoSize = true;
			this.nodeNameLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.nodeNameLbl.Location = new System.Drawing.Point(0, 0);
			this.nodeNameLbl.Name = "nodeNameLbl";
			this.nodeNameLbl.Size = new System.Drawing.Size(47, 20);
			this.nodeNameLbl.TabIndex = 0;
			this.nodeNameLbl.Text = "W1-1";
			// 
			// OverworldViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.starCoin3Pbx);
			this.Controls.Add(this.starCoin2Pbx);
			this.Controls.Add(this.starCoin1Pbx);
			this.Controls.Add(this.nodeNameLbl);
			this.Controls.Add(this.mainPanel);
			this.Name = "OverworldViewer";
			this.Size = new System.Drawing.Size(380, 150);
			((System.ComponentModel.ISupportInitialize)(this.starCoin1Pbx)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.starCoin2Pbx)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.starCoin3Pbx)).EndInit();
			this.mainPanel.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label paddingLbl;
		private System.Windows.Forms.PictureBox starCoin1Pbx;
		private System.Windows.Forms.PictureBox starCoin2Pbx;
		private System.Windows.Forms.PictureBox starCoin3Pbx;
		private System.Windows.Forms.Panel mainPanel;
		private System.Windows.Forms.Label nodeNameLbl;
	}
}
