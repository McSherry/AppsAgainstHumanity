using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace CsNetLib2
{
    public class NetLibServer : ITransmittable
    {
		private TcpListener Listener;
		private Dictionary<long, Client> Clients = new Dictionary<long, Client>();
		private ITransferProtocol Protocol;

		public event DataAvailabe OnDataAvailable;
		public event BytesAvailable OnBytesAvailable;

		public NetLibServer(int port, TransferProtocol protocol)
			: this(IPAddress.Any, port, protocol) { }
		public NetLibServer(IPAddress localaddr, int port, TransferProtocol protocol)
		{
			Protocol = new TransferProtocolFactory().CreateTransferProtocol(protocol);
			Listener = new TcpListener(localaddr, port);
			Protocol.AddEventCallbacks(OnDataAvailable, OnBytesAvailable);
		}
		public void Start()
		{
			Listener.Start();
			Listener.BeginAcceptTcpClient(AcceptTcpClientCallback, null);
		}
		private void AcceptTcpClientCallback(IAsyncResult ar)
		{
			var tcpClient = Listener.EndAcceptTcpClient(ar);
			var buffer = new byte[tcpClient.ReceiveBufferSize];
			var client = new Client(tcpClient, buffer);

			lock (Clients) {
				Clients.Add(client.ClientId, client);
			}
			var stream = client.NetworkStream;

			stream.BeginRead(client.Buffer, 0, client.Buffer.Length, ReadCallback, client);

		}
		private void ReadCallback(IAsyncResult result)
		{
			var client = result.AsyncState as Client;
			if (client == null) return;
			var networkStream = client.NetworkStream;
			int read = 0;
			try {
				read = networkStream.EndRead(result);
			} catch (System.IO.IOException) {
				Console.WriteLine("Remote host closed connection.");
			}
			if (read == 0) {
				lock (Clients) {
					Clients[client.ClientId] = null;
					return;
				}
			}
			Protocol.ProcessData(client.Buffer.Take(read).ToArray(), client.ClientId);

			try {
				networkStream.BeginRead(client.Buffer, 0, client.Buffer.Length, ReadCallback, client);
			}catch(System.IO.IOException){
				Console.WriteLine("Remote host closed connection.");
			}
		}
		public void SendBytes(byte[] buffer, long clientId)
		{
			buffer = Protocol.FormatData(buffer);
			lock (Clients) {
				Clients[clientId].NetworkStream.BeginWrite(buffer, 0, buffer.Length, SendCallback, clientId);
			}
		}
		public void Send(string data, long clientId)
		{
			byte[] buffer = Encoding.ASCII.GetBytes(data);
			SendBytes(buffer, clientId);
		}
		private void SendCallback(IAsyncResult ar)
		{
			try {
				Clients[(long)ar.AsyncState].NetworkStream.EndWrite(ar);
			} catch (NullReferenceException) {
				Console.WriteLine("Failed to send data to client: Remote host closed connection.");
			}

		}
		public void Stop()
		{
			Listener.Stop();
		}
		internal class Client
		{
			private static long MaxClientId = 0;
			public long ClientId { get; private set; }
			public TcpClient TcpClient { get; private set; }
			public byte[] Buffer { get; private set; }
			public NetworkStream NetworkStream {
				get {
					try {
						return TcpClient.GetStream();
					}catch(InvalidOperationException){
						return null;
					}
				} 
			}
			public Client(TcpClient client, byte[] buffer)
			{
				TcpClient = client;
				Buffer = buffer;
				ClientId = MaxClientId;
				MaxClientId++;
			}
		}
    }
}
