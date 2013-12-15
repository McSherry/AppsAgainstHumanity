using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppsAgainstHumanity.Server
{
	static class Program
	{
        public static mainForm MainForm;
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
            MainForm = new mainForm();
			Application.Run(MainForm);
		}
	}
}
