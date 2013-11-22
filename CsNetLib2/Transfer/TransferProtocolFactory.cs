using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsNetLib2
{
	class TransferProtocolFactory
	{
		public ITransferProtocol CreateTransferProtocol(TransferProtocol protocol)
		{
			switch (protocol) {
				case TransferProtocol.Streaming:
					return new StreamingProtocol();
				case TransferProtocol.Delimited:
					return new DelimitedProtocol();
				case TransferProtocol.SetSize:
					return new SetSizeProtocol();
				default:
					return null;
			}
		}
	}
}
