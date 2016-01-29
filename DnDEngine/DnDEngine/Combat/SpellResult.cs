using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DnDEngine.Logging;

namespace DnDEngine.Combat
{
	[Serializable]
	public class SpellResult : ILoggable
	{
		[NonSerialized]
		private IUnitEx caster;
		[NonSerialized]
		private IUnitEx[] targets;
		[NonSerialized]
		private ISpell spell;

		public string Caster { get; private set; }
		public string[] Targets { get; private set; }
		public string Spell { get; private set; }

		public HitResult[] HitRolls { get; private set; }
		public CoreStats? SpellSave { get; private set; }
		public int SpellDC { get; private set; }
		public DiceRoll[] SaveRolls { get; private set; }
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
					SaveBonus[i] = targets[i].Stats.Calculated[SpellSave] - 10;
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
						DiceRoll damageRoll = damageDice.Roll();

						// Calculate damage dealt (double damage for crit, half damage for successful save)
						DamageType type = spell.Energy == EnergyType.Force ? DamageType.Physical : DamageType.Magical;
						Damage damageDealt = CombatManager.CalculateDamage(caster, type, spell.Energy,
							(int)Math.Ceiling(damageRoll.Total
							* (HitRolls[i] != null && HitRolls[i].CritConfirm ? 2 : 1)
							/ (Saved[i] ? 2f : 1f)));

						// Apply damage to target (heals from Positive energy included)
						int damageTaken = CombatManager.ApplyDamage(targets[i], damageDealt);

						// Record damage results
						DamageDealt[i] = new DamageReport(caster, targets[i]);
						DamageDealt[i].AddDamage(damageRoll, damageDealt, damageTaken);
					}

					// Call spell-specific cast handlers
					AdditionalEffects[i] = spell.Cast(caster, targets[i], Saved[i]);
				}
			}
		}

		public SpellResult(IUnitEx caster, ISpell spell, params IUnitEx[] targets)
		{
			this.caster = caster;
			this.targets = targets;
			this.spell = spell;

			Caster = caster.Name;
			Spell = spell.Name;
			Targets = targets.Select<IUnitEx, string>(x => x.Name).ToArray<string>();

			HitRolls = new HitResult[Targets.Length];
			SpellSave = spell.Save;
			SpellDC = 10 + caster.Level;
			SaveRolls = new DiceRoll[Targets.Length];
			SaveBonus = new int[Targets.Length];
			Saved = new bool[Targets.Length];

			DamageDealt = new DamageReport[Targets.Length];
			AdditionalEffects = new ILoggable[Targets.Length + 1];

			Resolve();
		}

		string ILoggable.Inline
		{
			get
			{
				return String.Format("{0} casts {1}", Caster, Spell);
			}
		}

		// Consider prebuilding this string due to its complexity
		string ILoggable.Full
		{
			get
			{
				StringBuilder head = new StringBuilder();
				StringBuilder body = new StringBuilder();

				head.AppendFormat("{0} casts {1}", Caster, Spell);
				if (Targets.Length > 0)
				{
					head.Append(" on ");
					for (int i = 0; i < Targets.Length; i++)
					{
						head.AppendFormat(Targets[i]);
						if (i < Targets.Length - 1) head.Append(", ");

						if (HitRolls[i] != null)
							body.Append((HitRolls[i] as ILoggable).Full);

						if (SpellSave != null)
						{
							body.AppendLine(String.Format("{0} rolls a {1} ({2}{3}) and {4} the {5} save ({6}).",
								Targets[i], SaveRolls[i].Total + SaveBonus[i], SaveRolls[i].TotalString, SaveBonus[i].ToString(Log.ModifierFormat),
								Saved[i] ? "succeeds" : "fails", SpellSave, SpellDC));
						}

						if (DamageDealt[i] != null)
							body.Append((DamageDealt[i] as ILoggable).Full);

						if (AdditionalEffects[i] != null)
							body.Append(AdditionalEffects[i].Full);
					}
				}
				head.AppendLine(".");

				if (AdditionalEffects[Targets.Length] != null)
					body.Append(AdditionalEffects[Targets.Length].Full);

				return head.ToString() + body.ToString();
			}
		}
	}
}
