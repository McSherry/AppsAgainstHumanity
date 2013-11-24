using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;

namespace CsNetLib2
{
	public delegate void ClientDataAvailable(string data);

	public class NetLibClient : ITransmittable
	{
		private TcpClient client;
		private byte[] buffer;
		private ITransferProtocol Protocol;

		public event DataAvailabe OnDataAvailable;
		public event BytesAvailable OnBytesAvailable;

		public NetLibClient()
		{
			client = new TcpClient();
			client.LingerState.Enabled = true;
		}
		public void SendBytes(byte[] buffer)
		{
			buffer = Protocol.FormatData(buffer);
			client.GetStream().BeginWrite(buffer, 0, buffer.Length, SendCallback, null);
		}
		public void Send(string data, long clientId)
		{
			Send(data);
		}
		public void Send(string data)
		{
			byte[] buffer = Encoding.ASCII.GetBytes(data);
			SendBytes(buffer);
		}
		public void Disconnect()
		{
			client.Close();
		}
		public void SendCallback(IAsyncResult ar)
		{
			try {
				client.GetStream().EndWrite(ar);
			} catch (ObjectDisposedException) {
				Console.WriteLine("Socket closed.");
			}
		}

		public byte Delimiter
		{
			get
			{
				try {
					var protocol = (DelimitedProtocol)Protocol;
					return protocol.Delimiter;
				} catch (InvalidCastException) {
					throw new InvalidOperationException("Unable to set the delimiter: Protocol is not of type DelimitedProtocol");
				}
			}
			set
			{
				try {
					var protocol = (DelimitedProtocol)Protocol;
					protocol.Delimiter = value;
				} catch (InvalidCastException) {
					throw new InvalidOperationException("Unable to set the delimiter: Protocol is not of type DelimitedProtocol");
				}
			}
		}

		public void ConnectBlocking(string hostname, int port, TransferProtocol protocol)
		{
			Protocol = new TransferProtocolFactory().CreateTransferProtocol(protocol);
			Protocol.AddEventCallbacks(OnDataAvailable, OnBytesAvailable);
			client.Connect(hostname, port);
		}

		public async Task Connect(string hostname, int port, TransferProtocol protocol)
		{
			Protocol = new TransferProtocolFactory().CreateTransferProtocol(protocol);
			Protocol.AddEventCallbacks(OnDataAvailable, OnBytesAvailable);
			Task t = client.ConnectAsync(hostname, port);
			await t;
			NetworkStream stream = client.GetStream();
			buffer = new byte[client.ReceiveBufferSize];
			stream.BeginRead(buffer, 0, buffer.Length, ReadCallback, client);
		}
		private void ReadCallback(IAsyncResult result)
		{
			NetworkStream networkStream = null;
			try {
				networkStream = client.GetStream();
			} catch (ObjectDisposedException) {
				Console.WriteLine("Socket closed.");
				return;
			}
			var read = networkStream.EndRead(result);
			if (read == 0) {
				client.Close();
			}

			Protocol.ProcessData(buffer, 0);

			networkStream.BeginRead(buffer, 0, buffer.Length, ReadCallback, client);
		}
		public void AwaitConnect()
		{
			while (!client.Connected) {
				System.Threading.Thread.Sleep(5);
			}
		}
	}
}
