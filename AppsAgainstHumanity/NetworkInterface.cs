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
		/// <summary>
		/// Apps Against Humanity runs on port 11235
		/// </summary>
		public const int DefaultPort = 11235;
		/// <summary>
		/// The low-level TCP client
		/// </summary>
		public NetLibClient Client { get; private set; }
		/// <summary>
		/// The high-level protocol wrapper
		/// </summary>
		internal AAHProtocolWrapper ClientWrapper { get; private set; }
		/// <summary>
		/// ASCII ETX char
		/// </summary>
		private const byte ETX = 3;

		internal NetworkInterface()
		{
			Client = new NetLibClient();
			ClientWrapper = new AAHProtocolWrapper(Client);
		}

		/// <summary>
		/// Connects to an AAH server, and sends a JOIN command once a connection has been established.
		/// </summary>
		/// <param name="host">The host to connect to.</param>
		/// <param name="nick">The nickname that should be used.</param>
		internal async Task Connect(string host, int port, string nick)
		{
            if (port > Int16.MaxValue - 1 || port < 1024) port = DefaultPort;

			Task t = Client.Connect(host, port, TransferProtocols.Delimited, Encoding.UTF8);
			Client.Delimiter = ETX;
			await t;
			ClientWrapper.SendCommand(CommandType.JOIN, nick);
		}

		internal void Disconnect()
		{
			Client.Disconnect();
		}
	}
}
