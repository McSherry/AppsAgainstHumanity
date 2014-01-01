namespace AppsAgainstHumanity.Server.UI
{
    partial class settingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(settingsForm));
            this.saveBtn = new System.Windows.Forms.Button();
            this.resetBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.bannerPicBox = new System.Windows.Forms.PictureBox();
            this.deckLocLbl = new System.Windows.Forms.Label();
            this.deckLocTBox = new System.Windows.Forms.TextBox();
            this.deckLocBtn = new System.Windows.Forms.Button();
            this.portNumLbl = new System.Windows.Forms.Label();
            this.portNumTBox = new System.Windows.Forms.TextBox();
            this.portNumExLbl = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.bannerPicBox)).BeginInit();
            this.SuspendLayout();
            // 
            // saveBtn
            // 
            this.saveBtn.Location = new System.Drawing.Point(12, 333);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(75, 23);
            this.saveBtn.TabIndex = 0;
            this.saveBtn.Text = "Save";
            this.saveBtn.UseVisualStyleBackColor = true;
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // resetBtn
            // 
            this.resetBtn.Location = new System.Drawing.Point(203, 333);
            this.resetBtn.Name = "resetBtn";
            this.resetBtn.Size = new System.Drawing.Size(75, 23);
            this.resetBtn.TabIndex = 1;
            this.resetBtn.Text = "Reset";
            this.resetBtn.UseVisualStyleBackColor = true;
            this.resetBtn.Click += new System.EventHandler(this.resetBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(284, 333);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 2;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            // 
            // bannerPicBox
            // 
            this.bannerPicBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bannerPicBox.BackgroundImage")));
            this.bannerPicBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.bannerPicBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.bannerPicBox.Location = new System.Drawing.Point(0, 0);
            this.bannerPicBox.Name = "bannerPicBox";
            this.bannerPicBox.Size = new System.Drawing.Size(371, 76);
            this.bannerPicBox.TabIndex = 3;
            this.bannerPicBox.TabStop = false;
            // 
            // deckLocLbl
            // 
            this.deckLocLbl.AutoSize = true;
            this.deckLocLbl.Location = new System.Drawing.Point(12, 101);
            this.deckLocLbl.Name = "deckLocLbl";
            this.deckLocLbl.Size = new System.Drawing.Size(147, 13);
            this.deckLocLbl.TabIndex = 4;
            this.deckLocLbl.Text = "Decks / Expansions Location";
            // 
            // deckLocTBox
            // 
            this.deckLocTBox.Location = new System.Drawing.Point(12, 122);
            this.deckLocTBox.Name = "deckLocTBox";
            this.deckLocTBox.Size = new System.Drawing.Size(306, 20);
            this.deckLocTBox.TabIndex = 5;
            // 
            // deckLocBtn
            // 
            this.deckLocBtn.Location = new System.Drawing.Point(324, 122);
            this.deckLocBtn.Name = "deckLocBtn";
            this.deckLocBtn.Size = new System.Drawing.Size(35, 20);
            this.deckLocBtn.TabIndex = 6;
            this.deckLocBtn.Text = "...";
            this.deckLocBtn.UseVisualStyleBackColor = true;
            this.deckLocBtn.Click += new System.EventHandler(this.deckLocBtn_Click);
            // 
            // portNumLbl
            // 
            this.portNumLbl.AutoSize = true;
            this.portNumLbl.Location = new System.Drawing.Point(12, 156);
            this.portNumLbl.Name = "portNumLbl";
            this.portNumLbl.Size = new System.Drawing.Size(71, 13);
            this.portNumLbl.TabIndex = 7;
            this.portNumLbl.Text = "Listening Port";
            // 
            // portNumTBox
            // 
            this.portNumTBox.Location = new System.Drawing.Point(15, 176);
            this.portNumTBox.Name = "portNumTBox";
            this.portNumTBox.Size = new System.Drawing.Size(192, 20);
            this.portNumTBox.TabIndex = 8;
            // 
            // portNumExLbl
            // 
            this.portNumExLbl.AutoSize = true;
            this.portNumExLbl.Enabled = false;
            this.portNumExLbl.Location = new System.Drawing.Point(213, 179);
            this.portNumExLbl.Name = "portNumExLbl";
            this.portNumExLbl.Size = new System.Drawing.Size(146, 13);
            this.portNumExLbl.TabIndex = 9;
            this.portNumExLbl.Text = "Port 11235 is the default port.";
            // 
            // settingsForm
            // 
            this.AcceptButton = this.saveBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.CancelButton = this.cancelBtn;
            this.ClientSize = new System.Drawing.Size(371, 364);
            this.ControlBox = false;
            this.Controls.Add(this.portNumExLbl);
            this.Controls.Add(this.portNumTBox);
            this.Controls.Add(this.portNumLbl);
            this.Controls.Add(this.deckLocBtn);
            this.Controls.Add(this.deckLocTBox);
            this.Controls.Add(this.deckLocLbl);
            this.Controls.Add(this.bannerPicBox);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.resetBtn);
            this.Controls.Add(this.saveBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "settingsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Server Settings";
            this.Load += new System.EventHandler(this.settingsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bannerPicBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button saveBtn;
        private System.Windows.Forms.Button resetBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.PictureBox bannerPicBox;
        private System.Windows.Forms.Label deckLocLbl;
        private System.Windows.Forms.TextBox deckLocTBox;
        private System.Windows.Forms.Button deckLocBtn;
        private System.Windows.Forms.Label portNumLbl;
        private System.Windows.Forms.TextBox portNumTBox;
        private System.Windows.Forms.Label portNumExLbl;
    }
}