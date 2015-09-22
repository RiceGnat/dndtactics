using System;
using System.Text;
using RPGLibrary;

namespace DnDEngine
{
	public static class CombatManager
	{
		public static ILoggable RollAttack(IUnit attacker, IUnit defender, IWeapon weapon = null)
		{
			CombatResult result = new CombatResult();
			result.Attacker = attacker.Name;
			result.Defender = defender.Name;

			if (weapon == null)
				weapon = attacker.GetDetails<IUnitEquipment>().GetWeapon();

			result.Weapon = weapon.Name;

			Dice hit = new Dice(DiceType.D20);
			result.HitRoll = hit.Roll();
			result.HitBonus = attacker.Stats.Calculated[DerivedStats.HIT];
			result.DefenderAVD = defender.Stats.Calculated[DerivedStats.AVD];

			if (result.HitRoll.Total + result.HitBonus >= result.DefenderAVD)
			{
				result.Hit = true;
				if (result.HitRoll.Total >= weapon.CritRange)
				{
					result.Crit = true;
					result.CritRoll = hit.Roll();
					result.CritConfirm = result.CritRoll.Total >= weapon.CritRange;
				}

				Dice damage = new Dice(weapon.Damage);
				result.DamageRoll = damage.Roll();
				result.DamageDealt = CalculateDamage(attacker, DamageType.Physical, null, result.DamageRoll.Total * (result.CritConfirm ? weapon.CritMultiplier : 1));
				result.DamageTaken = ApplyDamage(defender, result.DamageDealt);
			}

			return result;
		}

		public static Damage CalculateDamage(IUnit unit, DamageType type, EnergyType? energy, int baseValue)
		{
			int dmgVal = baseValue;
			string stat = null;
			int statVal;

			if (energy != null)
			{
				stat = DerivedStats.MAG.ToString();
			}
			else
			{
				stat = DerivedStats.ATK.ToString();
			}

			if (stat != null)
			{
				statVal = unit.Stats.Calculated[stat];
				dmgVal = statVal < 0 ?
					(int)Math.Ceiling(dmgVal / (Math.Abs(statVal) / 100f + 1)) :
					dmgVal + dmgVal * statVal / 100;
			}

			return new Damage(unit, type, energy, dmgVal);
		}

		public static int ApplyDamage(IUnit unit, Damage damage)
		{
			IVolatileStats unitVals = unit.GetDetails<IVolatileStats>();

			int initial = unitVals.CurrentHP;
			int dmgVal = damage.Value;
			string stat = null;
			int statVal;

			if (damage.Type == DamageType.Physical)
			{
				stat = DerivedStats.DEF.ToString();
			}
			else if (damage.Type == DamageType.Magical)
			{
				stat = DerivedStats.RES.ToString();
			}

			if (stat != null)
			{
				statVal = unit.Stats.Calculated[stat];
				dmgVal = statVal >= 0 ?
					(int)Math.Ceiling(dmgVal / (Math.Abs(statVal) / 100f + 1)) :
					dmgVal + dmgVal * statVal / 100;
			}

			unitVals.CurrentHP -= dmgVal;

			return initial - unitVals.CurrentHP;
		}
	}

	[Serializable]
	public class CombatResult : ILoggable
	{
		public string Attacker { get; set; }
		public string Defender { get; set; }
		public string Weapon { get; set; }

		public DiceResult HitRoll { get; set; }
		public int HitBonus { get; set; }
		public int DefenderAVD { get; set; }
		public bool Hit { get; set; }
		public bool Crit { get; set; }
		public DiceResult CritRoll { get; set; }
		public bool CritConfirm { get; set; }

		public DiceResult DamageRoll { get; set; }
		public Damage DamageDealt { get; set; }
		public int DamageTaken { get; set; }

		string ILoggable.Inline
		{
			get
			{
				return String.Format("{0} attacks {1} with a {2} for {3} damage.", Attacker, Defender, Weapon, DamageTaken);
			}
		}

		string ILoggable.Full
		{
			get
			{
				StringBuilder s = new StringBuilder();
				s.AppendLine(String.Format("{0} attacks {1} with a {2}.", Attacker, Defender, Weapon));
				s.AppendLine(String.Format("{0} rolls a {1} ({2}+{3}) and {4} {5} ({6})!",
					Attacker, HitRoll.Total + HitBonus, HitRoll.TotalString, HitBonus,
					Hit ? "hits" : "misses", Defender, DefenderAVD));
				if (Hit)
				{
					if (Crit)
					{
						s.AppendLine(String.Format("{0} rolls {1} and {2} ", Attacker, CritRoll.TotalString, CritConfirm ? "confirms crit!" : "fails to confirm crit."));
					}

					s.AppendLine(String.Format("{0} rolls {1}, dealing {2} damage after bonuses.", Attacker, DamageRoll.RollsString, DamageDealt.Value));
					s.AppendLine(String.Format("{0} takes {1} damage after reductions.", Defender, DamageTaken));
				}
				else
				{
					s.AppendLine("No damage was dealt.");
				}

				return s.ToString();
			}
		}
	}
}
