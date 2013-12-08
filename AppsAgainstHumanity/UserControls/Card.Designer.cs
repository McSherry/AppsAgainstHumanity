namespace AppsAgainstHumanityClient
{
	partial class Card
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
			if (disposing && (components != null)) {
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
			this.lbl_CardText = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lbl_CardText
			// 
			this.lbl_CardText.AutoSize = true;
			this.lbl_CardText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbl_CardText.Location = new System.Drawing.Point(2, 2);
			this.lbl_CardText.MaximumSize = new System.Drawing.Size(116, 116);
			this.lbl_CardText.Name = "lbl_CardText";
			this.lbl_CardText.Size = new System.Drawing.Size(113, 40);
			this.lbl_CardText.TabIndex = 1;
			this.lbl_CardText.Text = "Card contents go here";
			// 
			// Card
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.Controls.Add(this.lbl_CardText);
			this.Name = "Card";
			this.Size = new System.Drawing.Size(120, 120);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lbl_CardText;

	}
}
