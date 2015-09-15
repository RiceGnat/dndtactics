using System;
using System.Collections.Generic;

namespace RPGLibrary
{
	[Serializable]
	public class StatsMap : StatsBase
	{
		private Dictionary<string, int> map = new Dictionary<string, int>();

		public override int Get(string stat)
		{
			if (!map.ContainsKey(stat))
			{
				throw new ArgumentException("No stat with the name " + stat + " has been set.");
			}

			return map[stat];
		}

		public override int Set(string stat, int value)
		{
			if (!map.ContainsKey(stat))
			{
				map.Add(stat, 0);
			}

			map[stat] = value;

			return map[stat];
		}
	}
}
