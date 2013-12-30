using System.Drawing;
using System.Drawing.Text;
using System.IO;

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
            // If the user's decided not to fuck with the files we've included,
            // we'll load Open Sans Bold as the font to use.
            if (File.Exists("Resources/OpenSans-Semibold.ttf"))
            {
                // This is a stupid as shit method of adding fonts. Why is it either
                // from a file, or via an IntPtr to memory? Why can't I load it from
                // a resource file? Regardless...
                PrivateFontCollection pfc = new PrivateFontCollection();
                pfc.AddFontFile("Resources/OpenSans-Semibold.ttf");

                this.lbl_CardText.Font = new System.Drawing.Font(pfc.Families[0], 9F, FontStyle.Bold);
            }
            else
            {
                // If we can't find the file, probably because the user's fucked with it,
                // we'll load MS Sans Serif instead, since the system is guaranteed to
                // have that.
                this.lbl_CardText.Font = new System.Drawing.Font(
                    "Microsoft Sans Serif", 9F,
                    System.Drawing.FontStyle.Regular,
                    System.Drawing.GraphicsUnit.Point,
                    ((byte)(0))
                );
            }
            this.lbl_CardText.Location = new System.Drawing.Point(2, 2);
            this.lbl_CardText.MaximumSize = new System.Drawing.Size(116, 116);
            this.lbl_CardText.Name = "lbl_CardText";
            this.lbl_CardText.Size = new System.Drawing.Size(112, 32);
            this.lbl_CardText.TabIndex = 2;
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
