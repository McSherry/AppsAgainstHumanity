using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsNetLib2
{
	/// <summary>
	/// A command object that holds all the information available for a command.
	/// </summary>
	public struct Command
	{
		/// <summary>
		/// The type of this command.
		/// </summary>
		public CommandType Type;
		/// <summary>
		/// The clientId of the client who sent this command.
		/// </summary>
		public long Sender;
		/// <summary>
		/// The arguments to this command, if any. Null if no arguments are present.
		/// </summary>
		public string[] Arguments;
	}
}
