using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Drawing.Text;
using CsNetLib2;
using System.Xml;
using System.Xml.Linq;

namespace AppsAgainstHumanityClient
{
	public partial class MainForm : Form
	{
		private NetworkInterface NetworkInterface;
		private Game Game = new Game();
		private PrivateFontCollection PrivateFont = new PrivateFontCollection();
		

		public MainForm(NetworkInterface networkInterface, string yourName)
		{
			Game.YourName = yourName;

			NetworkInterface = networkInterface;
			NetworkInterface.ClientWrapper.RegisterCommandHandler(CommandType.CLNF, (sender, arguments) =>
			{
				Game.AddPlayers(arguments);
				UpdatePlayerList();
			});
			NetworkInterface.ClientWrapper.RegisterCommandHandler(CommandType.RMSG, (sender, arguments) =>
			{
				AddChatLine(string.Format("<{0}>  {1}", arguments[0], arguments[1]));
			});
			NetworkInterface.ClientWrapper.RegisterCommandHandler(CommandType.BDCS, (sender, arguments) =>
			{
				AddChatLine(string.Format("<SERVER>  {0}", arguments[0]));
			});
			NetworkInterface.ClientWrapper.RegisterCommandHandler(CommandType.CLEX, (sender, arguments) =>
			{
				Game.RemovePlayer(arguments[0]);
				UpdatePlayerList();
				AddChatLine("<NOTICE> " + arguments[0] + " has left the game.");
			});
			NetworkInterface.ClientWrapper.RegisterCommandHandler(CommandType.GSTR, (sender, arguments) =>
			{
				SetGameStatusLabel("The game has started");
			});
			NetworkInterface.ClientWrapper.RegisterCommandHandler(CommandType.BLCK, (sender, arguments) =>
			{
				SetBlackCard(arguments[0], int.Parse(arguments[1]));
			});
			NetworkInterface.ClientWrapper.RegisterCommandHandler(CommandType.WHTE, (sender, arguments) =>
			{
				AddCard(crl_OwnedCards, arguments[0], arguments[1]); // Add a white card to the player's deck
			});
			NetworkInterface.ClientWrapper.RegisterCommandHandler(CommandType.RSTR, (sender, arguments) =>
			{
				crl_PickedCards.Clear();
			});
			NetworkInterface.ClientWrapper.RegisterCommandHandler(CommandType.CLJN, (sender, arguments) =>
			{
				Game.Players.Add(new Player(arguments[0])); // Add a new player to the Game
				UpdatePlayerList(); // Make sure the new player shows up in the player list
				AddChatLine("<NOTICE> " + arguments[0] + " has joined the game.");
			});
			NetworkInterface.ClientWrapper.RegisterCommandHandler(CommandType.BLNK, (sender, arguments) =>
			{
				AddCard(crl_PickedCards, "", ""); // Add a blank (upside down) card to indicate a card has been picked
			});
			NetworkInterface.ClientWrapper.RegisterCommandHandler(CommandType.REVL, (sender, arguments) =>
			{
				crl_PickedCards.RemoveBlanks();
				//ClearPickedCardList(); // Remove all blank cards from the picked card list
				for(int i = 0; i < arguments.Length; i++){
					if (!crl_PickedCards.HasCardWithId(arguments[i])) {
						AddCard(crl_PickedCards, arguments[i], arguments[++i]); // Add all revealed cards to the picked card list
					} else {
						i++;
					}
					SetGameStatusLabel("Please wait for the Card Czar to make their decision.");
				}
				if (Game.CardCzar.Name == Game.YourName) {
					SetSelectability(crl_PickedCards, true); // Allow the Card Czar to select from the picked card list
					crl_PickedCards.GroupNumber = arguments.Length / 2; // Set the group number for the picked card list so cards are selected in groups
					crl_PickedCards.MaxSelectNum = 1; // Allow the Card Czar to only select one group at a time
					SetGameStatusLabel("All players have submitted their choice, pick the one you like best.");
					SetGameActionButton(true);
				}
			});
			NetworkInterface.ClientWrapper.RegisterCommandHandler(CommandType.CRTO, (sender, arguments) =>
			{
				SetGameActionButton(false);
			});
			NetworkInterface.ClientWrapper.RegisterCommandHandler(CommandType.CZTO, (sender, arguments) =>
			{
				SetGameActionButton(false);
			});
			NetworkInterface.ClientWrapper.RegisterCommandHandler(CommandType.CZAR, (sender, arguments) =>
			{
				AddChatLine("<NOTICE> " + arguments[0] + " is the Card Czar.");
				if (arguments[0] == Game.YourName) {
					SetGameStatusLabel("You are the Card Czar! Wait for the other players to submit their cards, then pick the one you like best.");
					SetSelectability(crl_OwnedCards, false); // Prevent the Card Czar from being able to pick from his deck
					SetGameActionButton(false); // Prevent the Card Czar from picking a card before the white cards have been revealed
				} else {
					SetGameStatusLabel("The round has started. Pick the card you think fits best with the current Black Card and click 'submit selected'");
					SetSelectability(crl_OwnedCards, true); // Make sure everybody who isn't the Card Czar can select their cards
					SetGameActionButton(true); // Allow everybody who isn't the Card Czar to use the action button
				}
				Game.CardCzar = Game.GetPlayer(arguments[0]);
			});
			NetworkInterface.ClientWrapper.RegisterCommandHandler(CommandType.PICK, (sender, arguments) =>
			{
				AddCard(crl_PickedCards, arguments[0], "");
			});
			NetworkInterface.ClientWrapper.RegisterCommandHandler(CommandType.RWIN, (sender, arguments) =>
			{
				if (arguments.Length == 0) {
					AddChatLine("<NOTICE> The Card Czar has timed out, no Awesome Points have been awarded. Your card(s) will be returned when the next round begins.");
					SetGameStatusLabel("Please wait for the next round to begin.");
				}
				Game.Players.Where(p => p.Name == arguments[0]).First().AwesomePoints++;
				UpdatePlayerList();
				AddChatLine("<NOTICE> " + arguments[0] + " wins the round!");
				SetGameStatusLabel(arguments[0] + " has won the round. Please wait for the next round to begin.");
			});
			NetworkInterface.ClientWrapper.RegisterCommandHandler(CommandType.GWIN, (sender, arguments) =>
			{
				SetGameActionButton(false);
				AddChatLine("<NOTICE> " + arguments[0] + " wins the game!");
				SetGameStatusLabel("The game has ended.");
			});
			NetworkInterface.ClientWrapper.RegisterCommandHandler(CommandType.META, (sender, arguments) =>
			{
				var xmlText = Encoding.UTF8.GetString(Convert.FromBase64String(arguments[0]));
				XmlDocument xmd = new XmlDocument();
				xmd.LoadXml(xmlText);
				var playermax = xmd.GetElementsByTagName("playermax");
				var aplimit = xmd.GetElementsByTagName("aplimit");
				var timeout = xmd.GetElementsByTagName("timeout");
				if ((playermax.Count | aplimit.Count | timeout.Count) > 1) {
					MessageBox.Show("Error", "Invalid metadata returned by server. This error shouldn't affect your ability to play the game, but please report the error to the developers.", MessageBoxButtons.OK, MessageBoxIcon.Error);
				} else {
					SetMetadata(playermax.Item(0).InnerText, aplimit.Item(0).InnerText, timeout.Item(0).InnerText);
				}
			});

			// Check for any commands that got queued up while the connection form was being displayed
			while (NetworkInterface.ClientWrapper.HasQueuedCommand) {
				var cmd = NetworkInterface.ClientWrapper.GetNextQueuedCommand();
				if (cmd.Type == CommandType.CLNF) {
					Game.AddPlayers(cmd.Arguments);
					UpdatePlayerList();
				} else {
					throw new InvalidOperationException("Command type " + cmd.Type.ToString() + " was unexpected at this time.");
				}
			}
			// TODO: Send this once the server implements it
			networkInterface.ClientWrapper.SendCommand(CommandType.META);

			InitializeComponent();
		}

