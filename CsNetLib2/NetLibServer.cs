using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace CsNetLib2
{
	public delegate void ClientDisconnected(long clientId);

    public class NetLibServer : ITransmittable
    {
		private TcpListener Listener;
		private Dictionary<long, NetLibServerInternalClient> _clients = new Dictionary<long, NetLibServerInternalClient>();
		private TransferProtocol Protocol;

		public event DataAvailabe OnDataAvailable;
		public event BytesAvailable OnBytesAvailable;
		public event ClientDisconnected OnClientDisconnected;
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
        public Dictionary<long, NetLibServerInternalClient> Clients
        {
            get { return _clients; }
            set { _clients = value; }
        }

		public NetLibServer(int port, TransferProtocols protocol, Encoding encoding)
			: this(IPAddress.Any, port, protocol, encoding) { }
		public NetLibServer(IPAddress localaddr, int port, TransferProtocols protocol, Encoding encoding)
		{
			Protocol = new TransferProtocolFactory().CreateTransferProtocol(protocol, encoding, new Action<string>(Log));
			Listener = new TcpListener(localaddr, port);
		}

		private void HandleDisconnect(long clientId)
		{
			Console.WriteLine("Client #{0} disconnected", clientId);
			lock (_clients) {
				_clients[clientId].TcpClient.Close();
				Clients.Remove(clientId);
			}
			if (OnClientDisconnected != null) OnClientDisconnected(clientId);
		}
		public void CloseClientConnection(long clientId)
		{
			_clients[clientId].TcpClient.Close();
			_clients.Remove(clientId);
		}

		private void Log(string message)
		{
			// TODO: remove this or something or maybe add decent logging support
		}

		public void StartListening()
		{
			Protocol.AddEventCallbacks(OnDataAvailable, OnBytesAvailable);
			Listener.Start();
			Listener.BeginAcceptTcpClient(AcceptTcpClientCallback, null);
		}
		private void AcceptTcpClientCallback(IAsyncResult ar)
		{
			TcpClient tcpClient;
			try {
				tcpClient = Listener.EndAcceptTcpClient(ar);
			} catch (ObjectDisposedException) {
				// The AcceptTcpClient method didn't finish, as the server was shut down.
				// Therefore, we can simply cancel execution of the callback method.
				return;
			}
			var buffer = new byte[tcpClient.ReceiveBufferSize];
			var client = new NetLibServerInternalClient(tcpClient, buffer);

			lock (_clients) {
				_clients.Add(client.ClientId, client);
				Console.WriteLine("New TCP client accepted with ID " + client.ClientId);
			}
			var stream = client.NetworkStream;

			stream.BeginRead(client.Buffer, 0, client.Buffer.Length, ReadCallback, client);
			Listener.BeginAcceptTcpClient(AcceptTcpClientCallback, null);
		}
		private void ReadCallback(IAsyncResult result)
		{
			var client = result.AsyncState as NetLibServerInternalClient;
			if (client == null) return;
			var networkStream = client.NetworkStream;
			int read = 0;
			try {
				read = networkStream.EndRead(result);
			} catch (System.IO.IOException) {
				HandleDisconnect(client.ClientId);
				return;
			}
			if (read == 0) {
				lock (_clients) {
					_clients[client.ClientId] = null;
					return;
				}
			}
			Protocol.ProcessData(client.Buffer, read, client.ClientId);

			try {
				networkStream.BeginRead(client.Buffer, 0, client.Buffer.Length, ReadCallback, client);
			} catch (System.IO.IOException) {
				HandleDisconnect(client.ClientId);
			} catch (ObjectDisposedException) {
				Console.WriteLine("Read callback dropped because client #{0} has disconnected.", (long)client.ClientId);
			}
		}
		public bool SendBytes(byte[] buffer, long clientId)
		{
			buffer = Protocol.FormatData(buffer);
			lock (_clients) {
				try {
					_clients[clientId].NetworkStream.BeginWrite(buffer, 0, buffer.Length, SendCallback, clientId);
					return true;
				} catch (KeyNotFoundException) {
					return false;
				} catch (NullReferenceException) {
					HandleDisconnect(clientId);
					return false;
				}
					
			}
		}
		public bool Send(string data, long clientId)
		{
			byte[] buffer = Protocol.EncodingType.GetBytes(data);
			return SendBytes(buffer, clientId);
		}
		private void SendCallback(IAsyncResult ar)
		{
			try {
				_clients[(long)ar.AsyncState].NetworkStream.EndWrite(ar);
			} catch (NullReferenceException) {
				HandleDisconnect((long)ar.AsyncState);
				Console.WriteLine("Failed to send data to client: Remote host closed connection.");
			} catch (ObjectDisposedException) {
				Console.WriteLine("Send callback dropped because client #{0} has disconnected.", (long)ar.AsyncState);
			} catch (KeyNotFoundException) {
				Console.WriteLine("Send callback dropped because no client with ID {0} exists.", (long)ar.AsyncState);
			}

		}
		public void Stop()
		{
			foreach(var pair in _clients)
			Listener.Stop();
		}
		public class NetLibServerInternalClient
		{
			private static long MaxClientId = 0;
			public long ClientId { get; private set; }
			public TcpClient TcpClient { get; private set; }
			public byte[] Buffer { get; private set; }

			public NetworkStream NetworkStream {
				get {
					//try {
						return TcpClient.GetStream();
					/*}catch(InvalidOperationException){
						Console.WriteLine("I'm not sure what triggers this, please check and comment appropriately"); // Might be thrown when a client is disconnected? Maybe fire an event in that case?
						System.Diagnostics.Debugger.Break(); // TODO: Remove these before production
						return null;
					}*/
				} 
			}
            public bool IsAvailable
            {
                get { return TcpClient.Connected; }
            }

			public NetLibServerInternalClient(TcpClient client, byte[] buffer)
			{
				TcpClient = client;
				Buffer = buffer;
				ClientId = MaxClientId;
				MaxClientId++;
			}
		}
    }
}
