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
			this.tbx_Host = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.tbx_Nick = new System.Windows.Forms.TextBox();
			this.btn_Connect = new System.Windows.Forms.Button();
			this.btn_Exit = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// tbx_Host
			// 
			this.tbx_Host.Location = new System.Drawing.Point(96, 10);
			this.tbx_Host.Name = "tbx_Host";
			this.tbx_Host.Size = new System.Drawing.Size(133, 20);
			this.tbx_Host.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(54, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Server IP:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(58, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Username:";
			// 
			// tbx_Nick
			// 
			this.tbx_Nick.Location = new System.Drawing.Point(96, 36);
			this.tbx_Nick.Name = "tbx_Nick";
			this.tbx_Nick.Size = new System.Drawing.Size(133, 20);
			this.tbx_Nick.TabIndex = 3;
			// 
			// btn_Connect
			// 
			this.btn_Connect.Location = new System.Drawing.Point(118, 78);
			this.btn_Connect.Name = "btn_Connect";
			this.btn_Connect.Size = new System.Drawing.Size(111, 23);
			this.btn_Connect.TabIndex = 4;
			this.btn_Connect.Text = "Connect";
			this.btn_Connect.UseVisualStyleBackColor = true;
			this.btn_Connect.Click += new System.EventHandler(this.btn_Connect_Click);
			// 
			// btn_Exit
			// 
			this.btn_Exit.Location = new System.Drawing.Point(12, 78);
			this.btn_Exit.Name = "btn_Exit";
			this.btn_Exit.Size = new System.Drawing.Size(100, 23);
			this.btn_Exit.TabIndex = 5;
			this.btn_Exit.Text = "Exit";
			this.btn_Exit.UseVisualStyleBackColor = true;
			// 
			// ConnectionForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(241, 113);
			this.Controls.Add(this.btn_Exit);
			this.Controls.Add(this.btn_Connect);
			this.Controls.Add(this.tbx_Nick);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.tbx_Host);
			this.MaximumSize = new System.Drawing.Size(257, 151);
			this.MinimumSize = new System.Drawing.Size(257, 151);
			this.Name = "ConnectionForm";
			this.Text = "Connect to a server";
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
	}
}