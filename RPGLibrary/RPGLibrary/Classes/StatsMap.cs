using System;
using System.Collections.Generic;

namespace RPGLibrary
{
	/// <summary>
	/// Implements a map for stat lookup.
	/// </summary>
	[Serializable]
	public class StatsMap : StatsBase
	{
		private Dictionary<string, int> map = new Dictionary<string, int>();

		public override int Get(string stat)
		{
			if (!map.ContainsKey(stat))
			{
				return 0;
			}

			return map[stat];
		}

		public override int Set(string stat, int value)
		{
			if (!map.ContainsKey(stat))
			{
				map.Add(stat, 0);
			}

			int old = map[stat];
			map[stat] = value;

			return old;
		}
	}
}
