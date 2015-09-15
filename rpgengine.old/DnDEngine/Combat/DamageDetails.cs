using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPGEngine;
using DnDEngine.Combat.Magic;

namespace DnDEngine.Combat
{
    /// <summary>
    /// Contains detailed damage values organized by energy type.
    /// </summary>
    [Serializable]
    public class DamageDetails : ILoggable
    {
        /// <summary>
        /// Gets the die roll totals organized by energy type.
        /// </summary>
        public IDictionary<Spell.EnergyType, int> Rolls { get; private set; }

        /// <summary>
        /// Gets the die roll breakdown strings organized by energy type.
        /// </summary>
        public IDictionary<Spell.EnergyType, string> Breakdowns { get; private set; }

        /// <summary>
        /// Gets the total damage values after bonus organized by energy type.
        /// </summary>
        public IDictionary<Spell.EnergyType, int> Damages { get; private set; }

        /// <summary>
        /// Gets the total damage value.
        /// </summary>
        public int Total
        {
            get
            {
                return Damages.Values.Sum();
            }
        }

        /// <summary>
        /// Adds a die roll only.
        /// </summary>
        /// <param name="roll">Die roll total</param>
        /// <param name="breakdown">Die roll breakdown string</param>
        /// <param name="energy">(optional) Energy type</param>
        public void AddRoll(int roll, string breakdown, Spell.EnergyType energy = Spell.EnergyType.None)
        {
            Add(roll, breakdown, 0, energy);
        }

        /// <summary>
        /// Adds a die roll and calculated damage value.
        /// </summary>
        /// <param name="roll">Die roll total</param>
        /// <param name="breakdown">Die roll breakdown string</param>
        /// <param name="damage">Calculated damage value</param>
        /// <param name="energy">(optional) Energy type</param>
        public void Add(int roll, string breakdown, int damage, Spell.EnergyType energy = Spell.EnergyType.None)
        {
            if (Damages.ContainsKey(energy))
            {
                Rolls[energy] += roll;
                Breakdowns[energy] += String.Format("+<{0}>", breakdown);
                Damages[energy] += damage;
            }
            else
            {
                Rolls.Add(energy, roll);
                Breakdowns.Add(energy, String.Format("<{0}>", breakdown));
                Damages.Add(energy, damage);
            }
        }

        /// <summary>
        /// Gets the damage total for the given energy type.
        /// </summary>
        /// <param name="energy">Energy type</param>
        /// <returns>The total damage after bonus for the given energy type</returns>
        public int this[Spell.EnergyType energy]
        {
            get { return Damages.ContainsKey(energy) ? Damages[energy] : 0; }
        }

        /// <summary>
        /// Initializes an empty DamageDetails object.
        /// </summary>
        public DamageDetails()
        {
            Rolls = new Dictionary<Spell.EnergyType, int>();
            Breakdowns = new Dictionary<Spell.EnergyType, string>();
            Damages = new Dictionary<Spell.EnergyType, int>();
        }

        /// <summary>
        /// Initializes a DamageDetails object with the given values.
        /// </summary>
        /// <param name="roll">Die roll total</param>
        /// <param name="breakdown">Die roll breakdown string</param>
        /// <param name="damage">Calculated damage value</param>
        /// <param name="energy">(optional) Energy type</param>
        public DamageDetails(int roll, string breakdown, int damage, Spell.EnergyType energy = Spell.EnergyType.None)
            : this()
        {
            Add(roll, breakdown, damage, energy);
        }

        /// <summary>
        /// Gets a short summary of the damage.
        /// </summary>
        public string Summary
        {
            get { return Total + " damage"; }
        }

        /// <summary>
        /// Gets a full detailed description of the damage values.
        /// </summary>
        public string Full
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                int count = 0;
                foreach (Spell.EnergyType type in Damages.Keys)
                {
                    sb.AppendFormat(type == Spell.EnergyType.None ? "{0} Physical" : "{0} [{1}]", Damages[type], type);
                    count++;
                    if (count < Damages.Count) sb.Append(" + ");
                }

                if (Damages.Count == 1)
                {
                    return String.Format("{0} damage", sb.ToString());
                }
                else
                {
                    return String.Format("{0} damage ({1})", Total, sb.ToString());
                }
            }
        }

        /// <summary>
        /// Gets the damage summary.
        /// </summary>
        /// <returns>Summary string</returns>
        public override string ToString()
        {
            return Summary;
        }
    }
}
