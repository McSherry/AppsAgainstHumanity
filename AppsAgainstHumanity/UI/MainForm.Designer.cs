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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.crd_BlackCard = new AppsAgainstHumanityClient.Card(false);
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.crl_PickedCards = new AppsAgainstHumanityClient.CardList();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.crl_OwnedCards = new AppsAgainstHumanityClient.CardList();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.lb_Players = new System.Windows.Forms.ListBox();
			this.sts_GameStatus = new System.Windows.Forms.StatusStrip();
			this.stl_GameStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.tbx_Chat = new System.Windows.Forms.TextBox();
			this.btn_GameAction = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button9 = new System.Windows.Forms.Button();
			this.tbx_ChatLog = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.sts_GameStatus.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.crd_BlackCard);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(132, 145);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Black card";
			// 
			// crd_BlackCard
			// 
			this.crd_BlackCard.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.crd_BlackCard.Contents = "Card contents go here";
			this.crd_BlackCard.Location = new System.Drawing.Point(6, 19);
			this.crd_BlackCard.Name = "crd_BlackCard";
			this.crd_BlackCard.Size = new System.Drawing.Size(120, 120);
			this.crd_BlackCard.TabIndex = 0;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.crl_PickedCards);
			this.groupBox2.Location = new System.Drawing.Point(150, 12);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(779, 272);
			this.groupBox2.TabIndex = 3;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Picked cards";
			// 
			// crl_PickedCards
			// 
			this.crl_PickedCards.AutoScroll = true;
			this.crl_PickedCards.Location = new System.Drawing.Point(6, 19);
			this.crl_PickedCards.Name = "crl_PickedCards";
			this.crl_PickedCards.Size = new System.Drawing.Size(767, 246);
			this.crl_PickedCards.TabIndex = 0;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.crl_OwnedCards);
			this.groupBox3.Location = new System.Drawing.Point(150, 290);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(779, 148);
			this.groupBox3.TabIndex = 4;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Your cards";
			// 
			// crl_OwnedCards
			// 
			this.crl_OwnedCards.AutoScroll = true;
			this.crl_OwnedCards.Location = new System.Drawing.Point(6, 20);
			this.crl_OwnedCards.Name = "crl_OwnedCards";
			this.crl_OwnedCards.Size = new System.Drawing.Size(767, 120);
			this.crl_OwnedCards.TabIndex = 0;
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.lb_Players);
			this.groupBox4.Location = new System.Drawing.Point(12, 163);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(132, 184);
			this.groupBox4.TabIndex = 5;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Players online";
			// 
			// lb_Players
			// 
			this.lb_Players.FormattingEnabled = true;
			this.lb_Players.Location = new System.Drawing.Point(6, 20);
			this.lb_Players.Name = "lb_Players";
			this.lb_Players.Size = new System.Drawing.Size(120, 160);
			this.lb_Players.TabIndex = 0;
			// 
			// sts_GameStatus
			// 
			this.sts_GameStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stl_GameStatusLabel});
			this.sts_GameStatus.Location = new System.Drawing.Point(0, 582);
			this.sts_GameStatus.Name = "sts_GameStatus";
			this.sts_GameStatus.Size = new System.Drawing.Size(938, 22);
			this.sts_GameStatus.TabIndex = 1;
			this.sts_GameStatus.Text = "statusStrip1";
			// 
			// stl_GameStatusLabel
			// 
			this.stl_GameStatusLabel.Name = "stl_GameStatusLabel";
			this.stl_GameStatusLabel.Size = new System.Drawing.Size(118, 17);
			this.stl_GameStatusLabel.Text = "toolStripStatusLabel1";
			// 
			// tbx_Chat
			// 
			this.tbx_Chat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbx_Chat.Location = new System.Drawing.Point(0, 564);
			this.tbx_Chat.Name = "tbx_Chat";
			this.tbx_Chat.Size = new System.Drawing.Size(864, 20);
			this.tbx_Chat.TabIndex = 6;
			this.tbx_Chat.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbx_Chat_KeyDown);
			this.tbx_Chat.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbx_Chat_KeyPress);
			// 
			// btn_GameAction
			// 
			this.btn_GameAction.Location = new System.Drawing.Point(12, 353);
			this.btn_GameAction.Name = "btn_GameAction";
			this.btn_GameAction.Size = new System.Drawing.Size(132, 23);
			this.btn_GameAction.TabIndex = 7;
			this.btn_GameAction.Text = "Submit Selected";
			this.btn_GameAction.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(12, 382);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(132, 23);
			this.button2.TabIndex = 8;
			this.button2.Text = "I am without purpose";
			this.button2.UseVisualStyleBackColor = true;
			// 
			// button9
			// 
			this.button9.Location = new System.Drawing.Point(12, 411);
			this.button9.Name = "button9";
			this.button9.Size = new System.Drawing.Size(132, 23);
			this.button9.TabIndex = 9;
			this.button9.Text = "I am without meaning";
			this.button9.UseVisualStyleBackColor = true;
			// 
			// tbx_ChatLog
			// 
			this.tbx_ChatLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbx_ChatLog.Location = new System.Drawing.Point(0, 440);
			this.tbx_ChatLog.Multiline = true;
			this.tbx_ChatLog.Name = "tbx_ChatLog";
			this.tbx_ChatLog.ReadOnly = true;
			this.tbx_ChatLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tbx_ChatLog.Size = new System.Drawing.Size(938, 120);
			this.tbx_ChatLog.TabIndex = 0;
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Location = new System.Drawing.Point(863, 564);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 20);
			this.button1.TabIndex = 10;
			this.button1.Text = "Send";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(938, 604);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.tbx_ChatLog);
			this.Controls.Add(this.button9);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.btn_GameAction);
			this.Controls.Add(this.tbx_Chat);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.sts_GameStatus);
			this.Controls.Add(this.groupBox1);
			this.MaximumSize = new System.Drawing.Size(954, 1080);
			this.MinimumSize = new System.Drawing.Size(954, 642);
			this.Name = "MainForm";
			this.Text = "Apps Against Humanity";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.sts_GameStatus.ResumeLayout(false);
			this.sts_GameStatus.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.ListBox lb_Players;
		private Card crd_BlackCard;
		private CardList crl_PickedCards;
		private CardList crl_OwnedCards;
		private System.Windows.Forms.StatusStrip sts_GameStatus;
		private System.Windows.Forms.TextBox tbx_Chat;
		private System.Windows.Forms.Button btn_GameAction;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button9;
		private System.Windows.Forms.TextBox tbx_ChatLog;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ToolStripStatusLabel stl_GameStatusLabel;
	}
}

