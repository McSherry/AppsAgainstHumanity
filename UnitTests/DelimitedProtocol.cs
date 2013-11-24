﻿using System;
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

		private byte[] Retain = new byte[0];

		public void AddEventCallbacks(DataAvailabe data, BytesAvailable bytes)
		{
			DataAvailableCallback = data;
			BytesAvailableCallback = bytes;
		}

		public void ProcessData(byte[] buffer, long clientId)
		{
			if (Retain.Length != 0) { // There's still data left over
				byte[] oldBuf = buffer; // Temporarily put the new data aside
				buffer = new byte[oldBuf.Length + Retain.Length]; // Expand the buffer to fit both the old and the new data
				Array.Copy(Retain, buffer, Retain.Length); // Put the old data in first
				Array.Copy(oldBuf, 0, buffer, Retain.Length, oldBuf.Length); // Now put the new data back in
			}

			int beginIndex = 0; // This is where the next message starts
			for (int i = beginIndex; i < buffer.Length; i++) { // Iterate over buffer
				if (buffer[i] == Delimiter) { // We've found a delimiter
					ProcessMessage(buffer, beginIndex == 0 ? beginIndex : beginIndex + 1, i, clientId); // Process a message from the begin index to the current position
					beginIndex = i; // Since we've found a new delimiter, set the begin index equal to its location
				}
				if (i == buffer.Length - 1) {
					Retain = new byte[buffer.Length - beginIndex - 1];
					Array.Copy(buffer, beginIndex + 1, Retain, 0, buffer.Length - beginIndex - 1); // Take any left over data and keep it for next usage
				}
			}
		}
		private void ProcessMessage(byte[] buffer, int begin, int end, long clientId)
		{
			byte[] data = new byte[end-begin];
			for(int i = 0; i < data.Length; i++){
				data[i] = buffer[i + begin];
			}
			BytesAvailableCallback(data, clientId);
			DataAvailableCallback(Encoding.ASCII.GetString(data), clientId);
		}
	}
}