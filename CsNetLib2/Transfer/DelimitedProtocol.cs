using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsNetLib2
{
	public class DelimitedProtocol : TransferProtocol
	{
		private DataAvailabe DataAvailableCallback;
		private BytesAvailable BytesAvailableCallback;

		private byte[] l_Delimiter = new byte[]{ 0x00 };
		public byte[] Delimiter { get { return l_Delimiter; } set { l_Delimiter = value; } }

		private byte[] Retain = new byte[0];

		private Action<string> logCallback;

		public DelimitedProtocol(Encoding encoding, Action<string> logCallback) : base(encoding) {
			this.logCallback = logCallback;
		}

		public override void AddEventCallbacks(DataAvailabe data, BytesAvailable bytes)
		{
			DataAvailableCallback = data;
			BytesAvailableCallback = bytes;
		}

		public override byte[] FormatData(byte[] data)
		{
			byte[] newData = new byte[data.Length + Delimiter.Length]; // Add space for delimiter
			Array.Copy(data, newData, data.Length); // Put the data back in
			Array.Copy(Delimiter, 0, newData, data.Length, Delimiter.Length);
			return newData;
		}

		public override void ProcessData(byte[] buffer, int read, long clientId)
		{
			if (Retain.Length != 0) { // There's still data left over
				byte[] oldBuf = buffer; // Temporarily put the new data aside
				buffer = new byte[read + Retain.Length]; // Expand the buffer to fit both the old and the new data
				Array.Copy(Retain, buffer, Retain.Length); // Put the old data in first
				Array.Copy(oldBuf, 0, buffer, Retain.Length, read); // Now put the new data back in
				read += Retain.Length;
			}
			//var set = buffer.Where(b => b != 0).Select(b => string.Format("{0:X2}", b));
			var set = buffer.Where(b => b != 0).Select(b => (char)b);
			var dmp = string.Join("", set);


			int beginIndex = 0; // This is where the next message starts
			for (int i = beginIndex; i < read; i++) { // Iterate over buffer
				if (buffer[i] == Delimiter[0]) { // We've found a delimiter
					bool validDelimiter = true;
					i++;
					for (int j = 1; j < Delimiter.Length; j++, i++) { // Check if the next delimiter bytes occur as well, if applicable
						if (i >= buffer.Length) {
							validDelimiter = false;
							break;
						}
						if (buffer[i] != Delimiter[j]) {
							validDelimiter = false;
							break;
						}
					}
					if (validDelimiter) {
						ProcessMessage(buffer, beginIndex, i - Delimiter.Length, clientId); // Process a message from the begin index to the current position
						beginIndex = i; // Since we've found a new delimiter, set the begin index equal to its location
						i--;
					}
				}
				if (i == read) {
					Retain = new byte[read - beginIndex];
					Array.Copy(buffer, beginIndex, Retain, 0, read - beginIndex); // Take any left over data and keep it for next usage
				}
			}
		}
		private void ProcessMessage(byte[] buffer, int begin, int end, long clientId)
		{
			byte[] data = new byte[end-begin];
			for(int i = 0; i < data.Length; i++){
				data[i] = buffer[i + begin];
			}
			string str = EncodeText(data);
			if(BytesAvailableCallback!= null) BytesAvailableCallback(data, clientId);
			DataAvailableCallback(str, clientId);
		}
	}
}
