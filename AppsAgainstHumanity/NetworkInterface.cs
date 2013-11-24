using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CsNetLib2;

namespace AppsAgainstHumanityClient
{
	delegate void CommandHandler(string[] arguments);

	public class NetworkInterface
	{
		private NetLibClient Client;
		private const int Port = 11235;
		private const char NUL = (char)0;
		private const byte ETX = 3;
		internal CommandProcessor CommandProcessor { get; private set; }

		private Dictionary<string, CommandHandler> Commands;

		internal NetworkInterface()
		{
			Client = new NetLibClient();
			Client.OnDataAvailable += HandleIncomingData;
			CommandProcessor = new CommandProcessor();
			Commands = new Dictionary<string, CommandHandler>() {
				{ "ACKN", CommandProcessor.ProcessACKN },
				{ "REFU", CommandProcessor.ProcessREFU },
			};
		}

		private void HandleIncomingData(string data, long clientId)
		{
			var command = data.Substring(0, 4);
			string[] args = null;
			if (data.Length > 4) {
				var arglist = data.Substring(4);
				args = arglist.Split(new char[] { NUL });
			}
			Commands[command](args);
		}

		private void SendData(CommandType cmd, string argument)
		{
			SendData(cmd, new string[] { argument });
		}

		private void SendData(CommandType cmd, string[] args = null)
		{
			StringBuilder messageBuilder = new StringBuilder();
			messageBuilder.Append(cmd.ToString());

			messageBuilder.Append(string.Join(NUL.ToString(), args));

			string data = messageBuilder.ToString();

			Client.Send(data);
		}

		internal async void Connect(string host, string nick)
		{
			Task t = Client.Connect(host, 11235, TransferProtocol.Streaming);
			Client.Delimiter = ETX;
			await t;
			SendData(CommandType.JOIN, nick);
		}
	}
}
