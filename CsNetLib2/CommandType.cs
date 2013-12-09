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
		SMSG,
		RMSG,
		BDCS,
		PONG,
		GSTR,
        RSTR,
        BLCK,
        WHTE,
        CZAR,
        PICK,
        BLNK,
        CZPK,
        REVL,
        PNTS,
        RWIN,
        ENDG,
        GWIN,
        CRTO,
        CZTO,
        UNRG,
        PING,
        DISC,
        // For use when an invalid command is sent to AAHProtocolWrapper.
        // Recommended behaviour for handlers registered to this on a server is to reply with UNRG.
        // Recommended behaviour for clients is not to bind to this.
        _InternalInvalid
	}
}
