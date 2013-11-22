using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsNetLib2
{
	public class DelimitedProtocol : ITransferProtocol
	{
		private DataAvailabe DataAvailableCallback;
		private BytesAvailable BytesAvailableCallback;

		private byte l_Delimiter = 0x00;
		public byte Delimiter { get { return l_Delimiter; } set { l_Delimiter = value; } }

		private byte[] Retain;

		public void AddEventCallbacks(DataAvailabe data, BytesAvailable bytes)
		{
			DataAvailableCallback = data;
			BytesAvailableCallback = bytes;
		}

		public void ProcessData(byte[] buffer, long clientId)
		{
			int beginIndex = 0;
			for (int i = 0; i < buffer.Length; i++) {
				if (buffer[i] == Delimiter) {
					ProcessMessage(buffer, beginIndex, i, clientId);
					beginIndex = i;
				}
			}
		}
		private void ProcessMessage(byte[] buffer, int begin, int end, long clientId)
		{
			byte[] data = new byte[end-begin];
			for(int i = 0; i < data.Length; i++){
				buffer[i+begin] = data[i];
			}
			BytesAvailableCallback(data, clientId);
			DataAvailableCallback(Encoding.ASCII.GetString(data), clientId);
		}
	}
}
