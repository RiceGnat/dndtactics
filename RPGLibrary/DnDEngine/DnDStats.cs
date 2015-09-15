using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPGLibrary;

namespace DnDEngine
{
	[Serializable]
	public class DnDStats : StatsMap
	{
		public enum Core
		{
			LVL,
			STR,
			CON,
			DEX,
			INT,
			WIS,
			CHA
		}

		public enum Derived
		{
			HP,
			MP,
			ATK,
			DEF,
			MAG,
			RES
		}

		public static int CalculateATK(int strVal)
		{
			return strVal;
		}

		public bool IsLinked { get; private set; }

		private bool IsDerivedStat(string stat)
		{
			return Enum.GetNames(typeof(Derived)).Contains(stat);
		}

		public override int Get(string stat)
		{
			if (IsLinked && IsDerivedStat(stat))
			{
				if (stat == Derived.ATK.ToString())
					return CalculateATK(Get(Core.STR));
			}

			return base.Get(stat);
		}

		public override int Set(string stat, int value)
		{
			if (IsLinked && IsDerivedStat(stat))
			{
				throw new InvalidOperationException("Derived stats are read-only for this object.");
			}

			return base.Set(stat, value);
		}

		public DnDStats(bool linked = true)
		{
			IsLinked = linked;

			foreach (string stat in Enum.GetNames(typeof(Core)))
			{
				Set(stat, 0);
			}

			if (!IsLinked)
			{
				foreach (string stat in Enum.GetNames(typeof(Derived)))
				{
					Set(stat, 0);
				}
			}
		}

		public DnDStats(DnDStats source, bool copyValues = false)
		{
			this.IsLinked = source.IsLinked;

			foreach (string stat in Enum.GetNames(typeof(Core)))
			{
				Set(stat, source[stat]);
			}
		}
	}
}
