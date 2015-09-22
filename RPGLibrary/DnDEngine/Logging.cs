using System;
using System.Text;
using RPGLibrary;

namespace DnDEngine
{
	public static class Logging
	{
		public const string ModifierFormat = "+#;-#;+0";

		public static string DescribeUnit(IUnit unit)
		{
			StringBuilder s = new StringBuilder();
			s.AppendLine(unit.Name);
			s.AppendLine(String.Format("Level {0} {1}", unit.Level, unit.Class));
			s.AppendLine();

			s.AppendFormat("HP {0}/{1}", unit.GetDetails<IVolatileStats>().CurrentHP, unit.Stats.Calculated[DerivedStats.HP]);
			s.AppendLine(String.Format("\tHIT {0,3}", unit.Stats.Calculated[DerivedStats.HIT].ToString(Logging.ModifierFormat)));
			s.AppendFormat("MP {0}/{1}", unit.GetDetails<IVolatileStats>().CurrentMP, unit.Stats.Calculated[DerivedStats.MP]);
			s.AppendLine(String.Format("\tAVD {0,3}", unit.Stats.Calculated[DerivedStats.AVD]));
			s.AppendLine();

			s.AppendFormat("STR {0,2}", unit.Stats.Calculated[CoreStats.STR]);
			s.AppendLine(String.Format("\tATK {0,3}", unit.Stats.Calculated[DerivedStats.ATK]));
			s.AppendFormat("CON {0,2}", unit.Stats.Calculated[CoreStats.CON]);
			s.AppendLine(String.Format("\tDEF {0,3}", unit.Stats.Calculated[DerivedStats.DEF]));
			s.AppendFormat("DEX {0,2}", unit.Stats.Calculated[CoreStats.DEX]);
			s.AppendLine(String.Format("\tMAG {0,3}", unit.Stats.Calculated[DerivedStats.MAG]));
			s.AppendFormat("INT {0,2}", unit.Stats.Calculated[CoreStats.INT]);
			s.AppendLine(String.Format("\tRES {0,3}", unit.Stats.Calculated[DerivedStats.RES]));
			s.AppendLine(String.Format("WIS {0,2}", unit.Stats.Calculated[CoreStats.WIS]));
			s.AppendLine(String.Format("CHA {0,2}", unit.Stats.Calculated[CoreStats.CHA]));

			return s.ToString();
		}
	}
}