		private void SetMetadata(string playermax, string aplimit, string timeout)
		{
			if (lbl_PlayerLimit.InvokeRequired || lbl_PlayingTo.InvokeRequired || lbl_PickTimeout.InvokeRequired) {
				Invoke(new Action<string, string, string>(SetMetadata), playermax, aplimit, timeout);
			} else {
				lbl_PlayerLimit.Text = playermax;
				lbl_PlayingTo.Text = aplimit;
				lbl_PickTimeout.Text = timeout;
			}
		}

		private void SetGameActionButton(bool enabled)
		{
			if (btn_GameAction.InvokeRequired) {
				Invoke(new Action<bool>(SetGameActionButton), enabled);
			} else {
				btn_GameAction.Enabled = enabled;
			}
		}
		private void SetSelectability(CardList cl, bool selectable)
		{
			if (cl.InvokeRequired) {
				Invoke(new Action<CardList, bool>(SetSelectability), cl, selectable);
			} else {
				cl.CanSelectCards = selectable;
			}
		}
		private void AddCard(CardList cl, string id, string text)
		{
			if (cl.InvokeRequired) {
				Invoke(new Action<CardList, string, string>(AddCard), cl, id, text);
			} else {
				cl.AddCard(new Card(text, id));
			}
		}
		private void SetBlackCard(string text, int pickNum)
		{
			if (crd_BlackCard.InvokeRequired) {
				Invoke(new Action<string, int>(SetBlackCard), text, pickNum);
			} else {
				crd_BlackCard.CardText = text;
				crd_BlackCard.PickNum = pickNum;
				crl_OwnedCards.MaxSelectNum = crd_BlackCard.PickNum;
			}
		}

