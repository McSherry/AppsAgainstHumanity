namespace AppsAgainstHumanityClient
{
	partial class MainForm
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
			this.sts_GameStatus = new System.Windows.Forms.StatusStrip();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.lb_Players = new System.Windows.Forms.ListBox();
			this.tbx_Chat = new System.Windows.Forms.TextBox();
			this.groupBox4.SuspendLayout();
			this.SuspendLayout();
			// 
			// sts_GameStatus
			// 
			this.sts_GameStatus.Location = new System.Drawing.Point(0, 519);
			this.sts_GameStatus.Name = "sts_GameStatus";
			this.sts_GameStatus.Size = new System.Drawing.Size(777, 22);
			this.sts_GameStatus.TabIndex = 1;
			this.sts_GameStatus.Text = "statusStrip1";
			// 
			// groupBox1
			// 
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(137, 165);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Black card";
			// 
			// groupBox2
			// 
			this.groupBox2.Location = new System.Drawing.Point(156, 13);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(609, 164);
			this.groupBox2.TabIndex = 3;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Picked cards";
			// 
			// groupBox3
			// 
			this.groupBox3.Location = new System.Drawing.Point(156, 184);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(609, 185);
			this.groupBox3.TabIndex = 4;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Your cards";
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.lb_Players);
			this.groupBox4.Location = new System.Drawing.Point(12, 184);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(138, 185);
			this.groupBox4.TabIndex = 5;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Players online";
			// 
			// lb_Players
			// 
			this.lb_Players.FormattingEnabled = true;
			this.lb_Players.Location = new System.Drawing.Point(6, 20);
			this.lb_Players.Name = "lb_Players";
			this.lb_Players.Size = new System.Drawing.Size(126, 160);
			this.lb_Players.TabIndex = 0;
			// 
			// tbx_Chat
			// 
			this.tbx_Chat.Location = new System.Drawing.Point(0, 496);
			this.tbx_Chat.Name = "tbx_Chat";
			this.tbx_Chat.Size = new System.Drawing.Size(777, 20);
			this.tbx_Chat.TabIndex = 6;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(777, 541);
			this.Controls.Add(this.tbx_Chat);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.sts_GameStatus);
			this.Controls.Add(this.groupBox1);
			this.Name = "MainForm";
			this.Text = "Apps Against Humanity";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.groupBox4.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip sts_GameStatus;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.ListBox lb_Players;
		private System.Windows.Forms.TextBox tbx_Chat;
	}
}

