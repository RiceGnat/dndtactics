using System;
using System.Linq;
using RPGLibrary;

namespace DnDEngine
{
	public enum CoreStats
	{
		LVL,
		STR,
		CON,
		DEX,
		INT,
		WIS,
		CHA
	}

	public enum DerivedStats
	{
		HP,
		MP,
		ATK,
		DEF,
		MAG,
		RES,
		HIT,
		AVD
	}

	[Serializable]
	public class DnDStats : StatsMap
	{
		public static int CalculateATK(int strVal)
		{
			return strVal;
		}

		private int CalculateDerivedStat(string stat)
		{
			int val = 0;
			if (stat == DerivedStats.HP.ToString())
				val = 10 + Get(CoreStats.CON) * Get(CoreStats.LVL);
			if (stat == DerivedStats.MP.ToString())
				val = (Get(CoreStats.INT) + Get(CoreStats.WIS) - 10) * Get(CoreStats.LVL);
			if (stat == DerivedStats.ATK.ToString())
				val = (Get(CoreStats.STR) - 10) * 10;
			if (stat == DerivedStats.DEF.ToString())
				val = (Get(CoreStats.CON) - 10) * 8 + (Get(CoreStats.STR) - 10) * 2;
			if (stat == DerivedStats.MAG.ToString())
				val = (Get(CoreStats.INT) - 10) * 10;
			if (stat == DerivedStats.RES.ToString())
				val = (Get(CoreStats.WIS) - 10) * 5 + (Get(CoreStats.CHA) - 10) * 5;
			if (stat == DerivedStats.HIT.ToString())
				val = Get(CoreStats.DEX) - 10 + Get(CoreStats.LVL) - 1;
			if (stat == DerivedStats.AVD.ToString())
				val = Get(CoreStats.DEX);
			return val;
		}

		public bool IsLinked { get; private set; }

		private bool IsDerivedStat(string stat)
		{
			return Enum.GetNames(typeof(DerivedStats)).Contains(stat);
		}

		public override int Get(string stat)
		{
			if (IsLinked && IsDerivedStat(stat))
			{
				return CalculateDerivedStat(stat);
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

			foreach (string stat in Enum.GetNames(typeof(CoreStats)))
			{
				Set(stat, 0);
			}

			if (!IsLinked)
			{
				foreach (string stat in Enum.GetNames(typeof(DerivedStats)))
				{
					Set(stat, 0);
				}
			}
		}

		public DnDStats(DnDStats source, bool copyValues = false)
		{
			this.IsLinked = source.IsLinked;

			foreach (string stat in Enum.GetNames(typeof(CoreStats)))
			{
				Set(stat, source[stat]);
			}
		}
	}
}
