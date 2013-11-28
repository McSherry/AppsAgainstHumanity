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
		private Dictionary<long, NetLibServerInternalClient> _clients = new Dictionary<long, NetLibServerInternalClient>();
		private TransferProtocol Protocol;

		public event DataAvailabe OnDataAvailable;
		public event BytesAvailable OnBytesAvailable;
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
			Protocol = new TransferProtocolFactory().CreateTransferProtocol(protocol, encoding);
			Listener = new TcpListener(localaddr, port);
		}

		public void CloseClientConnection(long clientId)
		{
			_clients[clientId].TcpClient.Close();
			_clients.Remove(clientId);
		}

		public void StartListening()
		{
			Protocol.AddEventCallbacks(OnDataAvailable, OnBytesAvailable);
			Listener.Start();
			Listener.BeginAcceptTcpClient(AcceptTcpClientCallback, null);
		}
		private void AcceptTcpClientCallback(IAsyncResult ar)
		{
			var tcpClient = Listener.EndAcceptTcpClient(ar);
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
				Console.WriteLine("Remote host closed connection.");
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
			}catch(System.IO.IOException){
				Console.WriteLine("Remote host closed connection.");
			}
		}
		public bool SendBytes(byte[] buffer, long clientId)
		{
			buffer = Protocol.FormatData(buffer);
			lock (_clients) {
                try
                {
                    _clients[clientId].NetworkStream.BeginWrite(buffer, 0, buffer.Length, SendCallback, clientId);
                    return true;
                }
                catch (NullReferenceException nrex)
                {
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
				Console.WriteLine("Failed to send data to client: Remote host closed connection.");
			}

		}
		public void Stop()
		{
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
					try {
						return TcpClient.GetStream();
					}catch(InvalidOperationException){
						return null;
					}
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
