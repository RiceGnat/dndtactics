using System;

namespace RPGLibrary
{
	public class StatsAdder : StatsBase
	{
		private IStats a;
		private IStats b;

		public override int Get(string stat)
		{
			return a[stat] + b[stat];
		}

		public override int Set(string stat, int value)
		{
			throw new InvalidOperationException("Stats may not be set for this object.");
		}

		public StatsAdder(IStats a, IStats b)
		{
			this.a = a;
			this.b = b;
		}
	}
}
