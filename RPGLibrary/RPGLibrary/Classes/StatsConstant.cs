using System;

namespace RPGLibrary
{
	public class StatsConstant : StatsBase
	{
		private int value;

		public override int Get(string stat)
		{
			return value;
		}

		public override int Set(string stat, int value)
		{
			throw new InvalidOperationException("Cannot set stats for this object");
		}

		public StatsConstant(int value)
		{
			this.value = value;
		}

		public static StatsConstant Zero = new StatsConstant(0);
		public static StatsConstant One = new StatsConstant(1);
	}
}
