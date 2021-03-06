﻿namespace AppsAgainstHumanityClient
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lb_Players = new System.Windows.Forms.ListBox();
            this.sts_GameStatus = new System.Windows.Forms.StatusStrip();
            this.stl_GameStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tbx_Chat = new System.Windows.Forms.TextBox();
            this.btn_GameAction = new System.Windows.Forms.Button();
            this.tbx_ChatLog = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.lbl_PickTimeout = new System.Windows.Forms.Label();
            this.lbl_PlayingTo = new System.Windows.Forms.Label();
            this.lbl_PlayerLimit = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.crl_OwnedCards = new AppsAgainstHumanityClient.CardList();
            this.crl_PickedCards = new AppsAgainstHumanityClient.PickedCardList();
            this.crd_BlackCard = new AppsAgainstHumanityClient.BlackCard();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.sts_GameStatus.SuspendLayout();
            this.groupBox5.SuspendLayout();
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
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.crl_OwnedCards);
            this.groupBox3.Location = new System.Drawing.Point(150, 283);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(779, 268);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Your cards";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lb_Players);
            this.groupBox4.Location = new System.Drawing.Point(12, 163);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(132, 121);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Players online";
            // 
            // lb_Players
            // 
            this.lb_Players.FormattingEnabled = true;
            this.lb_Players.Location = new System.Drawing.Point(6, 20);
            this.lb_Players.Name = "lb_Players";
            this.lb_Players.Size = new System.Drawing.Size(120, 95);
            this.lb_Players.TabIndex = 0;
            // 
            // sts_GameStatus
            // 
            this.sts_GameStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stl_GameStatusLabel});
            this.sts_GameStatus.Location = new System.Drawing.Point(0, 678);
            this.sts_GameStatus.Name = "sts_GameStatus";
            this.sts_GameStatus.Size = new System.Drawing.Size(938, 22);
            this.sts_GameStatus.TabIndex = 1;
            this.sts_GameStatus.Text = "statusStrip1";
            // 
            // stl_GameStatusLabel
            // 
            this.stl_GameStatusLabel.Name = "stl_GameStatusLabel";
            this.stl_GameStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // tbx_Chat
            // 
            this.tbx_Chat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbx_Chat.Location = new System.Drawing.Point(0, 660);
            this.tbx_Chat.Name = "tbx_Chat";
            this.tbx_Chat.Size = new System.Drawing.Size(864, 20);
            this.tbx_Chat.TabIndex = 6;
            this.tbx_Chat.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbx_Chat_KeyDown);
            this.tbx_Chat.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbx_Chat_KeyPress);
            // 
            // btn_GameAction
            // 
            this.btn_GameAction.Location = new System.Drawing.Point(12, 404);
            this.btn_GameAction.Name = "btn_GameAction";
            this.btn_GameAction.Size = new System.Drawing.Size(132, 23);
            this.btn_GameAction.TabIndex = 7;
            this.btn_GameAction.Text = "Submit Selected";
            this.btn_GameAction.UseVisualStyleBackColor = true;
            this.btn_GameAction.Click += new System.EventHandler(this.btn_GameAction_Click);
            // 
            // tbx_ChatLog
            // 
            this.tbx_ChatLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbx_ChatLog.Location = new System.Drawing.Point(0, 565);
            this.tbx_ChatLog.Multiline = true;
            this.tbx_ChatLog.Name = "tbx_ChatLog";
            this.tbx_ChatLog.ReadOnly = true;
            this.tbx_ChatLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbx_ChatLog.Size = new System.Drawing.Size(938, 90);
            this.tbx_ChatLog.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(863, 660);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 20);
            this.button1.TabIndex = 10;
            this.button1.Text = "Send";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.lbl_PickTimeout);
            this.groupBox5.Controls.Add(this.lbl_PlayingTo);
            this.groupBox5.Controls.Add(this.lbl_PlayerLimit);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Location = new System.Drawing.Point(13, 283);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(131, 115);
            this.groupBox5.TabIndex = 11;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Game Info";
            // 
            // lbl_PickTimeout
            // 
            this.lbl_PickTimeout.AutoSize = true;
            this.lbl_PickTimeout.Location = new System.Drawing.Point(90, 84);
            this.lbl_PickTimeout.Name = "lbl_PickTimeout";
            this.lbl_PickTimeout.Size = new System.Drawing.Size(0, 13);
            this.lbl_PickTimeout.TabIndex = 5;
            // 
            // lbl_PlayingTo
            // 
            this.lbl_PlayingTo.AutoSize = true;
            this.lbl_PlayingTo.Location = new System.Drawing.Point(90, 57);
            this.lbl_PlayingTo.Name = "lbl_PlayingTo";
            this.lbl_PlayingTo.Size = new System.Drawing.Size(0, 13);
            this.lbl_PlayingTo.TabIndex = 4;
            // 
            // lbl_PlayerLimit
            // 
            this.lbl_PlayerLimit.AutoSize = true;
            this.lbl_PlayerLimit.Location = new System.Drawing.Point(90, 28);
            this.lbl_PlayerLimit.Name = "lbl_PlayerLimit";
            this.lbl_PlayerLimit.Size = new System.Drawing.Size(0, 13);
            this.lbl_PlayerLimit.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Pick timeout:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Playing to:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Player limit:";
            // 
            // crl_OwnedCards
            // 
            this.crl_OwnedCards.AutoScroll = true;
            this.crl_OwnedCards.CanSelectCards = false;
            this.crl_OwnedCards.Location = new System.Drawing.Point(6, 20);
            this.crl_OwnedCards.MaxSelectNum = 0;
            this.crl_OwnedCards.Name = "crl_OwnedCards";
            this.crl_OwnedCards.Size = new System.Drawing.Size(767, 246);
            this.crl_OwnedCards.TabIndex = 0;
            // 
            // crl_PickedCards
            // 
            this.crl_PickedCards.AutoScroll = true;
            this.crl_PickedCards.CanSelectCards = false;
            this.crl_PickedCards.GroupNumber = 0;
            this.crl_PickedCards.Location = new System.Drawing.Point(6, 19);
            this.crl_PickedCards.MaxSelectNum = 0;
            this.crl_PickedCards.Name = "crl_PickedCards";
            this.crl_PickedCards.Size = new System.Drawing.Size(767, 246);
            this.crl_PickedCards.TabIndex = 0;
            // 
            // crd_BlackCard
            // 
            this.crd_BlackCard.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.crd_BlackCard.CardText = "";
            this.crd_BlackCard.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.crd_BlackCard.Location = new System.Drawing.Point(6, 19);
            this.crd_BlackCard.Name = "crd_BlackCard";
            this.crd_BlackCard.PickNum = 0;
            this.crd_BlackCard.SelectionIndex = 0;
            this.crd_BlackCard.Size = new System.Drawing.Size(120, 120);
            this.crd_BlackCard.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(938, 700);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tbx_ChatLog);
            this.Controls.Add(this.btn_GameAction);
            this.Controls.Add(this.tbx_Chat);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.sts_GameStatus);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(954, 1080);
            this.MinimumSize = new System.Drawing.Size(954, 642);
            this.Name = "MainForm";
            this.Text = "Apps Against Humanity";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.sts_GameStatus.ResumeLayout(false);
            this.sts_GameStatus.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.ListBox lb_Players;
		private BlackCard crd_BlackCard;
		private PickedCardList crl_PickedCards;
		private CardList crl_OwnedCards;
		private System.Windows.Forms.StatusStrip sts_GameStatus;
		private System.Windows.Forms.TextBox tbx_Chat;
		private System.Windows.Forms.Button btn_GameAction;
		private System.Windows.Forms.TextBox tbx_ChatLog;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ToolStripStatusLabel stl_GameStatusLabel;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lbl_PickTimeout;
		private System.Windows.Forms.Label lbl_PlayingTo;
        private System.Windows.Forms.Label lbl_PlayerLimit;
	}
}

