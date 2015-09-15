using System;
using System.Runtime.Serialization;

namespace RPGLibrary
{
	[Serializable]
	public class StatsModifier : UnitDecorator, IUnit
	{
		private class StatsEvaluator : StatsBase, IStatsPackage
		{
			private StatsModifier modifier;

			public IStats Calculated
			{
				get { return this; }
			}

			public IStats Base
			{
				get { return modifier.baseUnit.Stats.Base; }
			}

			public IStats Additions
			{
				get { return new StatsAdder(modifier.baseUnit.Stats.Additions, modifier.Adds); }
			}

			public IStats Multiplications
			{
				get { return new StatsAdder(modifier.baseUnit.Stats.Multiplications, modifier.Mults); }
			}

			public override int Get(string stat)
			{
				return (Base[stat] + Additions[stat]) * (100 + Multiplications[stat]) / 100;
			}

			public override int Set(string stat, int value)
			{
				throw new InvalidOperationException("Stats may not be modified through this object.");
			}

			public StatsEvaluator(StatsModifier modifier)
			{
				this.modifier = modifier;
			}
		}

		[NonSerialized]
		private StatsEvaluator statEval;

		public IStats Adds { get; protected set; }
		public IStats Mults { get; protected set; }

		IStatsPackage IUnit.Stats
		{
			get { return statEval; }
		}

		protected virtual void Link()
		{
			statEval = new StatsEvaluator(this);
		}

		[OnDeserialized]
		private void Rebind(StreamingContext context)
		{
			Link();
		}

		public StatsModifier()
			: this(new StatsMap(), new StatsMap()) { }

		public StatsModifier(IStats additions, IStats multiplications)
		{
			Link();
			Adds = additions;
			Mults = multiplications;
		}
	}
}
