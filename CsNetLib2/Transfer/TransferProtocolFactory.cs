using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsNetLib2
{
	class TransferProtocolFactory
	{
		public TransferProtocol CreateTransferProtocol(TransferProtocols protocol, Encoding encoding)
		{
			switch (protocol) {
				case TransferProtocols.Streaming:
					return new StreamingProtocol(encoding);
				case TransferProtocols.Delimited:
					return new DelimitedProtocol(encoding);
				case TransferProtocols.SetSize:
					return new SetSizeProtocol(encoding);
				default:
					return null;
			}
		}
	}
}
