using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AppsAgainstHumanity.Server.Crypto;
using CsNetLib2;
using AppsAgainstHumanity.Server.Game;
using System.IO;

namespace AppsAgainstHumanity.Server
{
	public partial class mainForm : Form
	{
        public const byte ETX = 3;

		//private NetLibServer Server;
		//private AAHProtocolWrapper ServerWrapper;
        Game.Game game;

        private Dictionary<int, Deck> _cardDecks;
        private void _fillDeckSelectBox()
        {
            cardDeckCBox.Items.Clear();
            _cardDecks = new Dictionary<int, Deck>();
            string deckPath = "decks";
            int deckCtr = 0;
            foreach (string s in Directory.GetFiles(deckPath, "*.xml"))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(s))
                    {
                        _cardDecks.Add(
                            deckCtr,
                            new Deck(sr.ReadToEnd())
                        );
                    }
                    deckCtr++;
                }
                catch (UnauthorizedAccessException) { }
            }

            for (int i = 0; i < deckCtr; i++)
            {
                this.cardDeckCBox.Items.Add(_cardDecks[i].Name);
            }
            this.cardDeckCBox.SelectedIndex = 0;
        }

        public mainForm()
		{
			InitializeComponent();

            #region Set up form defaults
            this.playerLimitBox.Value = 5;
            this.awesomePointsLimitBox.Value = 8;
            this.timeoutLimitCBox.Value = 30;
            this.czarSelectCBox.SelectedIndex = 0;
            this.gameRulesetCBox.SelectedIndex = 0;
            this.connectedPlayersListBox.Text = String.Empty;
            _fillDeckSelectBox();
            #endregion

            this.serverVersionLbl.Text = Metadata.Version;
		}

        private void _receivedMessageHandler(Player sender, string message)
        {
            if (serverChatRTBox.InvokeRequired)
            {
                Invoke(new Action<Player, string>(_receivedMessageHandler), sender, message);
            }
            else
            {
                this.serverChatRTBox.AppendText(String
                    .Format(
                        "{0}<{1}> {2}",
                        serverChatRTBox.TextLength == 0 ? String.Empty : Environment.NewLine,
                        sender.Nickname,
                        message
                    ));
            }
        }
        private void _playerJoinHandler(Player player)
        {

            if (connectedPlayersListBox.InvokeRequired)
            {
                Invoke(new Action<Player>(_playerJoinHandler), player);
            }
            else
            {
                this.connectedPlayersListBox.Items.Add(player.Nickname);
                this.serverChatRTBox.AppendText(String
                    .Format(
                        "{0}Player {1} joined the game!",
                        serverChatRTBox.TextLength == 0 ? String.Empty : Environment.NewLine,
                        player.Nickname
                    ));
            }
        }
        private void _playerLeaveHandler(Player player)
        {
            if (connectedPlayersListBox.InvokeRequired) Invoke(new Action<Player>(_playerLeaveHandler), player);
            else this.connectedPlayersListBox.Items.Remove(player.Nickname);
        }

        private void broadcastBtn_Click(object sender, EventArgs e)
        {
            if (broadcastTBox.Text.Length > 0)
            {
                game.Broadcast(broadcastTBox.Text);
                this.serverChatRTBox.AppendText(String
                    .Format(
                        "{0}[SERVER] {1}",
                        serverChatRTBox.TextLength == 0 ? String.Empty : Environment.NewLine,
                        broadcastTBox.Text
                    ));
                this.broadcastTBox.Clear();
                this.broadcastTBox.Focus();
            }
            else return;
        }
        private void serverChatRTBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }
        private void deckReloadBtn_Click(object sender, EventArgs e)
        {
            _fillDeckSelectBox();
        }
        private void serverStartBtn_Click(object sender, EventArgs e)
        {
            GameParameters gp = new GameParameters()
            {
                Cards = _cardDecks[cardDeckCBox.SelectedIndex],
                Players = int.Parse(playerLimitBox.Value.ToString()),
                PointsLimit = int.Parse(awesomePointsLimitBox.Value.ToString()),
                TimeoutLimit = int.Parse(timeoutLimitCBox.Value.ToString()),
                Ruleset = (GameRuleset)gameRulesetCBox.SelectedIndex,
                CzarSelection = (CzarSelection)czarSelectCBox.SelectedIndex,
                AllowGambling = allowGamblingCheckBox.Checked
            };

            this.game = new Game.Game(gp);

            game.OnClientMessageReceived += _receivedMessageHandler;
            game.OnPlayerJoin += _playerJoinHandler;
            game.OnPlayerLeave += _playerLeaveHandler;

            #region set form elements to enabled/disabled
            cardDeckCBox.Enabled = false;
            deckReloadBtn.Enabled = false;
            playerLimitBox.Enabled = false;
            awesomePointsLimitBox.Enabled = false;
            timeoutLimitCBox.Enabled = false;
            czarSelectCBox.Enabled = false;
            gameRulesetCBox.Enabled = false;
            allowGamblingCheckBox.Enabled = false;
            allowChatCheckBox.Enabled = false;
            timeoutKickCheckBox.Enabled = false;
            serverStartBtn.Enabled = false;
            gameStartBtn.Enabled = true;
            gameStopBtn.Enabled = true;
            #endregion

            this.serverStatusIndicRect.BackColor = Color.Gold;
            this.serverStatusIndicLbl.ForeColor = Color.OrangeRed;
            this.serverStatusIndicLbl.Text = "Waiting for Players...";
        }
        private void gameStartBtn_Click(object sender, EventArgs e)
        {
            #region enable/disable
            this.gameStartBtn.Enabled = false;
            #endregion

            this.serverStatusIndicRect.BackColor = Color.YellowGreen;
            this.serverStatusIndicLbl.ForeColor = Color.DarkGreen;
            this.serverStatusIndicLbl.Text = "Game has begun.";

            // TODO: Uncomment this.
            game.Start();
        }
        private void gameStopBtn_Click(object sender, EventArgs e)
        {
            if (game != null)
            {
                if (game.HasStarted) game.Stop(stopServer: false);
                game.Stop(stopServer: true);

                this.serverStatusIndicLbl.ForeColor = Color.Red;
                this.serverStatusIndicLbl.Text = "Offline.";
                this.serverStatusIndicRect.BackColor = Color.Red;

                #region set form elements to enabled/disabled
                cardDeckCBox.Enabled = true;
                deckReloadBtn.Enabled = true;
                playerLimitBox.Enabled = true;
                awesomePointsLimitBox.Enabled = true;
                timeoutLimitCBox.Enabled = true;
                // TODO: Uncomment these when implemented.
                //czarSelectCBox.Enabled = true;
                //gameRulesetCBox.Enabled = true;
                //allowGamblingCheckBox.Enabled = true;
                //allowChatCheckBox.Enabled = true;
                //timeoutKickCheckBox.Enabled = true;
                serverStartBtn.Enabled = true;
                gameStartBtn.Enabled = false;
                gameStopBtn.Enabled = false;
                #endregion
            }
        }
	}
}
