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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.lb_Players = new System.Windows.Forms.ListBox();
			this.sts_GameStatus = new System.Windows.Forms.StatusStrip();
			this.tbx_Chat = new System.Windows.Forms.TextBox();
			this.cardList1 = new AppsAgainstHumanityClient.CardList();
			this.crl_PickedCards = new AppsAgainstHumanityClient.CardList();
			this.crd_BlackCard = new AppsAgainstHumanityClient.Card();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
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
			this.groupBox3.Controls.Add(this.cardList1);
			this.groupBox3.Location = new System.Drawing.Point(150, 290);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(779, 148);
			this.groupBox3.TabIndex = 4;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Your cards";
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
			this.sts_GameStatus.Location = new System.Drawing.Point(0, 454);
			this.sts_GameStatus.Name = "sts_GameStatus";
			this.sts_GameStatus.Size = new System.Drawing.Size(938, 22);
			this.sts_GameStatus.TabIndex = 1;
			this.sts_GameStatus.Text = "statusStrip1";
			// 
			// tbx_Chat
			// 
			this.tbx_Chat.Location = new System.Drawing.Point(0, 436);
			this.tbx_Chat.Name = "tbx_Chat";
			this.tbx_Chat.Size = new System.Drawing.Size(938, 20);
			this.tbx_Chat.TabIndex = 6;
			// 
			// cardList1
			// 
			this.cardList1.AutoScroll = true;
			this.cardList1.Location = new System.Drawing.Point(6, 20);
			this.cardList1.Name = "cardList1";
			this.cardList1.Size = new System.Drawing.Size(767, 120);
			this.cardList1.TabIndex = 0;
			// 
			// crl_PickedCards
			// 
			this.crl_PickedCards.AutoScroll = true;
			this.crl_PickedCards.Location = new System.Drawing.Point(6, 19);
			this.crl_PickedCards.Name = "crl_PickedCards";
			this.crl_PickedCards.Size = new System.Drawing.Size(767, 246);
			this.crl_PickedCards.TabIndex = 0;
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
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(938, 476);
			this.Controls.Add(this.tbx_Chat);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.sts_GameStatus);
			this.Controls.Add(this.groupBox1);
			this.Name = "MainForm";
			this.Text = "Apps Against Humanity";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
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
		private CardList cardList1;
		private System.Windows.Forms.StatusStrip sts_GameStatus;
		private System.Windows.Forms.TextBox tbx_Chat;
	}
}

