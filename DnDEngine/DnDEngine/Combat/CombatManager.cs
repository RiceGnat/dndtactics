using System;
using System.Linq;
using System.Text;
using DnDEngine.Logging;

namespace DnDEngine.Combat
{
	public static class CombatManager
	{
		public static Damage CalculateDamage(IUnitEx unit, DamageType type, EnergyType? energy, int baseValue)
		{
			if (unit == null) throw new ArgumentNullException("unit");

			int dmgVal = baseValue;
			string stat = null;
			int statVal;

			// Determine damage bonus stat
			if (energy != null)
			{
				stat = DerivedStats.MAG.ToString();
			}
			else
			{
				stat = DerivedStats.ATK.ToString();
			}

			// Calculate damage with bonus
			statVal = unit.Stats.Calculated[stat];
			dmgVal = statVal < 0 ?
				(int)Math.Ceiling(dmgVal / (-statVal / 100f + 1)) :
				dmgVal + dmgVal * statVal / 100;

			return new Damage(type, energy, dmgVal);
		}

		public static int ApplyDamage(IUnitEx unit, Damage damage)
		{
			if (unit == null) throw new ArgumentNullException("unit");

			IVolatileStats unitVals = unit.GetDetails<IVolatileStats>();

			int initial = unitVals.CurrentHP;
			int dmgVal = damage.Value;
			string stat = null;
			int statVal;

			// Determine resisting stat (Positive and Negative energy can't be resisted)
			if (damage.Energy != EnergyType.Positive && damage.Energy != EnergyType.Negative)
			{
				if (damage.Type == DamageType.Physical)
				{
					stat = DerivedStats.DEF.ToString();
				}
				else if (damage.Type == DamageType.Magical)
				{
					stat = DerivedStats.RES.ToString();
				}
			}

			// Calculate damage after resistances
			if (stat != null)
			{
				statVal = unit.Stats.Calculated[stat];
				dmgVal = statVal >= 0 ?
					(int)Math.Ceiling(dmgVal / (statVal / 100f + 1)) :
					dmgVal + dmgVal * -statVal / 100;
			}

			// Modify unit's current HP
			if (damage.Energy == EnergyType.Positive)
			{ // Positive energy heals target (add checks for undead later!)
				unitVals.CurrentHP += dmgVal;
			}
			else unitVals.CurrentHP -= dmgVal;

			// Return HP lost
			return initial - unitVals.CurrentHP;
		}

		public static ILoggable RollAttack(IUnitEx attacker, IUnitEx defender, IWeapon weapon = null)
		{
			if (attacker == null) throw new ArgumentNullException("attacker");
			if (defender == null) throw new ArgumentNullException("defender");

			// If no weapon specified in function call, get attacker's equipped weapon
			if (weapon == null)
				weapon = attacker.Equipment.GetWeapon();

			// If no equipped weapon, default to unarmed strike
			if (weapon == null)
				weapon = EquipmentCatalog.UnarmedStrike;

			// Resolve attack and return result
			return new AttackResult(attacker, defender, weapon);
		}


		public static ILoggable RollSpell(IUnitEx caster, ISpell spell, params IUnitEx[] targets)
		{
			if (caster == null) throw new ArgumentNullException("caster");
			if (spell == null) throw new ArgumentNullException("spell");

			// Resolve spell and return result
			return new SpellResult(caster, spell, targets);
		}
	}
}
