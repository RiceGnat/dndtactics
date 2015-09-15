using System;

namespace RPGLibrary
{
	[Serializable]
	public abstract class StatsBase : IStats
	{
		public abstract int Get(string stat);

		public int Get(Enum stat)
		{
			return Get(stat.ToString());
		}

		public abstract int Set(string stat, int value);

		public int Set(Enum stat, int value)
		{
			return Set(stat.ToString(), value);
		}

		public int this[string stat]
		{
			get
			{
				return Get(stat);
			}
			set
			{
				Set(stat, value);
			}
		}

		public int this[Enum stat]
		{
			get
			{
				return Get(stat);
			}
			set
			{
				Set(stat, value);
			}
		}
	}
}
