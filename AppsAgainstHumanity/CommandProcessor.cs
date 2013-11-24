using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace AppsAgainstHumanityClient
{
	class CommandProcessor
	{
		// HACK: Template, remove when done
		private void ProcessTEMP()
		{

		}
		public void ProcessACKN(string[] arguments)
		{

		}
		public void ProcessREFU(long sender, string[] arguments)
		{
			MessageBox.Show(arguments[0], "Unable to connect", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}
}
