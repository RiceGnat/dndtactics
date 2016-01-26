using System;
using System.Text;
using DnDEngine.Logging;

namespace DnDEngine.Combat
{
	[Serializable]
	public class AttackResult : ILoggable
	{
		[NonSerialized]
		private IUnitEx attacker;
		[NonSerialized]
		private IUnitEx defender;
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
				Damage damageDealt = CombatManager.CalculateDamage(attacker, DamageType.Physical, null, rollTotal * (HitRoll.CritConfirm ? weapon.CritMultiplier : 1));

				// Apply damage to defender
				int damageTaken = CombatManager.ApplyDamage(defender, damageDealt);

				// Record damage results
				DamageDealt.AddDamage(damage.Result, damageDealt, damageTaken);

				// Damage from weapon enchantments (non-scaled?)
			}
		}

		public AttackResult(IUnitEx attacker, IUnitEx defender, IWeapon weapon)
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
}
