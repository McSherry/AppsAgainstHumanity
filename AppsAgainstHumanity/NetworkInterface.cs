using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSNetLib;

namespace AppsAgainstHumanityClient
{
	delegate void CommandHandler(string[] arguments);

	public class NetworkInterface
	{
		private NetClient Client;
		private const int Port = 11235;
		private const char Nul = (char)((byte)0);
		private CommandProcessor CmdProcessor;

		private Dictionary<string, CommandHandler> Commands;

		public NetworkInterface(MainForm frm)
		{
			Client = new NetClient();
			Client.OnNetworkDataAvailabe += HandleIncomingData;
			CmdProcessor = new CommandProcessor(frm);
			Commands = new Dictionary<string, CommandHandler>() {
				{ "ACKN", CmdProcessor.ProcessACKN },
				{ "REFU", CmdProcessor.ProcessREFU },
			};
		}

		private void HandleIncomingData(string data)
		{
			var command = data.Substring(0, 4);
			string[] args = null;
			if (data.Length > 4) {
				var arglist = data.Substring(4);
				args = arglist.Split(new char[] { Nul });
			}
			Commands[command](args);
		}

		private void SendData(Commands cmd, string[] args = null)
		{
			StringBuilder messageBuilder = new StringBuilder();
			messageBuilder.Append(cmd.ToString());
			
			// TODO: Format and send command string

			//Client.SendData(data);
		}

		public void Connect(string host, string nick)
		{
			Client.Connect(host, 11235);
			// TODO: Send join command
		}
	}
}
