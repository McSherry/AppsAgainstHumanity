using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsAgainstHumanityClient
{
	class BlackCard : Card
	{
		public int PickNum
		{
			get;
			set;
		}

		public BlackCard(string text = null, string id = null, int pickNum = 0)
			:base(text, id)
		{
			PickNum = pickNum;
		}
	}
}
