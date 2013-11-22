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

		public void ProcessData(byte[] buffer, long clientId)
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
