using System;

namespace RPGLibrary
{
	/// <summary>
	/// Gives a constant value for every stat.
	/// </summary>
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

		private static readonly StatsConstant zero = new StatsConstant(0);
		private static readonly StatsConstant one = new StatsConstant(1);

		public static StatsConstant Zero { get { return zero; } }
		public static StatsConstant One { get { return one; } }
	}
}
