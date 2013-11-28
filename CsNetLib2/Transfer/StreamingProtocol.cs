using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsNetLib2
{
	public class StreamingProtocol : TransferProtocol
	{
		private DataAvailabe DataAvailableCallback;
		private BytesAvailable BytesAvailableCallback;

		public StreamingProtocol(Encoding encoding) : base(encoding) { }

		public override void AddEventCallbacks(DataAvailabe data, BytesAvailable bytes)
		{
			DataAvailableCallback = data;
			BytesAvailableCallback = bytes;
		}

		public override byte[] FormatData(byte[] data)
		{
			return data; // Streaming protocol doesn't care about formatting
		}

		public override void ProcessData(byte[] buffer, int read, long clientId)
		{
			if (BytesAvailableCallback != null) {
				BytesAvailableCallback(buffer, clientId);
			}
			if (DataAvailableCallback != null) {
				DataAvailableCallback(EncodeText(buffer), clientId);
			}
		}
	}
}
