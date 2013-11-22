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

	public class NetLibClient
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
		public void Connect(string hostname, int port, TransferProtocol protocol)
		{
			Protocol = new TransferProtocolFactory().CreateTransferProtocol(protocol);
			Protocol.AddEventCallbacks(OnDataAvailable, OnBytesAvailable);
			client.BeginConnect(hostname, port, ConnectCallback, null);
		}
		public void ConnectCallback(IAsyncResult ar)
		{
			client.EndConnect(ar);
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
