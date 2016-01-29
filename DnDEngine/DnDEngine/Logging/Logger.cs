using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DnDEngine.Combat;

namespace DnDEngine.Logging
{
	public class Logger : ILogger
	{
		private const string modifierFormat = "+#;-#;+0";
		public virtual string ModifierFormat
		{
			get { return modifierFormat; }
		}

		public virtual string DescribeUnit(IUnitEx unit, LogType length)
		{
			StringBuilder s = new StringBuilder();
			s.AppendLine(unit.Name);
			s.AppendLine(String.Format("Level {0} {1}", unit.Level, unit.Class));
			s.AppendLine();

			s.AppendFormat("HP {0}/{1}", unit.GetDetails<IVolatileStats>().CurrentHP, unit.Stats.Calculated[DerivedStats.HP]);
			s.AppendLine(String.Format("\tHIT {0,3}", unit.Stats.Calculated[DerivedStats.HIT].ToString(ModifierFormat)));
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

		public virtual string LogHit(HitResult result, LogType length)
		{
			switch (length)
			{
				case LogType.Inline:
					return String.Format("{0} {1} {2}!",
						result.Attacker,
						result.Hit ? result.CritConfirm ? "crits" : "hits" : "misses",
						result.Defender);
				case LogType.Full:
					StringBuilder s = new StringBuilder();
					s.AppendLine(String.Format("{0} rolls a {1} ({2}{3}) and {4} {5} ({6})!",
						result.Attacker,
						result.Roll.Total + result.AttackerBonus,
						result.Roll.TotalString,
						result.AttackerBonus.ToString(Log.ModifierFormat),
						result.Hit ? "hits" : "misses",
						result.Defender,
						result.DefenderAVD));
					if (result.Crit)
					{
						s.AppendLine(String.Format("{0} rolls {1} and {2} ",
							result.Attacker,
							result.CritRoll.TotalString,
							result.CritConfirm ? "confirms crit!" : "fails to confirm crit."));
					}
					return s.ToString();
				default: return null;
			}
		}

		public virtual string LogAttack(AttackResult result, LogType length)
		{
			switch (length)
			{
				case LogType.Inline:
					return String.Format("{0} attacks {1} with a {2} and {3}.",
						result.Attacker,
						result.Defender,
						result.Weapon,
						result.HitRoll.Hit ?
							String.Format("hits, dealing {0} damage", result.DamageDealt.TotalTaken) :
							"misses"
						);
				case LogType.Full:
					StringBuilder s = new StringBuilder();
					s.Append((result.HitRoll as ILoggable).Full);
					if (result.HitRoll.Hit)
					{
						s.Append((result.DamageDealt as ILoggable).Full);
					}
					else
					{
						s.AppendLine("No damage was dealt.");
					}

					return s.ToString();
				default: return null;
			}
		}

		public virtual string LogSpell(SpellResult result, LogType length)
		{
			throw new NotImplementedException();
		}
	}
}
