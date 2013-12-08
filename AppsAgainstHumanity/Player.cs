using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsAgainstHumanityClient
{
	/// <summary>
	/// Holds information about an Apps Against Humanity player
	/// </summary>
	class Player
	{
		/// <summary>
		/// Holds the amount of Awesome Points the player has won
		/// </summary>
		public int AwesomePoints { get; set; }
		/// <summary>
		/// The player's nickname. Also used to identify the player.
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// True if the player is currently the Card Czar, false otherwise.
		/// </summary>
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
