using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CsNetLib2;

namespace AppsAgainstHumanityClient
{
	public partial class MainForm : Form
	{
		private NetworkInterface NetworkInterface;
		private Game Game = new Game();

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
				AddCard(crl_OwnedCards, arguments[0], arguments[1]);
			});
			NetworkInterface.ClientWrapper.RegisterCommandHandler(CommandType.RSTR, (sender, arguments) =>
			{
				// TODO: Handle RSTR
			});
			NetworkInterface.ClientWrapper.RegisterCommandHandler(CommandType.CZAR, (sender, arguments) =>
			{
				if (arguments[0] == Game.YourName) {
					SetGameStatusLabel("You are the Card Czar! Wait for the other players to submit their cards, then pick the one you like best.");
					SetSelectability(crl_OwnedCards, false);
				}
				Game.CardCzar = Game.GetPlayer(arguments[0]);
			});
			NetworkInterface.ClientWrapper.RegisterCommandHandler(CommandType.PICK, (sender, arguments) =>
			{
				AddCard(crl_PickedCards, arguments[0], "");
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

			InitializeComponent();
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
			//NetworkInterface.ClientWrapper.SendCommand(CommandTyp
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
			if(crl_PickedCards.SelectedCards.Count != 0){
				if (crl_PickedCards.SelectedCards.Count == crl_PickedCards.MaxSelectNum) {
					foreach (var card in crl_PickedCards.SelectedCards) {
						NetworkInterface.ClientWrapper.SendCommand(CommandType.PICK, card.Id);
					}
				} else {
					MessageBox.Show(string.Format("Please pick {0} cards.", crl_PickedCards.MaxSelectNum), "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			} else if (Game.CardCzar.Name == Game.YourName) {

			} else {

			}
		}
	}
}
