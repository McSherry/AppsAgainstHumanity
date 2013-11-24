using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CsNetLib2;

namespace AppsAgainstHumanityClient
{
	public class NetworkInterface
	{
		private const int Port = 11235;
		private NetLibClient Client;
		private AAHProtocolWrapper ClientWrapper;
		/// <summary>
		/// ASCII ETX char
		/// </summary>
		private const byte ETX = 3;

		internal NetworkInterface()
		{
			Client = new NetLibClient();
			ClientWrapper = new AAHProtocolWrapper(Client);
			ClientWrapper.RegisterCommandHandler(CommandType.ACKN, (sender, arguments) =>
			{

			});
		}

		internal async void Connect(string host, string nick)
		{
			try {
				Task t = Client.Connect(host, 11235, TransferProtocol.Delimited);
				Client.Delimiter = ETX;
				await t;
			} catch (Exception e) {
				System.Diagnostics.Debugger.Break();
			}
			ClientWrapper.SendCommand(CommandType.JOIN, nick);
		}

		internal void Disconnect()
		{
			Client.Disconnect();
		}
	}
}
