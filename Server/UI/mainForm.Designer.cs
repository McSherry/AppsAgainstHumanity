namespace AppsAgainstHumanity.Server.UI
{
	partial class mainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            this.cardDeckLbl = new System.Windows.Forms.Label();
            this.serverChatRTBox = new System.Windows.Forms.RichTextBox();
            this.broadcastTBox = new System.Windows.Forms.TextBox();
            this.broadcastBtn = new System.Windows.Forms.Button();
            this.cardDeckCBox = new System.Windows.Forms.ComboBox();
            this.gameMonitorGBox = new System.Windows.Forms.GroupBox();
            this.connectedPlayersListBox = new System.Windows.Forms.ListBox();
            this.gameConfigGBox = new System.Windows.Forms.GroupBox();
            this.expansionPackButtons = new System.Windows.Forms.Button();
            this.serverStatusIndicLbl = new System.Windows.Forms.Label();
            this.serverStatusIndicRect = new System.Windows.Forms.Panel();
            this.gameRulesetCBox = new System.Windows.Forms.ComboBox();
            this.gameRulesetLbl = new System.Windows.Forms.Label();
            this.serverStartBtn = new System.Windows.Forms.Button();
            this.deckReloadBtn = new System.Windows.Forms.Button();
            this.czarSelectCBox = new System.Windows.Forms.ComboBox();
            this.czarSelectLbl = new System.Windows.Forms.Label();
            this.allowChatCheckBox = new System.Windows.Forms.CheckBox();
            this.timeoutKickCheckBox = new System.Windows.Forms.CheckBox();
            this.allowGamblingCheckBox = new System.Windows.Forms.CheckBox();
            this.timeoutLimitCBox = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.awesomePointsLimitBox = new System.Windows.Forms.NumericUpDown();
            this.pointLimitLbl = new System.Windows.Forms.Label();
            this.playerLimitBox = new System.Windows.Forms.NumericUpDown();
            this.playerLimitLbl = new System.Windows.Forms.Label();
            this.gameStopBtn = new System.Windows.Forms.Button();
            this.gameStartBtn = new System.Windows.Forms.Button();
            this.serverVersionLbl = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.aahAboutDescRTBox = new System.Windows.Forms.RichTextBox();
            this.aahWebLinkLbl = new System.Windows.Forms.LinkLabel();
            this.serverSettingsBtn = new System.Windows.Forms.Button();
            this.gameMonitorGBox.SuspendLayout();
            this.gameConfigGBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeoutLimitCBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.awesomePointsLimitBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.playerLimitBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // cardDeckLbl
            // 
            this.cardDeckLbl.AutoSize = true;
            this.cardDeckLbl.Location = new System.Drawing.Point(9, 29);
            this.cardDeckLbl.Name = "cardDeckLbl";
            this.cardDeckLbl.Size = new System.Drawing.Size(58, 13);
            this.cardDeckLbl.TabIndex = 1;
            this.cardDeckLbl.Text = "Card Deck";
            // 
            // serverChatRTBox
            // 
            this.serverChatRTBox.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.serverChatRTBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.serverChatRTBox.HideSelection = false;
            this.serverChatRTBox.Location = new System.Drawing.Point(215, 19);
            this.serverChatRTBox.Name = "serverChatRTBox";
            this.serverChatRTBox.ReadOnly = true;
            this.serverChatRTBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.serverChatRTBox.ShortcutsEnabled = false;
            this.serverChatRTBox.Size = new System.Drawing.Size(332, 149);
            this.serverChatRTBox.TabIndex = 2;
            this.serverChatRTBox.TabStop = false;
            this.serverChatRTBox.Text = "";
            this.serverChatRTBox.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.serverChatRTBox_LinkClicked);
            // 
            // broadcastTBox
            // 
            this.broadcastTBox.Location = new System.Drawing.Point(12, 174);
            this.broadcastTBox.MaxLength = 250;
            this.broadcastTBox.Name = "broadcastTBox";
            this.broadcastTBox.Size = new System.Drawing.Size(446, 20);
            this.broadcastTBox.TabIndex = 3;
            // 
            // broadcastBtn
            // 
            this.broadcastBtn.Location = new System.Drawing.Point(464, 174);
            this.broadcastBtn.Name = "broadcastBtn";
            this.broadcastBtn.Size = new System.Drawing.Size(83, 23);
            this.broadcastBtn.TabIndex = 4;
            this.broadcastBtn.Text = "Broadcast";
            this.broadcastBtn.UseVisualStyleBackColor = true;
            this.broadcastBtn.Click += new System.EventHandler(this.broadcastBtn_Click);
            // 
            // cardDeckCBox
            // 
            this.cardDeckCBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cardDeckCBox.FormattingEnabled = true;
            this.cardDeckCBox.Location = new System.Drawing.Point(73, 23);
            this.cardDeckCBox.Name = "cardDeckCBox";
            this.cardDeckCBox.Size = new System.Drawing.Size(368, 21);
            this.cardDeckCBox.TabIndex = 5;
            // 
            // gameMonitorGBox
            // 
            this.gameMonitorGBox.Controls.Add(this.connectedPlayersListBox);
            this.gameMonitorGBox.Controls.Add(this.serverChatRTBox);
            this.gameMonitorGBox.Controls.Add(this.broadcastTBox);
            this.gameMonitorGBox.Controls.Add(this.broadcastBtn);
            this.gameMonitorGBox.Location = new System.Drawing.Point(255, 242);
            this.gameMonitorGBox.Name = "gameMonitorGBox";
            this.gameMonitorGBox.Size = new System.Drawing.Size(558, 206);
            this.gameMonitorGBox.TabIndex = 6;
            this.gameMonitorGBox.TabStop = false;
            this.gameMonitorGBox.Text = "Game Monitor";
            // 
            // connectedPlayersListBox
            // 
            this.connectedPlayersListBox.FormattingEnabled = true;
            this.connectedPlayersListBox.Location = new System.Drawing.Point(12, 19);
            this.connectedPlayersListBox.Name = "connectedPlayersListBox";
            this.connectedPlayersListBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.connectedPlayersListBox.Size = new System.Drawing.Size(197, 147);
            this.connectedPlayersListBox.TabIndex = 5;
            // 
            // gameConfigGBox
            // 
            this.gameConfigGBox.Controls.Add(this.expansionPackButtons);
            this.gameConfigGBox.Controls.Add(this.serverStatusIndicLbl);
            this.gameConfigGBox.Controls.Add(this.serverStatusIndicRect);
            this.gameConfigGBox.Controls.Add(this.gameRulesetCBox);
            this.gameConfigGBox.Controls.Add(this.gameRulesetLbl);
            this.gameConfigGBox.Controls.Add(this.serverStartBtn);
            this.gameConfigGBox.Controls.Add(this.deckReloadBtn);
            this.gameConfigGBox.Controls.Add(this.czarSelectCBox);
            this.gameConfigGBox.Controls.Add(this.czarSelectLbl);
            this.gameConfigGBox.Controls.Add(this.allowChatCheckBox);
            this.gameConfigGBox.Controls.Add(this.timeoutKickCheckBox);
            this.gameConfigGBox.Controls.Add(this.allowGamblingCheckBox);
            this.gameConfigGBox.Controls.Add(this.timeoutLimitCBox);
            this.gameConfigGBox.Controls.Add(this.label1);
            this.gameConfigGBox.Controls.Add(this.awesomePointsLimitBox);
            this.gameConfigGBox.Controls.Add(this.pointLimitLbl);
            this.gameConfigGBox.Controls.Add(this.playerLimitBox);
            this.gameConfigGBox.Controls.Add(this.playerLimitLbl);
            this.gameConfigGBox.Controls.Add(this.gameStopBtn);
            this.gameConfigGBox.Controls.Add(this.gameStartBtn);
            this.gameConfigGBox.Controls.Add(this.cardDeckLbl);
            this.gameConfigGBox.Controls.Add(this.cardDeckCBox);
            this.gameConfigGBox.Location = new System.Drawing.Point(255, 12);
            this.gameConfigGBox.Name = "gameConfigGBox";
            this.gameConfigGBox.Size = new System.Drawing.Size(558, 224);
            this.gameConfigGBox.TabIndex = 7;
            this.gameConfigGBox.TabStop = false;
            this.gameConfigGBox.Text = "Game Configuration";
            // 
            // expansionPackButtons
            // 
            this.expansionPackButtons.Location = new System.Drawing.Point(447, 50);
            this.expansionPackButtons.Name = "expansionPackButtons";
            this.expansionPackButtons.Size = new System.Drawing.Size(100, 23);
            this.expansionPackButtons.TabIndex = 23;
            this.expansionPackButtons.Text = "Expansions";
            this.expansionPackButtons.UseVisualStyleBackColor = true;
            this.expansionPackButtons.Click += new System.EventHandler(this.expansionPackButtons_Click);
            // 
            // serverStatusIndicLbl
            // 
            this.serverStatusIndicLbl.AutoSize = true;
            this.serverStatusIndicLbl.ForeColor = System.Drawing.Color.Red;
            this.serverStatusIndicLbl.Location = new System.Drawing.Point(413, 132);
            this.serverStatusIndicLbl.Name = "serverStatusIndicLbl";
            this.serverStatusIndicLbl.Size = new System.Drawing.Size(40, 13);
            this.serverStatusIndicLbl.TabIndex = 22;
            this.serverStatusIndicLbl.Text = "Offline.";
            // 
            // serverStatusIndicRect
            // 
            this.serverStatusIndicRect.BackColor = System.Drawing.Color.Red;
            this.serverStatusIndicRect.Location = new System.Drawing.Point(391, 130);
            this.serverStatusIndicRect.Name = "serverStatusIndicRect";
            this.serverStatusIndicRect.Size = new System.Drawing.Size(17, 17);
            this.serverStatusIndicRect.TabIndex = 21;
            // 
            // gameRulesetCBox
            // 
            this.gameRulesetCBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gameRulesetCBox.Enabled = false;
            this.gameRulesetCBox.FormattingEnabled = true;
            this.gameRulesetCBox.Items.AddRange(new object[] {
            "Standard",
            "Rebooting the Universe",
            "Packing Heat",
            "Rando Cardrissian",
            "God Is Dead",
            "Survival of the Fittest",
            "Serious Business"});
            this.gameRulesetCBox.Location = new System.Drawing.Point(327, 79);
            this.gameRulesetCBox.Name = "gameRulesetCBox";
            this.gameRulesetCBox.Size = new System.Drawing.Size(220, 21);
            this.gameRulesetCBox.TabIndex = 20;
            // 
            // gameRulesetLbl
            // 
            this.gameRulesetLbl.AutoSize = true;
            this.gameRulesetLbl.Location = new System.Drawing.Point(247, 84);
            this.gameRulesetLbl.Name = "gameRulesetLbl";
            this.gameRulesetLbl.Size = new System.Drawing.Size(74, 13);
            this.gameRulesetLbl.TabIndex = 19;
            this.gameRulesetLbl.Text = "Game Ruleset";
            // 
            // serverStartBtn
            // 
            this.serverStartBtn.Location = new System.Drawing.Point(391, 160);
            this.serverStartBtn.Name = "serverStartBtn";
            this.serverStartBtn.Size = new System.Drawing.Size(156, 23);
            this.serverStartBtn.TabIndex = 18;
            this.serverStartBtn.Text = "Start Server";
            this.serverStartBtn.UseVisualStyleBackColor = true;
            this.serverStartBtn.Click += new System.EventHandler(this.serverStartBtn_Click);
            // 
            // deckReloadBtn
            // 
            this.deckReloadBtn.Location = new System.Drawing.Point(447, 21);
            this.deckReloadBtn.Name = "deckReloadBtn";
            this.deckReloadBtn.Size = new System.Drawing.Size(100, 23);
            this.deckReloadBtn.TabIndex = 17;
            this.deckReloadBtn.Text = "Reload Decks";
            this.deckReloadBtn.UseVisualStyleBackColor = true;
            this.deckReloadBtn.Click += new System.EventHandler(this.deckReloadBtn_Click);
            // 
            // czarSelectCBox
            // 
            this.czarSelectCBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.czarSelectCBox.FormattingEnabled = true;
            this.czarSelectCBox.Items.AddRange(new object[] {
            "Sequential",
            "Random"});
            this.czarSelectCBox.Location = new System.Drawing.Point(115, 79);
            this.czarSelectCBox.Name = "czarSelectCBox";
            this.czarSelectCBox.Size = new System.Drawing.Size(121, 21);
            this.czarSelectCBox.TabIndex = 16;
            // 
            // czarSelectLbl
            // 
            this.czarSelectLbl.AutoSize = true;
            this.czarSelectLbl.Location = new System.Drawing.Point(9, 85);
            this.czarSelectLbl.Name = "czarSelectLbl";
            this.czarSelectLbl.Size = new System.Drawing.Size(100, 13);
            this.czarSelectLbl.TabIndex = 15;
            this.czarSelectLbl.Text = "Card Czar Selection";
            // 
            // allowChatCheckBox
            // 
            this.allowChatCheckBox.AutoSize = true;
            this.allowChatCheckBox.Checked = true;
            this.allowChatCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.allowChatCheckBox.Location = new System.Drawing.Point(267, 130);
            this.allowChatCheckBox.Name = "allowChatCheckBox";
            this.allowChatCheckBox.Size = new System.Drawing.Size(108, 17);
            this.allowChatCheckBox.TabIndex = 14;
            this.allowChatCheckBox.Text = "Allow Player Chat";
            this.allowChatCheckBox.UseVisualStyleBackColor = true;
            // 
            // timeoutKickCheckBox
            // 
            this.timeoutKickCheckBox.AutoSize = true;
            this.timeoutKickCheckBox.Location = new System.Drawing.Point(116, 130);
            this.timeoutKickCheckBox.Name = "timeoutKickCheckBox";
            this.timeoutKickCheckBox.Size = new System.Drawing.Size(145, 17);
            this.timeoutKickCheckBox.TabIndex = 13;
            this.timeoutKickCheckBox.Text = "Kick Players on Time-Out";
            this.timeoutKickCheckBox.UseVisualStyleBackColor = true;
            // 
            // allowGamblingCheckBox
            // 
            this.allowGamblingCheckBox.AutoSize = true;
            this.allowGamblingCheckBox.Enabled = false;
            this.allowGamblingCheckBox.Location = new System.Drawing.Point(12, 130);
            this.allowGamblingCheckBox.Name = "allowGamblingCheckBox";
            this.allowGamblingCheckBox.Size = new System.Drawing.Size(98, 17);
            this.allowGamblingCheckBox.TabIndex = 12;
            this.allowGamblingCheckBox.Text = "Allow Gambling";
            this.allowGamblingCheckBox.UseVisualStyleBackColor = true;
            // 
            // timeoutLimitCBox
            // 
            this.timeoutLimitCBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.timeoutLimitCBox.Location = new System.Drawing.Point(381, 53);
            this.timeoutLimitCBox.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.timeoutLimitCBox.Minimum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.timeoutLimitCBox.Name = "timeoutLimitCBox";
            this.timeoutLimitCBox.Size = new System.Drawing.Size(58, 20);
            this.timeoutLimitCBox.TabIndex = 11;
            this.timeoutLimitCBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.timeoutLimitCBox.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(305, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Time-Out Limit";
            // 
            // awesomePointsLimitBox
            // 
            this.awesomePointsLimitBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.awesomePointsLimitBox.Location = new System.Drawing.Point(250, 53);
            this.awesomePointsLimitBox.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.awesomePointsLimitBox.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.awesomePointsLimitBox.Name = "awesomePointsLimitBox";
            this.awesomePointsLimitBox.Size = new System.Drawing.Size(49, 20);
            this.awesomePointsLimitBox.TabIndex = 9;
            this.awesomePointsLimitBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.awesomePointsLimitBox.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // pointLimitLbl
            // 
            this.pointLimitLbl.AutoSize = true;
            this.pointLimitLbl.Location = new System.Drawing.Point(138, 57);
            this.pointLimitLbl.Name = "pointLimitLbl";
            this.pointLimitLbl.Size = new System.Drawing.Size(109, 13);
            this.pointLimitLbl.TabIndex = 8;
            this.pointLimitLbl.Text = "Awesome Points Limit";
            // 
            // playerLimitBox
            // 
            this.playerLimitBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.playerLimitBox.Location = new System.Drawing.Point(89, 53);
            this.playerLimitBox.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.playerLimitBox.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.playerLimitBox.Name = "playerLimitBox";
            this.playerLimitBox.Size = new System.Drawing.Size(40, 20);
            this.playerLimitBox.TabIndex = 7;
            this.playerLimitBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.playerLimitBox.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // playerLimitLbl
            // 
            this.playerLimitLbl.AutoSize = true;
            this.playerLimitLbl.Location = new System.Drawing.Point(26, 57);
            this.playerLimitLbl.Name = "playerLimitLbl";
            this.playerLimitLbl.Size = new System.Drawing.Size(60, 13);
            this.playerLimitLbl.TabIndex = 6;
            this.playerLimitLbl.Text = "Player Limit";
            // 
            // gameStopBtn
            // 
            this.gameStopBtn.Enabled = false;
            this.gameStopBtn.Location = new System.Drawing.Point(472, 189);
            this.gameStopBtn.Name = "gameStopBtn";
            this.gameStopBtn.Size = new System.Drawing.Size(75, 23);
            this.gameStopBtn.TabIndex = 1;
            this.gameStopBtn.Text = "Stop";
            this.gameStopBtn.UseVisualStyleBackColor = true;
            this.gameStopBtn.Click += new System.EventHandler(this.gameStopBtn_Click);
            // 
            // gameStartBtn
            // 
            this.gameStartBtn.Enabled = false;
            this.gameStartBtn.Location = new System.Drawing.Point(391, 189);
            this.gameStartBtn.Name = "gameStartBtn";
            this.gameStartBtn.Size = new System.Drawing.Size(75, 23);
            this.gameStartBtn.TabIndex = 0;
            this.gameStartBtn.Text = "Start Game";
            this.gameStartBtn.UseVisualStyleBackColor = true;
            this.gameStartBtn.Click += new System.EventHandler(this.gameStartBtn_Click);
            // 
            // serverVersionLbl
            // 
            this.serverVersionLbl.AutoSize = true;
            this.serverVersionLbl.Enabled = false;
            this.serverVersionLbl.Location = new System.Drawing.Point(12, 438);
            this.serverVersionLbl.Name = "serverVersionLbl";
            this.serverVersionLbl.Size = new System.Drawing.Size(37, 13);
            this.serverVersionLbl.TabIndex = 8;
            this.serverVersionLbl.Text = "v0.0.0";
            this.serverVersionLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::AppsAgainstHumanity.Server.Properties.Resources.logo172_undertext;
            this.pictureBox1.InitialImage = global::AppsAgainstHumanity.Server.Properties.Resources.logo172_undertext;
            this.pictureBox1.Location = new System.Drawing.Point(43, 23);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(172, 172);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // aahAboutDescRTBox
            // 
            this.aahAboutDescRTBox.BackColor = System.Drawing.SystemColors.Control;
            this.aahAboutDescRTBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.aahAboutDescRTBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.aahAboutDescRTBox.Enabled = false;
            this.aahAboutDescRTBox.Location = new System.Drawing.Point(12, 203);
            this.aahAboutDescRTBox.Name = "aahAboutDescRTBox";
            this.aahAboutDescRTBox.ReadOnly = true;
            this.aahAboutDescRTBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.aahAboutDescRTBox.Size = new System.Drawing.Size(237, 82);
            this.aahAboutDescRTBox.TabIndex = 11;
            this.aahAboutDescRTBox.Text = "Copyright 2013 (c) Johan Geluk, Liam McSherry\n\nApps Against Humanity is released " +
    "under the Apache 2.0 licence, and the source code is available via GitHub.";
            // 
            // aahWebLinkLbl
            // 
            this.aahWebLinkLbl.ActiveLinkColor = System.Drawing.Color.DodgerBlue;
            this.aahWebLinkLbl.AutoSize = true;
            this.aahWebLinkLbl.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.aahWebLinkLbl.LinkColor = System.Drawing.Color.DodgerBlue;
            this.aahWebLinkLbl.Location = new System.Drawing.Point(103, 288);
            this.aahWebLinkLbl.Name = "aahWebLinkLbl";
            this.aahWebLinkLbl.Size = new System.Drawing.Size(58, 13);
            this.aahWebLinkLbl.TabIndex = 12;
            this.aahWebLinkLbl.TabStop = true;
            this.aahWebLinkLbl.Text = "getaah.net";
            this.aahWebLinkLbl.VisitedLinkColor = System.Drawing.Color.DodgerBlue;
            this.aahWebLinkLbl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.aahWebLinkLbl_LinkClicked);
            // 
            // serverSettingsBtn
            // 
            this.serverSettingsBtn.Location = new System.Drawing.Point(174, 428);
            this.serverSettingsBtn.Name = "serverSettingsBtn";
            this.serverSettingsBtn.Size = new System.Drawing.Size(75, 23);
            this.serverSettingsBtn.TabIndex = 13;
            this.serverSettingsBtn.Text = "Settings";
            this.serverSettingsBtn.UseVisualStyleBackColor = true;
            this.serverSettingsBtn.Click += new System.EventHandler(this.serverSettingsBtn_Click);
            // 
            // mainForm
            // 
            this.AcceptButton = this.broadcastBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(825, 460);
            this.Controls.Add(this.serverSettingsBtn);
            this.Controls.Add(this.aahWebLinkLbl);
            this.Controls.Add(this.aahAboutDescRTBox);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.serverVersionLbl);
            this.Controls.Add(this.gameConfigGBox);
            this.Controls.Add(this.gameMonitorGBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "mainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AppsAgainstHumanity Server";
            this.gameMonitorGBox.ResumeLayout(false);
            this.gameMonitorGBox.PerformLayout();
            this.gameConfigGBox.ResumeLayout(false);
            this.gameConfigGBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeoutLimitCBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.awesomePointsLimitBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.playerLimitBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.Label cardDeckLbl;
        private System.Windows.Forms.RichTextBox serverChatRTBox;
        private System.Windows.Forms.TextBox broadcastTBox;
        private System.Windows.Forms.Button broadcastBtn;
        private System.Windows.Forms.ComboBox cardDeckCBox;
        private System.Windows.Forms.GroupBox gameMonitorGBox;
        private System.Windows.Forms.GroupBox gameConfigGBox;
        private System.Windows.Forms.Button gameStopBtn;
        private System.Windows.Forms.Button gameStartBtn;
        private System.Windows.Forms.NumericUpDown awesomePointsLimitBox;
        private System.Windows.Forms.Label pointLimitLbl;
        private System.Windows.Forms.NumericUpDown playerLimitBox;
        private System.Windows.Forms.Label playerLimitLbl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown timeoutLimitCBox;
        private System.Windows.Forms.CheckBox timeoutKickCheckBox;
        private System.Windows.Forms.CheckBox allowGamblingCheckBox;
        private System.Windows.Forms.CheckBox allowChatCheckBox;
        private System.Windows.Forms.ComboBox czarSelectCBox;
        private System.Windows.Forms.Label czarSelectLbl;
        private System.Windows.Forms.Button deckReloadBtn;
        private System.Windows.Forms.ListBox connectedPlayersListBox;
        private System.Windows.Forms.Button serverStartBtn;
        private System.Windows.Forms.ComboBox gameRulesetCBox;
        private System.Windows.Forms.Label gameRulesetLbl;
        private System.Windows.Forms.Label serverStatusIndicLbl;
        private System.Windows.Forms.Panel serverStatusIndicRect;
        private System.Windows.Forms.Label serverVersionLbl;
        private System.Windows.Forms.Button expansionPackButtons;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.RichTextBox aahAboutDescRTBox;
        private System.Windows.Forms.LinkLabel aahWebLinkLbl;
        private System.Windows.Forms.Button serverSettingsBtn;
	}
}

