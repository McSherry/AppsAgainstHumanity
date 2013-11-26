using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsNetLib2
{
	public enum CommandType
	{
		JOIN,
		ACKN,
		REFU,
		NICK,
        NACC,
        NDNY,
        CLNF,
        CLJN,
        CLEX,
        PING,
        // For use when an invalid command is sent to AAHProtocolWrapper.
        // Recommended behaviour for handlers registered to this on a server is to reply with UNRG.
        // Recommended behaviour for clients is not to bind to this.
        _InternalInvalid
	}
}
