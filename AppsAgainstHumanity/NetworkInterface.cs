﻿using System;
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
		private const int Port = 11235;
		/// <summary>
		/// The low-level TCP client
		/// </summary>
		private NetLibClient Client;
		/// <summary>
		/// The high-level protocol wrapper
		/// </summary>
		private AAHProtocolWrapper ClientWrapper;
		/// <summary>
		/// ASCII ETX char
		/// </summary>
		private const byte ETX = 3;


		internal NetworkInterface()
		{
			Client = new NetLibClient();
			ClientWrapper = new AAHProtocolWrapper(Client);

            ClientWrapper.RegisterCommandHandler(CommandType.NDNY, (s, e) =>
            {
                System.Windows.Forms.MessageBox.Show(e[0]);
            });
		}

		internal void RegisterCommandHandler(CommandType type, CommandHandler handler)
		{
			ClientWrapper.RegisterCommandHandler(type, handler);
		}

		/// <summary>
		/// Connects to an AAH server, and sends a JOIN command once a connection has been established.
		/// </summary>
		/// <param name="host">The host to connect to.</param>
		/// <param name="nick">The nickname that should be used.</param>
		internal async Task Connect(string host, string nick)
		{
			try {
				Task t = Client.Connect(host, 11235, TransferProtocol.Delimited);
				Client.Delimiter = ETX;
				await t;
			} catch (Exception e) {
                System.Windows.Forms.MessageBox.Show(e.Message);
				return;
			}
			ClientWrapper.SendCommand(CommandType.JOIN, nick);
		}

		internal void Disconnect()
		{
			Client.Disconnect();
		}
	}
}
