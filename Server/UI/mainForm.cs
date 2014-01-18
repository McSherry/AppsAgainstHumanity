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

namespace AppsAgainstHumanity.Server.UI
{
	public partial class mainForm : Form
	{
        private enum _GameMonitorState
        {
            Online,
            Waiting,
            Offline
        }

        public const byte ETX = 3;

		//private NetLibServer Server;
		//private AAHProtocolWrapper ServerWrapper;
        Game.Game game;

        internal List<Deck> _expansionDecks = new List<Deck>();
        private Dictionary<int, Deck> _cardDecks;
        private void _loadEmbeddedDeck(int ctr = 0)
        {
            Console.WriteLine("ERROR LOADING DECKS: LOADING EMBEDDED COPY");
            _cardDecks.Add(ctr, new Deck(Server.Properties.Resources.US));
        }
        private void _fillDeckSelectBox()
        {
            cardDeckCBox.Items.Clear();
            _cardDecks = new Dictionary<int, Deck>();
            int deckCtr = 0;

            try
            {
                foreach (string s in Directory.GetFiles(Settings.DeckPath, "*.xml"))
                {
                    using (StreamReader sr = new StreamReader(s))
                    {
                        try
                        {
                            _cardDecks.Add(
                                deckCtr,
                                new Deck(sr.ReadToEnd())
                            );
                        }
                        catch (System.Xml.XmlException)
                        {
                            Console.WriteLine("ERROR PARSING: {0}", s);
                            continue;
                        }
                    }
                    deckCtr++;
                }
            }
            catch (DirectoryNotFoundException)
            {
                // If the user's configured the server to look for decks in a directory that doesn't exist,
                // we'll load our copy of the US main deck that's embedded inside the application.
                _loadEmbeddedDeck(deckCtr);
                deckCtr++;
            }
            catch (ArgumentException)
            {
                // Here the user's given us an invalid path. Once again, we're loading the embedded copy.
                _loadEmbeddedDeck(deckCtr);
                deckCtr++;
            }
            catch (UnauthorizedAccessException)
            {
                // User probably tried to get us to load stuff from a protected directory.
                _loadEmbeddedDeck(deckCtr);
                deckCtr++;
            }
            finally
            {
                // Similarly, if the user has got us to look in a directory where there aren't any
                // decks, we'll load the embedded US version.
                if (deckCtr == 0) _loadEmbeddedDeck();
            }

            // Remove from the card packs dictionary all items which are not addons.
            foreach (KeyValuePair<int, Deck> kvp in _cardDecks.ToList().Where(cp => cp.Value.Type != PackType.Pack))
            {
                _cardDecks.Remove(kvp.Key);
                deckCtr--;
            }

            for (int i = 0; i < deckCtr; i++)
            {
                this.cardDeckCBox.Items.Add(_cardDecks.ElementAt(i).Value.Name);
            }
            this.cardDeckCBox.SelectedIndex = 0;
        }
        private void _enableUIComponents()
        {
            if (this.InvokeRequired) this.Invoke(new Action(_enableUIComponents));
            else
            {
                #region set form elements to enabled/disabled
                cardDeckCBox.Enabled = true;
                deckReloadBtn.Enabled = true;
                playerLimitBox.Enabled = true;
                awesomePointsLimitBox.Enabled = true;
                timeoutLimitCBox.Enabled = true;
                // TODO: Uncomment these when implemented.
                czarSelectCBox.Enabled = true;
                //gameRulesetCBox.Enabled = true;
                //allowGamblingCheckBox.Enabled = true;
                allowChatCheckBox.Enabled = true;
                timeoutKickCheckBox.Enabled = true;
                onePickBlacksCBox.Enabled = true;
                expansionPackButtons.Enabled = true;
                serverStartBtn.Enabled = true;
                gameStartBtn.Enabled = false;
                gameStopBtn.Enabled = false;
                #endregion
            }
        }
        private void _disableUIComponents()
        {
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
            onePickBlacksCBox.Enabled = false;
            serverStartBtn.Enabled = false;
            gameStartBtn.Enabled = true;
            gameStopBtn.Enabled = true;
            expansionPackButtons.Enabled = false;
        }
        private void _setGameMonitorState(_GameMonitorState gms)
        {
            Color FC = Color.Empty, BC = FC;
            string text = String.Empty;

            switch (gms)
            {
                case _GameMonitorState.Offline:
                    FC = Color.Red;
                    BC = Color.Red;
                    text = "Offline.";
                    break;
                case _GameMonitorState.Online:
                    BC = Color.YellowGreen;
                    FC = Color.DarkGreen;
                    text = "Game has begun.";
                    break;
                case _GameMonitorState.Waiting:
                    FC = Color.OrangeRed;
                    BC = Color.Gold;
                    text = "Waiting for players...";
                    break;
            }

            if (this.serverStatusIndicLbl.InvokeRequired)
                this.serverStatusIndicLbl.Invoke(new Action<_GameMonitorState>(_setGameMonitorState), gms);
            else
            {
                this.serverStatusIndicLbl.ForeColor = FC;
                this.serverStatusIndicLbl.Text = text;
            }

            if (this.serverStatusIndicRect.InvokeRequired)
                this.serverStatusIndicRect.Invoke(new Action<_GameMonitorState>(_setGameMonitorState), gms);
            else this.serverStatusIndicRect.BackColor = BC;
        }

