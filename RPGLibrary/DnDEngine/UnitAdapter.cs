using System;
using RPGLibrary;

namespace DnDEngine
{
	[Serializable]
	public class UnitAdapter : IUnitEx
	{
		private IUnit unit;

		private IUnit Proxy
		{
			get
			{
				return unit.Modifiers.Result;
			}
		}

		public string Name
		{
			get
			{
				return Proxy.Name;
			}
			set
			{
				Proxy.Name = value;
			}
		}
		public string Class
		{
			get
			{
				return Proxy.Class;
			}
			set
			{
				Proxy.Class = value;
			}
		}
		public int Level
		{
			get
			{
				return Proxy.Level;
			}
			set
			{
				Proxy.Level = value;
			}
		}

		public IStatsPackage Stats { get { return Proxy.Stats; } }
		public IDecoratorManager<IUnit> Modifiers { get { return Proxy.Modifiers; } }
		

		public IVolatileStats VolatileStats { get { return GetDetails<IVolatileStats>(); } }
		public int CurrentHP
		{
			get { return VolatileStats.CurrentHP; }
			set { VolatileStats.CurrentHP = value; }
		}
		public int CurrentMP
		{
			get { return VolatileStats.CurrentMP; }
			set { VolatileStats.CurrentMP = value; }
		}
		public int Experience
		{
			get { return VolatileStats.Experience; }
			set { VolatileStats.Experience = value; }
		}

		public IUnitEquipment Equipment { get { return GetDetails<IUnitEquipment>(); } }
		public IStats BaselineStats { get { return Equipment.EquippedBaselineUnit.Stats.Calculated; } }

		public void Initialize()
		{
			VolatileStats.Initialize();
		}

		public T GetDetails<T>() where T : class
		{
			return Proxy.GetDetails<T>();
		}

		public UnitAdapter(IUnit unit)
		{
			this.unit = unit;
		}
	}
}
