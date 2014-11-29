namespace AppsAgainstHumanityClient
{
	partial class ConnectionForm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectionForm));
            this.tbx_Host = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbx_Nick = new System.Windows.Forms.TextBox();
            this.btn_Connect = new System.Windows.Forms.Button();
            this.btn_Exit = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbx_Port = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.websiteLLbl = new System.Windows.Forms.LinkLabel();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tbx_Host
            // 
            this.tbx_Host.Location = new System.Drawing.Point(9, 42);
            this.tbx_Host.Name = "tbx_Host";
            this.tbx_Host.Size = new System.Drawing.Size(307, 20);
            this.tbx_Host.TabIndex = 0;
            this.tbx_Host.Text = "127.0.0.1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Server Address";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Server Port";
            // 
            // tbx_Nick
            // 
            this.tbx_Nick.Location = new System.Drawing.Point(9, 152);
            this.tbx_Nick.Name = "tbx_Nick";
            this.tbx_Nick.Size = new System.Drawing.Size(307, 20);
            this.tbx_Nick.TabIndex = 3;
            this.tbx_Nick.Text = "Liam";
            // 
            // btn_Connect
            // 
            this.btn_Connect.Location = new System.Drawing.Point(127, 190);
            this.btn_Connect.Name = "btn_Connect";
            this.btn_Connect.Size = new System.Drawing.Size(195, 23);
            this.btn_Connect.TabIndex = 4;
            this.btn_Connect.Text = "Connect";
            this.btn_Connect.UseVisualStyleBackColor = true;
            this.btn_Connect.Click += new System.EventHandler(this.btn_Connect_Click);
            // 
            // btn_Exit
            // 
            this.btn_Exit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Exit.Location = new System.Drawing.Point(6, 190);
            this.btn_Exit.Name = "btn_Exit";
            this.btn_Exit.Size = new System.Drawing.Size(115, 23);
            this.btn_Exit.TabIndex = 5;
            this.btn_Exit.Text = "Exit";
            this.btn_Exit.UseVisualStyleBackColor = true;
            this.btn_Exit.Click += new System.EventHandler(this.btn_Exit_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tbx_Port);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btn_Connect);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbx_Nick);
            this.groupBox1.Controls.Add(this.btn_Exit);
            this.groupBox1.Controls.Add(this.tbx_Host);
            this.groupBox1.Location = new System.Drawing.Point(165, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(328, 221);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connection Information";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 132);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(114, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Username / Nickname";
            // 
            // tbx_Port
            // 
            this.tbx_Port.Location = new System.Drawing.Point(9, 97);
            this.tbx_Port.Name = "tbx_Port";
            this.tbx_Port.Size = new System.Drawing.Size(150, 20);
            this.tbx_Port.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Enabled = false;
            this.label3.Location = new System.Drawing.Point(165, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(157, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Leave empty for default (11235)";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::AppsAgainstHumanityClient.Properties.Resources.logo64;
            this.pictureBox1.Location = new System.Drawing.Point(52, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(64, 64);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Enabled = false;
            this.label5.Location = new System.Drawing.Point(12, 87);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(146, 26);
            this.label5.TabIndex = 8;
            this.label5.Text = "Copyright 2013-2014 (c)\r\n Johan Geluk, Liam McSherry";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // websiteLLbl
            // 
            this.websiteLLbl.ActiveLinkColor = System.Drawing.Color.DodgerBlue;
            this.websiteLLbl.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.websiteLLbl.LinkColor = System.Drawing.Color.DodgerBlue;
            this.websiteLLbl.Location = new System.Drawing.Point(12, 125);
            this.websiteLLbl.Name = "websiteLLbl";
            this.websiteLLbl.Size = new System.Drawing.Size(146, 13);
            this.websiteLLbl.TabIndex = 9;
            this.websiteLLbl.TabStop = true;
            this.websiteLLbl.Text = "getaah.net";
            this.websiteLLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.websiteLLbl.VisitedLinkColor = System.Drawing.Color.DodgerBlue;
            this.websiteLLbl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.websiteLLbl_LinkClicked);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Enabled = false;
            this.label6.Location = new System.Drawing.Point(6, 220);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "label6";
            // 
            // ConnectionForm
            // 
            this.AcceptButton = this.btn_Connect;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.CancelButton = this.btn_Exit;
            this.ClientSize = new System.Drawing.Size(505, 239);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.websiteLLbl);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ConnectionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Apps Against Humanity - Connect to a Server";
            this.Load += new System.EventHandler(this.ConnectionForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox tbx_Host;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tbx_Nick;
		private System.Windows.Forms.Button btn_Connect;
		private System.Windows.Forms.Button btn_Exit;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbx_Port;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.LinkLabel websiteLLbl;
        private System.Windows.Forms.Label label6;
	}
}