using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsNetLib2
{
	public interface ITransferProtocol
	{
		/// <summary>
		/// Passes callback pointers to OnDataAvailable events to the class that implements the ProcessData method, 
		/// so that the appropriate events can be fired once the data has been processed. 
		/// </summary>
		/// <param name="data">Delegate to the OnDataAvailable event that will be fired</param>
		/// <param name="bytes">Delegate to the OnBytesAvaialable event that will be fired</param>
		void AddEventCallbacks(DataAvailabe data, BytesAvailable bytes);

		/// <summary>
		/// Processes the incoming data. Once the data has been processed, events are fired and the data is passed with them.
		/// </summary>
		void ProcessData(byte[] buffer, long clientId);
	}
}