		private void SetGameStatusLabel(string text)
		{
			if (sts_GameStatus.InvokeRequired) {
				Invoke(new Action<string>(SetGameStatusLabel), text);
			} else {
				stl_GameStatusLabel.Text = text;
			}
		}
		protected override void OnClosing(CancelEventArgs e)
		{
			// TODO: Send disconnect request to server to attempt a clean disconnect
			NetworkInterface.ClientWrapper.SendCommand(CommandType.DISC);
			bool received = false;
			NetworkInterface.ClientWrapper.RegisterCommandHandler(CommandType.DISC, (sender, arguments) =>
			{
				received = true;
			});
			int waitTime = 0;
			while (!received && waitTime < 1000) { // Wait for the server to confirm a DISC, but don't wait longer than one second.
				System.Threading.Thread.Sleep(20);
				waitTime += 20;
			}
			base.OnClosing(e);
		}


		/// <summary>
		/// Thread-safe method that will synchronize the players listbox with the player list in the Game class.
		/// </summary>
		private void UpdatePlayerList()
		{
			if (lb_Players.InvokeRequired) {
				Invoke(new Action(UpdatePlayerList));
			} else {
				lb_Players.Items.Clear();
				foreach (Player p in Game.Players) {
					lb_Players.Items.Add(p);
				}
			}
		}

		/// <summary>
		/// Thread-safe method that will add a chat line to the chat box
		/// </summary>
		/// <param name="line"></param>
		private void AddChatLine(string line)
		{
			if (tbx_ChatLog.InvokeRequired) {
				Invoke(new Action<string>(AddChatLine), line);
			} else {
				// You might be wondering "Why the hell do you prepend \r\n to a string? Why not append it? 
				// Why are you using a ternary operator to check if the textbox is empty? Why is NASA a ghost of its former self?"
				// Do not worry. Soon, everything will be clear - well, everything except the NASA part.
				// You see, normally one would append \r\n to a string to make sure the next string is put at the next line.
				// In our case, we cannot do this, as the chat box would scroll to the last, empty, line. Because of this,
				// \r\n is put before the text instead, but this will create another issue: upon adding the first line of text to the
				// textbox, the \r\n prefix will first create an empty line, then put the text to be added on the next line.
				// To prevent this from happening, we check if the textbox is empty: if it is, we don't have to prepend \r\n.
				// Otherwise, we do. There, that wasn't so difficult, was it now?
				string prefix = tbx_ChatLog.Text == string.Empty ? "" : "\r\n";
				tbx_ChatLog.AppendText(prefix + line);
			}
		}

		/// <summary>
		/// Yep. It does exactly what you think it does.
		/// </summary>
		/// <param name="message"></param>
		private void SendChatMessage(string message)
		{
			NetworkInterface.ClientWrapper.SendCommand(CommandType.SMSG, message);
		}

		/// <summary>
		/// This makes sure that a chat line will be sent when the user presses Enter.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void tbx_Chat_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter && tbx_Chat.Text != string.Empty) {
				SendChatMessage(tbx_Chat.Text);
				tbx_Chat.Text = string.Empty;
			}
		}

		/// <summary>
		/// This makes sure that the user cannot enter more than 200 characters into the chat box.
		/// If they do, the keypress event will be dropped, and an OS beep will be played.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void tbx_Chat_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (tbx_Chat.Text.Length == 200 && e.KeyChar != 8) {
				e.Handled = true;
				System.Media.SystemSounds.Beep.Play();
			}
		}

		private void btn_GameAction_Click(object sender, EventArgs e)
		{
			if(crl_OwnedCards.SelectedCards.Count != 0){
				if (crl_OwnedCards.SelectedCards.Count == crl_OwnedCards.MaxSelectNum) {
					while (crl_OwnedCards.SelectedCards.Count != 0) {
						var card = crl_OwnedCards.SelectedCards[0];
						NetworkInterface.ClientWrapper.SendCommand(CommandType.PICK, card.Id);
						AddCard(crl_PickedCards, card.Id, card.CardText);
						crl_OwnedCards.RemoveCard(card);
					}
					SetGameActionButton(false);
					SetGameStatusLabel("Please wait for the other players to submit their picks.");
				} else {
					MessageBox.Show(string.Format("Please pick {0} cards.", crl_PickedCards.MaxSelectNum), "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			} else if (Game.CardCzar.Name == Game.YourName && crl_PickedCards.SelectedCards.Count != 0) {
				NetworkInterface.ClientWrapper.SendCommand(CommandType.CZPK, crl_PickedCards.SelectedCards.First().Id);
				SetGameActionButton(false);
			} else {

			}
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			Stream fontStream = this.GetType().Assembly.GetManifestResourceStream("OpenSans-Bold.ttf");

			byte[] fontdata = new byte[fontStream.Length];

			fontStream.Read(fontdata, 0, (int)fontStream.Length);
			fontStream.Close();

			unsafe {
				fixed (byte* pFontData = fontdata) {
					PrivateFont.AddMemoryFont((System.IntPtr)pFontData, fontdata.Length);
				}
			}
		}
	}
}
