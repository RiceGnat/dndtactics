using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RPGLibrary
{
	[Serializable]
	public class UnitBase : IUnit
	{
		protected class BaseStatsRouter : IStatsPackage
		{
			private UnitBase unit;

			public IStats Calculated
			{
				get { 
					return unit.Modifiers.Result.Stats == this ? Base : unit.modifiers.Result.Stats.Calculated; 
				}
			}

			public IStats Base
			{
				get { return unit.baseStats; }
			}

			public IStats Additions
			{
				get { return StatsConstant.Zero; }
			}

			public IStats Multiplications
			{
				get { return StatsConstant.Zero; }
			}

			public BaseStatsRouter(UnitBase unit)
			{
				this.unit = unit;
			}
		}

		[NonSerialized]
		private BaseStatsRouter statRouter;
		private DecoratorManager<IUnit> modifiers = new DecoratorManager<IUnit>();

		protected IStats baseStats;
		protected object details;

		public virtual string Name { get; set; }
		public virtual string Class { get; set; }
		public virtual int Level { get; set; }

		public virtual IStatsPackage Stats { get { return statRouter; } }
		public virtual IDecoratorManager<IUnit> Modifiers { get { return modifiers; } }

		public virtual T GetDetails<T>()
			where T : class
		{
			return details as T;
		}

		protected virtual void Link()
		{
			statRouter = new BaseStatsRouter(this);
			modifiers.Bind(this);
		}

		[OnDeserialized]
		private void Rebind(StreamingContext context)
		{
			Link();
		}

		public UnitBase()
			: this("Unit", "Class", 1) { }

		public UnitBase(string name, string unitClass, int level)
			: this(name, unitClass, level, new StatsMap()) { }

		public UnitBase(string name, string unitClass, int level, IStats baseStats)
		{
			this.baseStats = baseStats;

			Name = name;
			Class = unitClass;
			Level = level;

			Link();
		}
	}
}
