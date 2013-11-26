using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsAgainstHumanityClient
{
	class Player
	{
		public int AwesomePoints { get; set; }
		public string Name { get; private set; }
		public bool CardCzar { get; set; }

		public Player(string name)
		{
			Name = name;
		}
		public override string ToString()
		{
			return string.Format("{0} ({1})", Name, AwesomePoints);
		}
	}
}
