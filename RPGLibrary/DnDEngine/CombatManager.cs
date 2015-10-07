using System;
using System.Linq;
using System.Text;
using RPGLibrary;

namespace DnDEngine
{
	public static class CombatManager
	{
		public static Damage CalculateDamage(IUnit unit, DamageType type, EnergyType? energy, int baseValue)
		{
			if (unit == null) throw new ArgumentException("Unit cannot be null.", "unit");

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
				(int)Math.Ceiling(dmgVal / (Math.Abs(statVal) / 100f + 1)) :
				dmgVal + dmgVal * statVal / 100;

			return new Damage(type, energy, dmgVal);
		}

		public static int ApplyDamage(IUnit unit, Damage damage)
		{
			if (unit == null) throw new ArgumentException("Unit cannot be null.", "unit");

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
					(int)Math.Ceiling(dmgVal / (Math.Abs(statVal) / 100f + 1)) :
					dmgVal + dmgVal * statVal / 100;
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

		public static ILoggable RollAttack(IUnit attacker, IUnit defender, IWeapon weapon = null)
		{
			if (attacker == null) throw new ArgumentException("Attacker cannot be null.", "attacker");
			if (defender == null) throw new ArgumentException("Defender cannot be null.", "defender");

			// If no weapon specified in function call, get attacker's equipped weapon
			if (weapon == null)
				weapon = attacker.GetDetails<IUnitEquipment>().GetWeapon();

			// If no equipped weapon, default to unarmed strike
			if (weapon == null)
				weapon = Catalog.UnarmedStrike;

			// Resolve attack and return result
			return new AttackResult(attacker, defender, weapon);
		}

		[Serializable]
		private class HitResult : ILoggable
		{
			public string Attacker { get; private set; }
			public string Defender { get; private set; }

			public DiceResult Roll { get; private set; }
			public int AttackerBonus { get; private set; }
			public int DefenderAVD { get; private set; }
			public bool Hit { get; private set; }
			public bool CritRange { get; private set; }
			public DiceResult CritRoll { get; private set; }
			public bool Crit { get; private set; }
			public bool CritConfirm { get; private set; }

			public HitResult(IUnit attacker, IUnit defender, int critRange)
			{
				Attacker = attacker.Name;
				Defender = defender.Name;

				AttackerBonus = attacker.Stats.Calculated[DerivedStats.HIT];
				DefenderAVD = defender.Stats.Calculated[DerivedStats.AVD];

				// Roll to-hit
				Dice hit = new Dice(DiceType.D20);
				Roll = hit.Roll();

				// If hit
				if (Roll.Total + AttackerBonus >= DefenderAVD)
				{
					Hit = true;

					// Roll for crit
					if (Roll.Total >= critRange)
					{
						Crit = true;
						CritRoll = hit.Roll();
						CritConfirm = CritRoll.Total >= critRange;
					}
				}
			}

			string ILoggable.Inline
			{
				get { return String.Format("{0} {1} {2}!", Attacker, Hit ? CritConfirm ? "crits" : "hits" : "misses", Defender); }
			}

			string ILoggable.Full
			{
				get
				{
					StringBuilder s = new StringBuilder();
					s.AppendLine(String.Format("{0} rolls a {1} ({2}+{3}) and {4} {5} ({6})!",
						Attacker, Roll.Total + AttackerBonus, Roll.TotalString, AttackerBonus,
						Hit ? "hits" : "misses", Defender, DefenderAVD));
					if (Crit)
					{
						s.AppendLine(String.Format("{0} rolls {1} and {2} ", Attacker, CritRoll.TotalString, CritConfirm ? "confirms crit!" : "fails to confirm crit."));
					}
					return s.ToString();
				}
			}
		}

		[Serializable]
		private class AttackResult : ILoggable
		{
			[NonSerialized]
			private IUnit attacker;
			[NonSerialized]
			private IUnit defender;
			[NonSerialized]
			private IWeapon weapon;

			public string Attacker { get; private set; }
			public string Defender { get; private set; }
			public string Weapon { get; private set; }

			public HitResult HitRoll { get; private set; }

			public DamageReport DamageDealt { get; private set; }

			private void Resolve()
			{
				// Roll to-hit
				HitRoll = new HitResult(attacker, defender, weapon.CritRange);

				// If hit
				if (HitRoll.Hit)
				{
					// Roll for damage
					Dice damage = new Dice(weapon.Damage);
					int rollTotal = damage.Roll().Total;

					// Calculate damage dealt
					Damage damageDealt = CalculateDamage(attacker, DamageType.Physical, null, rollTotal * (HitRoll.CritConfirm ? weapon.CritMultiplier : 1));

					// Apply damage to defender
					int damageTaken = ApplyDamage(defender, damageDealt);

					// Record damage results
					DamageDealt.AddDamage(damage.Result, damageDealt, damageTaken);

					// Damage from weapon enchantments (non-scaled?)
				}
			}

			public AttackResult(IUnit attacker, IUnit defender, IWeapon weapon)
			{
				this.attacker = attacker;
				this.defender = defender;
				this.weapon = weapon;

				Attacker = attacker.Name;
				Defender = defender.Name;
				Weapon = weapon.Name;

				DamageDealt = new DamageReport(attacker, defender);

				Resolve();
			}

			string ILoggable.Inline
			{
				get
				{
					return String.Format("{0} attacks {1} with a {2} for {3} damage.", Attacker, Defender, Weapon, DamageDealt.TotalTaken);
				}
			}

			string ILoggable.Full
			{
				get
				{
					StringBuilder s = new StringBuilder();
					s.Append((HitRoll as ILoggable).Full);
					if (HitRoll.Hit)
					{
						s.Append((DamageDealt as ILoggable).Full);
					}
					else
					{
						s.AppendLine("No damage was dealt.");
					}

					return s.ToString();
				}
			}
		}

		public static ILoggable RollSpell(IUnit caster, ISpell spell, params IUnit[] targets)
		{
			if (caster == null) throw new ArgumentException("Caster cannot be null.", "caster");
			if (spell == null) throw new ArgumentException("Spell cannot be null.", "spell");

			// Resolve spell and return result
			return new SpellResult(caster, spell, targets);
		}

		[Serializable]
		private class SpellResult : ILoggable
		{
			[NonSerialized]
			private IUnit caster;
			[NonSerialized]
			private IUnit[] targets;
			[NonSerialized]
			private ISpell spell;

			public string Caster { get; private set; }
			public string[] Targets { get; private set; }
			public string Spell { get; private set; }

			public HitResult[] HitRolls { get; private set; }
			public CoreStats? SpellSave { get; private set; }
			public int SpellDC { get; private set; }
			public DiceResult[] SaveRolls { get; private set; }
			public int[] SaveBonus { get; private set; }
			public bool[] Saved { get; private set; }

			public DamageReport[] DamageDealt { get; private set; }

			public ILoggable[] AdditionalEffects { get; private set; }

			private void Resolve()
			{
				// Determine scaling stat
				string stat = null;
				switch (spell.Magic)
				{
					case MagicType.Arcane:
						stat = CoreStats.INT.ToString();
						break;
					case MagicType.Divine:
						stat = CoreStats.WIS.ToString();
						break;
				}

				// Get scale factor (minimum 1)
				int n = 1;
				if (stat != null)
				{
					n = Math.Max(1, caster.Stats.Calculated[stat] - 9);
				}


				// Iterate through targets
				for (int i = 0; i < Targets.Length; i++)
				{
					Dice d20 = new Dice(DiceType.D20);

					// Default local value to true for non-ray spells
					bool hit = true;

					// Roll hit for rays
					if (spell.Type == CastType.Ray)
					{
						HitRolls[i] = new HitResult(caster, targets[i], 20);
						hit = HitRolls[i].Hit;
					}

					// Roll saves if applicable
					if (SpellSave != null)
					{
						SaveRolls[i] = d20.Roll();
						SaveBonus[i] = targets[i].Stats.Calculated[SpellSave];
						Saved[i] = SaveRolls[i].Total + SaveBonus[i] >= SpellDC;
					}

					// If the spell hit (always true for non-rays)
					if (hit)
					{
						// If the spell deals damage
						if (spell.Damage != null)
						{
							// Roll for damage
							Dice damageDice = new Dice(spell.Damage.Value, n);
							DiceResult damageRoll = damageDice.Roll();

							// Calculate damage dealt
							DamageType type = spell.Energy == EnergyType.Force ? DamageType.Physical : DamageType.Magical;
							Damage damageDealt = CalculateDamage(caster, type, spell.Energy, damageRoll.Total);

							// Apply damage to target (heals from Positive energy included)
							int damageTaken = ApplyDamage(targets[i], damageDealt);

							// Record damage results
							DamageDealt[i].AddDamage(damageRoll, damageDealt, damageTaken);
						}
						
						// Call spell-specific cast handlers
						AdditionalEffects[i] = spell.Cast(caster, targets[i], Saved[i]);
					}
				}
			}

			public SpellResult(IUnit caster, ISpell spell, params IUnit[] targets)
			{
				this.caster = caster;
				this.targets = targets;
				this.spell = spell;

				Caster = caster.Name;
				Spell = spell.Name;
				Targets = targets.Select<IUnit, string>(x => x.Name).ToArray<string>();

				HitRolls = new HitResult[Targets.Length];
				SpellSave = spell.Save;
				SpellDC = 10 + caster.Level;
				SaveRolls = new DiceResult[Targets.Length];
				SaveBonus = new int[Targets.Length];
				Saved = new bool[Targets.Length];

				DamageDealt = new DamageReport[Targets.Length];
				AdditionalEffects = new ILoggable[Targets.Length];

				Resolve();
			}

			string ILoggable.Inline
			{
				get { throw new NotImplementedException(); }
			}

			string ILoggable.Full
			{
				get { throw new NotImplementedException(); }
			}
		}
	}
}
