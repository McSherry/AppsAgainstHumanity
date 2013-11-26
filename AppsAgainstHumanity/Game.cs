using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsAgainstHumanityClient
{
	class Game
	{
		public List<Player> Players
		{
			get;
			private set;
		}

		public Game()
		{
			Players = new List<Player>();
		}

		internal void AddPlayers(string[] playerNames)
		{
			foreach (string name in playerNames) {
				Players.Add(new Player(name));
			}
		}
	}
}
