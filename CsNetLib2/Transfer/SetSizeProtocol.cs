using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsNetLib2
{
	class SetSizeProtocol : ITransferProtocol
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
			throw new NotImplementedException("SetSizeProtocol is not implemented yet");
		}

		public void ProcessData(byte[] buffer, int read, long clientId)
		{
			throw new NotImplementedException("SetSizeProtocol is not implemented yet");
		}
	}
}
