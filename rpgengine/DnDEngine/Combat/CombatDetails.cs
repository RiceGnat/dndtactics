using System;
using System.Linq;
using System.Text;
using RPGEngine;
using DnDEngine.Items;

namespace DnDEngine.Combat
{
    /// <summary>
    /// Contains details about a combat action.
    /// </summary>
    [Serializable]
    public class CombatDetails : ILoggable
    {
        /// <summary>
        /// Indicates how an attack missed.
        /// </summary>
        public enum MissedBy
        {
            /// <summary>
            /// Attack missed completely.
            /// </summary>
            Base,
            /// <summary>
            /// Attack couldn't penetrate defender's natural armor.
            /// </summary>
            NaturalArmor,
            /// <summary>
            /// Attack blocked by defender's armor.
            /// </summary>
            ArmorBonus,
            /// <summary>
            /// Defender evaded the attack.
            /// </summary>
            DexBonus
        }

        /// <summary>
        /// Gets the attacking unit's name.
        /// </summary>
        public string Attacker { get; private set; }
        /// <summary>
        /// Gets the defending unit's name.
        /// </summary>
        public string Defender { get; private set; }

        /// <summary>
        /// Gets a copy of the attacking weapon.
        /// </summary>
        public Equipment AttackWeapon { get; private set; }
        /// <summary>
        /// Gets the defender's AC.
        /// </summary>
        public short DefenderAC { get; private set; }

        /// <summary>
        /// Gets or sets the result of the hit die roll.
        /// </summary>
        public ushort HitRoll { get; set; }
        /// <summary>
        /// Gets or sets the attacker's hit modifier.
        /// </summary>
        public short HitMod { get; set; }
        /// <summary>
        /// Gets or sets whether or not the attack hit.
        /// </summary>
        public bool Hit { get; set; }

        /// <summary>
        /// Gets or sets whether or not the attack crit.
        /// </summary>
        public bool Crit { get; set; }
        /// <summary>
        /// Gets or sets the result of the crit confirm die roll.
        /// </summary>
        public ushort CritRoll { get; set; }
        /// <summary>
        /// Gets or sets whether or not the attacker confirmed crit.
        /// </summary>
        public bool CritConfirm { get; set; }

        /// <summary>
        /// Gets or sets what the attack missed by.
        /// </summary>
        public MissedBy Miss { get; set; }

        /// <summary>
        /// Gets or sets details about the damage dealt.
        /// </summary>
        public DamageDetails DamageDealt { get; set; }
        /// <summary>
        /// Gets or sets the actual damage taken by the defender.
        /// </summary>
        public int DamageTaken { get; set; }
        /// <summary>
        /// Gets or sets the defender's remaining HP after the attack.
        /// </summary>
        public short RemainingHP { get; set; }

        /// <summary>
        /// Initializes a new CombatDetails object with the attacker's and defender's names, the attacking weapon, and the defender's AC.
        /// Values are copied from the units and references are not stored.
        /// </summary>
        /// <param name="attacker">The attacking unit.</param>
        /// <param name="defender">The defending unit.</param>
        public CombatDetails(IUnit attacker, IUnit defender)
        {
            Attacker = attacker.Name;
            Defender = defender.Name;
            AttackWeapon = (attacker.Extensions as IEquipped).Weapon.Clone() ?? Equipment.Armory.GetNewEquipment(0);
            DefenderAC = defender.Stats.Get(DerivedStats.Type.AC);
        }

        /// <summary>
        /// Gets a short summary of the combat action.
        /// </summary>
        public string Summary
        {
            get
            {
                return String.Format("{0} attacks {1} with a {2}", Attacker, Defender, AttackWeapon.Name)
                    + String.Format(Hit ? " for {0} damage" : " and misses", DamageTaken);
            }
        }

        /// <summary>
        /// Gets a detailed breakdown of the combat action.
        /// </summary>
        public string Full
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(String.Format("{0} attacks {1} with a {2}", Attacker, Defender, AttackWeapon.Name));
                sb.AppendLine(String.Format("Defender {0} has an AC of {1}", Defender, DefenderAC));
                sb.AppendFormat("{0} rolls {1} (<{2}>{3}) and ", Attacker, HitRoll + HitMod, HitRoll, HitMod.ToString(Constants.ModifierFormat));
                if (Hit)
                {
                    sb.AppendLine("hit!");
                    if (Crit) sb.AppendLine(String.Format("{0} rolls {1} and {2}", Attacker, CritRoll, CritConfirm ? "confirms crit!" : "fails to confirm crit"));
					sb.AppendLine(String.Format("{0} rolls {1} ({2}), dealing {3} after bonus", Attacker, String.Join("+", DamageDealt.Rolls.Values.Select(value => value.ToString()).ToArray()), String.Join("+", DamageDealt.Breakdowns.Values.ToArray()), DamageDealt.Full));
                    sb.AppendFormat("{0} takes {1} damage after reductions ({2} HP remaining)", Defender, DamageTaken, RemainingHP);
                }
                else
                {
                    sb.AppendLine(String.Format("misses by {0}", Miss));
                    sb.AppendFormat("{0} takes no damage", Defender);
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// Gets the combat summary.
        /// </summary>
        /// <returns>Summary string</returns>
        public override string ToString()
        {
            return Summary;
        }
    }
}
