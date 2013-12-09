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
		public string YourName { get; set; }

		public Player CardCzar
		{
			get;
			set;
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

		internal void RemovePlayer(string name)
		{
			var pred = Players.Where(p => p.Name == name);

			Players.RemoveAll(p => p.Name == name);
		}

		internal Player GetPlayer(string name)
		{
			return Players.Where(p => p.Name == name).First();
		}
	}
}
