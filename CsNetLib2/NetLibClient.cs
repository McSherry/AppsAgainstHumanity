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
	public delegate void Disconnected();
	public delegate void LogEvent(string data);

	public class NetLibClient : ITransmittable
	{
		private TcpClient client;
		private byte[] buffer;
		private TransferProtocol Protocol;

		public event DataAvailabe OnDataAvailable;
		public event BytesAvailable OnBytesAvailable;
		public event Disconnected OnDisconnect;
		public event LogEvent OnLogEvent;

		public bool Connected { get { return client.Connected; } }
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

		private void Log(string message)
		{
			if (OnLogEvent != null) {
				OnLogEvent(message);
			}
		}

		private void ProcessDisconnect()
		{
			if (OnDisconnect != null) {
				OnDisconnect();
			}
		}
		public bool SendBytes(byte[] buffer)
		{
			buffer = Protocol.FormatData(buffer);
            try
            {
                client.GetStream().BeginWrite(buffer, 0, buffer.Length, SendCallback, null);
                return true;
            }
            catch (NullReferenceException nrex)
            {
                return false;
            }
		}
		public bool Send(string data, long clientId)
		{
			return Send(data);
		}
		public bool Send(string data)
		{
			byte[] buffer = Protocol.EncodingType.GetBytes(data);
			return SendBytes(buffer);
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
				ProcessDisconnect();
			}
		}
		public async Task Connect(string hostname, int port, TransferProtocols protocol, Encoding encoding)
		{
			Protocol = new TransferProtocolFactory().CreateTransferProtocol(protocol, encoding, new Action<string>(Log));
			Protocol.AddEventCallbacks(OnDataAvailable, OnBytesAvailable);
			client = new TcpClient();
			Task t = client.ConnectAsync(hostname, port);
			await t;
			NetworkStream stream = client.GetStream();
			buffer = new byte[client.ReceiveBufferSize];
			stream.BeginRead(buffer, 0, buffer.Length, ReadCallback, client);
		}
		private void ReadCallback(IAsyncResult result)
		{
			Log("TRACE--> Read callback");
			NetworkStream networkStream = null;
			try {
				networkStream = client.GetStream();
			} catch (ObjectDisposedException) {
				ProcessDisconnect();
				return;
			}
			int read;
			try {
				read = networkStream.EndRead(result);
			} catch (System.IO.IOException) {
				ProcessDisconnect();
				return;
			}
			if (read == 0) {
				client.Close();
			}
			Log("TRACE--> Protocol handoff");
			Protocol.ProcessData(buffer, read, 0);
			try {
				networkStream.BeginRead(buffer, 0, buffer.Length, ReadCallback, client);
			} catch (ObjectDisposedException) {
				ProcessDisconnect();
				return;
			}
		}
		public void AwaitConnect()
		{
			while (!client.Connected) {
				System.Threading.Thread.Sleep(5);
			}
		}
	}
}
