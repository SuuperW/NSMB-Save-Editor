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
			this.SuspendLayout();
			// 
			// paddingLbl
			// 
			this.paddingLbl.AutoSize = true;
			this.paddingLbl.Location = new System.Drawing.Point(3, 0);
			this.paddingLbl.Name = "paddingLbl";
			this.paddingLbl.Size = new System.Drawing.Size(0, 13);
			this.paddingLbl.TabIndex = 0;
			// 
			// OverworldViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.Controls.Add(this.paddingLbl);
			this.Name = "OverworldViewer";
			this.Size = new System.Drawing.Size(380, 150);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label paddingLbl;
	}
}
