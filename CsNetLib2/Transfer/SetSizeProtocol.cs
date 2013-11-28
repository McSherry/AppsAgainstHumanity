using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsNetLib2
{
	class SetSizeProtocol : TransferProtocol
	{
		private DataAvailabe DataAvailableCallback;
		private BytesAvailable BytesAvailableCallback;

		public SetSizeProtocol(Encoding encoding) : base(encoding) { }

		public override void AddEventCallbacks(DataAvailabe data, BytesAvailable bytes)
		{
			DataAvailableCallback = data;
			BytesAvailableCallback = bytes;
		}

		public override byte[] FormatData(byte[] data)
		{
			throw new NotImplementedException("SetSizeProtocol is not implemented yet");
		}

		public override void ProcessData(byte[] buffer, int read, long clientId)
		{
			throw new NotImplementedException("SetSizeProtocol is not implemented yet");
		}
	}
}
