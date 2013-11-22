using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsNetLib2
{
	class Program
	{
		static void Main(string[] args)
		{
			var pr = new DelimitedProtocol();

			DataAvailabe da = (data, id) => {
				Console.WriteLine("Received string: \"{0}\"", data);
			};

			BytesAvailable ba = (data, id) => {

			};

			pr.AddEventCallbacks(da, ba);

			pr.Delimiter = 126; // ~ character
			pr.ProcessData(new byte[] { 65, 66, 65, 66, 65, 66, 126, 65, 126, 67, 68, 67, 68, 67, 68 }, 0); // A B A B A B END A END C D C D C D
			pr.ProcessData(new byte[] { 69, 70, 71, 72, 126 }, 0); // E F G H END
			pr.ProcessData(new byte[] { 69, 70, 71, 72, 126 }, 0); // E F G H END
			Console.ReadKey();
		}
	}
}
