using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsNetLib2
{
	public class StreamingProtocol : ITransferProtocol
	{
		private DataAvailabe DataAvailableCallback;
		private BytesAvailable BytesAvailableCallback;

		public void AddEventCallbacks(DataAvailabe data, BytesAvailable bytes)
		{
			DataAvailableCallback = data;
			BytesAvailableCallback = bytes;
		}

		public byte[] FormatData(byte[] data)
		{
			return data; // Streaming protocol doesn't care about formatting
		}

		public void ProcessData(byte[] buffer, int read, long clientId)
		{
			if (BytesAvailableCallback != null) {
				BytesAvailableCallback(buffer, clientId);
			}
			if (DataAvailableCallback != null) {
				DataAvailableCallback(Encoding.ASCII.GetString(buffer), clientId);
			}
		}
	}
}
