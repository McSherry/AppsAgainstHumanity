using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Xml;
using System.Xml.Linq;
using CsNetLib2;

namespace AppsAgainstHumanityClient
{
	public partial class ConnectionForm : Form
	{
		private NetworkInterface NetworkInterface;
		private const int VersionIdentifier = 200;

		public ConnectionForm()
		{
			NetworkInterface = new NetworkInterface();
			NetworkInterface.ClientWrapper.RegisterCommandHandler(CommandType.ACKN, ProcessACKN);
			NetworkInterface.ClientWrapper.RegisterCommandHandler(CommandType.REFU, ProcessREFU);
			NetworkInterface.ClientWrapper.RegisterCommandHandler(CommandType.NDNY, ProcessNDNY);
			NetworkInterface.ClientWrapper.RegisterCommandHandler(CommandType.META, (sender, arguments) =>
			{
				XDocument metaResp = new XDocument(
					new XDeclaration("1.0", "utf-8", "yes"),
					new XElement(
						"meta",
						new XElement(
							"version",
							VersionIdentifier
						)
					)
				);
				if (arguments == null) {
					NetworkInterface.ClientWrapper.SendCommand(CommandType.META, Convert.ToBase64String(Encoding.UTF8.GetBytes(metaResp.ToString())));
				}
			});
			InitializeComponent();
		}

		/// <summary>
		/// Handler for ACKN. When an ACKN is received, close this form and show the main form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="arguments"></param>
		private void ProcessACKN(long sender, string[] arguments)
		{
			ShowMainUI();
		}

		/// <summary>
		/// Thread-safe wrapper for setting the state of the connect button
		/// </summary>
		/// <param name="state"></param>
		private void SetConnectButtonState(bool state)
		{
			if (btn_Connect.InvokeRequired) {
				Invoke(new Action<bool>(SetConnectButtonState), new object[] { state });
			} else {
				btn_Connect.Enabled = state;
			}
		}

		private void ProcessREFU(long sender, string[] arguments)
		{
			MessageBox.Show(arguments[0], "Unable to connect", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			NetworkInterface.Disconnect();
			SetConnectButtonState(true);
		}
		private void ProcessNDNY(long sender, string[] arguments)
		{
			MessageBox.Show(arguments[0], "Server denies nickname", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			NetworkInterface.Disconnect();
			SetConnectButtonState(true);
		}

		private async void btn_Connect_Click(object sender, EventArgs e)
		{
			if (tbx_Nick.Text == string.Empty) {
				MessageBox.Show("Please enter a username", "No username specified", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
			if (tbx_Nick.Text.Length > 20) {
				MessageBox.Show("Username may not be longer than 20 characters", "Invalid username", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
			btn_Connect.Enabled = false;
			try {
				Console.WriteLine("Attempting to connect to the server");
				await NetworkInterface.Connect(tbx_Host.Text, tbx_Nick.Text);
				Console.WriteLine("Connection established");
			} catch (System.Net.Sockets.SocketException ex) {
				MessageBox.Show(ex.Message, "Unable to connect", MessageBoxButtons.OK, MessageBoxIcon.Error);
				btn_Connect.Enabled = true;
			}
		}
		private void ShowMainUI()
		{
			if (base.InvokeRequired) {
				Action act = new Action(ShowMainUI);
				Invoke(act);
				return;
			}
			base.Hide();
			// Make sure the now-hidden connection form won't process any network events anymore
			NetworkInterface.ClientWrapper.UnregisterAllHandlers();
			new MainForm(NetworkInterface, tbx_Nick.Text).Show();
		}
		private void btn_Exit_Click(object sender, EventArgs e)
		{
			NetworkInterface.Disconnect();
			base.Close();
		}
	}
}
