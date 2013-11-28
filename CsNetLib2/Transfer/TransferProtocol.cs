using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsNetLib2
{
	public abstract class TransferProtocol
	{
		public Encoding EncodingType { get; private set; }

		public TransferProtocol(Encoding encodingType)
		{
			EncodingType = encodingType;
		}

		protected string EncodeText(byte[] data)
		{
			return EncodingType.GetString(data);
		}
		protected byte[] DecodeText(string data)
		{
			return EncodingType.GetBytes(data);
		}
		protected string EncodeText(byte[] data, int index, int count)
		{
			return EncodingType.GetString(data, index, count);
		}

		/// <summary>
		/// Passes callback pointers to OnDataAvailable events to the class that implements the ProcessData method, 
		/// so that the appropriate events can be fired once the data has been processed. 
		/// </summary>
		/// <param name="data">Delegate to the OnDataAvailable event that will be fired</param>
		/// <param name="bytes">Delegate to the OnBytesAvaialable event that will be fired</param>
		public abstract void AddEventCallbacks(DataAvailabe data, BytesAvailable bytes);

		/// <summary>
		/// Processes the incoming data. Once the data has been processed, events are fired and the data is passed with them.
		/// </summary>
		public abstract void ProcessData(byte[] buffer, int read, long clientId);

		/// <summary>
		/// Formats the data about to be sent so is formatted according to the protocol.
		/// </summary>
		/// <param name="buffer">The data that is about to be sent.</param>
		/// <returns>The formatted data, to be sent.</returns>
		public abstract byte[] FormatData(byte[] buffer);
	}
}
