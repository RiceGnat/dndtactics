using System;
using System.Collections.Generic;
using RPGEngine;
using DnDEngine.Combat.Magic;

namespace DnDEngine.Combat
{
    /// <summary>
    /// Contains methods for carrying out combat actions
    /// </summary>
    public static class CombatManager
    {
        /// <summary>
        /// Roll for initiative
        /// </summary>
        /// <param name="unit">Unit</param>
        /// <returns>Result of initiative roll</returns>
        public static InitiativeDetails RollInitiative(IUnit unit)
        {
            return new InitiativeDetails(unit, new Dice(Dice.Type.D20).Roll(), unit.Stats.Get(CoreStats.Type.DEX) - 10);
        }

        /// <summary>
        /// Calculate attacker's damage with a given roll after damage bonuses
        /// </summary>
        /// <param name="attacker">Attacking unit</param>
        /// <param name="roll">Result of dice roll</param>
		/// <param name="stat">(optional) The stat to use for scaling (passing null will default to DerivedStats.Type.ATK)</param>
        /// <returns>Damage after bonuses</returns>
        public static int CalculateDamage(IUnit attacker, int roll, Enum stat = null)
        {
            IStats stats = attacker.Stats;
            stat = stat ?? DerivedStats.Type.ATK;
            return stats.Get(stat) < 0
                   ? (int)Math.Ceiling(roll / (Math.Abs(stats.Get(stat)) / 100f + 1))
                   : roll + roll * stats.Get(stat) / 100;
        }

        /// <summary>
        /// Calculate damage after applying the defender's damage reductions
        /// </summary>
        /// <param name="defender">Defending unit</param>
        /// <param name="damage">Incoming damage</param>
		/// <param name="multiplier">(optional) Multiplies the final damage value</param>
        /// <returns>Actual total damage value</returns>
        public static int ApplyDamage(CombatEntity defender, DamageDetails damage, float multiplier = 1)
        {
            IStats stats = defender.Stats;

            int value, actual = 0;

            foreach (Spell.EnergyType type in Enum.GetValues(typeof(Spell.EnergyType)))
            {
                value = damage[type];
                Enum stat = type == Spell.EnergyType.None ? DerivedStats.Type.DEF : DerivedStats.Type.RES;

                actual += (int)((stats.Get(stat) >= 0
                       ? (int)Math.Ceiling(value / (stats.Get(stat) / 100f + 1))
                       : value - value * stats.Get(stat) / 100) * multiplier);
            }

            defender.CurrentHP -= (short)actual;
            return actual;
        }

        /// <summary>
        /// Make an attack
        /// </summary>
        /// <param name="attacker">Attacking unit</param>
        /// <param name="defender">Defending unit</param>
        /// <returns>Results of combat</returns>
        public static ILoggable RollAttack(CombatEntity attacker, CombatEntity defender)
        {
            CombatDetails result = new CombatDetails(attacker, defender);

            // Roll a d20 to hit
            Dice d20 = new Dice(Dice.Type.D20);
            result.HitRoll = d20.Roll();
            result.HitMod = attacker.Stats.Get(DerivedStats.Type.HIT);

            int roll = result.HitRoll + result.HitMod;

            // Hit on natural 20 or match defender's AC
            if (result.HitRoll == 20 || roll >= result.DefenderAC)
            {
                result.Hit = true;
            }
            else
            {
                // Miss
                result.Hit = false;
                if (roll < defender.Stats.Get(CoreStats.Type.BaseAC))
                    result.Miss = CombatDetails.MissedBy.Base;
                else if (roll < defender.Stats.Get(DerivedStats.Type.AC_Natural))
                    result.Miss = CombatDetails.MissedBy.NaturalArmor;
                else if (roll < defender.Stats.Get(DerivedStats.Type.AC_Flat))
                    result.Miss = CombatDetails.MissedBy.ArmorBonus;
                else
                    result.Miss = CombatDetails.MissedBy.DexBonus;
            }

            if (result.Hit)
            {
                // Check for crit
                if (result.HitRoll >= result.AttackWeapon.WeaponProperties.CritRange)
                {
                    result.Crit = true;
                    result.CritRoll = d20.Roll();

                    // Confirm crit
                    result.CritConfirm = result.CritRoll >= result.AttackWeapon.WeaponProperties.CritRange;
                }

                // Roll for damage
                Dice dice = new Dice(Math.Max(1, attacker.Stats.Get(CoreStats.Type.STR) - 10), result.AttackWeapon.WeaponProperties.Damage);
                result.DamageDealt = new DamageDetails(dice.Roll(), dice.ToString(), CalculateDamage(attacker, dice.Result) * (result.CritConfirm ? result.AttackWeapon.WeaponProperties.CritMultiplier : 1));
                result.DamageTaken = ApplyDamage(defender, result.DamageDealt);
                result.RemainingHP = defender.CurrentHP;
            }

            return result;
        }

        /// <summary>
        /// Cast a spell
        /// </summary>
        /// <param name="spellId"></param>
        /// <param name="caster"></param>
        /// <param name="targets"></param>
        public static ILoggable CastSpell(uint spellId, CombatEntity caster, IEnumerable<CombatEntity> targets)
        {
            return Spell.Compendium.GetSpell(spellId).Cast(caster, targets);
        }
    }
}