        public mainForm()
		{
			InitializeComponent();

            #region Set up form defaults
            this.playerLimitBox.Value = 5;
            this.awesomePointsLimitBox.Value = 8;
            this.timeoutLimitCBox.Value = 30;
            this.czarSelectCBox.SelectedIndex = 0;
            Game.Game.ModeController gmc = Game.Game.GameModes;
            this.gameRulesetCBox.Items.AddRange(gmc._gameModeKeys.ToArray());
            this.gameRulesetCBox.SelectedIndex = 0;
            this.connectedPlayersListBox.Text = String.Empty;
            this.aahAboutDescRTBox.SelectAll();
            this.aahAboutDescRTBox.SelectionAlignment = HorizontalAlignment.Center;
            this.serverVersionLbl.Text = String
                .Format(
                    "{0}.{1}.{2} ({3})",
                    Metadata.MajorVersion,
                    Metadata.MinorVersion,
                    Metadata.PatchVersion,
                    Metadata.VersionIdentifier
                );
            _fillDeckSelectBox();
            #endregion

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
            else
            {
                this.connectedPlayersListBox.Items.Remove(player.Nickname);
                this.serverChatRTBox.AppendText(String
                    .Format(
                        "\nPlayer {0} has disconnected.",
                        player.Nickname
                    )
                );
            }
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
            Deck aggregateDeck = _cardDecks.ElementAt(cardDeckCBox.SelectedIndex).Value.Clone();
            foreach (Deck d in _expansionDecks) aggregateDeck.ExtendDeck(d);

            GameParameters gp = new GameParameters()
            {
                Cards = aggregateDeck,
                Players = int.Parse(playerLimitBox.Value.ToString()),
                PointsLimit = int.Parse(awesomePointsLimitBox.Value.ToString()),
                TimeoutLimit = int.Parse(timeoutLimitCBox.Value.ToString()),
                Ruleset = gameRulesetCBox.SelectedItem.ToString(),
                CzarSelection = (CzarSelection)czarSelectCBox.SelectedIndex,
                AllowGambling = allowGamblingCheckBox.Checked,
                KickOnTimeout = timeoutKickCheckBox.Checked,
                AllowPlayerChat = allowChatCheckBox.Checked,
                BlackCardPickLimit = onePickBlacksCBox.Checked
            };

            this.game = new Game.Game(gp);

            game.OnClientMessageReceived += _receivedMessageHandler;
            game.OnPlayerJoin += _playerJoinHandler;
            game.OnPlayerLeave += _playerLeaveHandler;
            game.OnPlayerDisconnected += _playerLeaveHandler;
            game.OnGameStopped += gameStop_handler;

            _disableUIComponents();

            _setGameMonitorState(_GameMonitorState.Waiting);
        }
        private void gameStartBtn_Click(object sender, EventArgs e)
        {
            // Check that we have the required amount of players to begin a
            // game.
            if (game.Players.Count >= Constants.MinimumPlayers)
            {
                #region enable/disable
                this.gameStartBtn.Enabled = false;
                #endregion

                _setGameMonitorState(_GameMonitorState.Online);

                // TODO: Uncomment this.
                game.Start();
            }
            else
            {
                // If we don't show the server administrator an error message.
                MessageBox.Show(
                    this,
                    String.Format(
                        "There must be at least {0} players connected before a game can begin.",
                        Constants.MinimumPlayers
                    ),
                    "Insufficient players.",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation
                );
            }
        }
        private void gameStopBtn_Click(object sender, EventArgs e)
        {
            if (game != null)
            {
                game.Stop();
            }
        }
        private void gameStop_handler(object sender, EventArgs e)
        {
            SuspendLayout();
            _setGameMonitorState(_GameMonitorState.Offline);
            _enableUIComponents();
            ResumeLayout();
        }

        private void expansionPackButtons_Click(object sender, EventArgs e)
        {
            expansionPackForm epf = new expansionPackForm();
            epf.ShowDialog(this);
        }

        private void aahWebLinkLbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(Constants.WebAddress.ToString());
        }

        private void serverSettingsBtn_Click(object sender, EventArgs e)
        {
            var sf = new settingsForm();

            sf.ShowDialog(this);

            sf.Dispose();
        }
	}
}
