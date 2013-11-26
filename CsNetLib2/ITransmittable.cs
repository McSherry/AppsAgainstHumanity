using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsNetLib2
{
	public interface ITransmittable
	{
		bool Send(string data, long clientId);
		event DataAvailabe OnDataAvailable;
	}
}
