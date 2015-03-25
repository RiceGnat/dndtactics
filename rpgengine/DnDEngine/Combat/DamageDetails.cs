using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPGEngine;
using DnDEngine.Combat.Magic;

namespace DnDEngine.Combat
{
    [Serializable]
    public class DamageDetails : ILoggable
    {
        public IDictionary<Spell.EnergyType, int> Rolls { get; private set; }
        public IDictionary<Spell.EnergyType, string> Breakdowns { get; private set; }
        public IDictionary<Spell.EnergyType, int> Damages { get; private set; }

        public int Total
        {
            get
            {
                return Damages.Values.Sum();
            }
        }

        public void AddRoll(int roll, string breakdown, Spell.EnergyType energy = Spell.EnergyType.None)
        {
            Add(roll, breakdown, 0, energy);
        }

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

        public int this[Spell.EnergyType energy]
        {
            get { return Damages.ContainsKey(energy) ? Damages[energy] : 0; }
        }

        public DamageDetails()
        {
            Rolls = new Dictionary<Spell.EnergyType, int>();
            Breakdowns = new Dictionary<Spell.EnergyType, string>();
            Damages = new Dictionary<Spell.EnergyType, int>();
        }

        public DamageDetails(int roll, string breakdown, int damage, Spell.EnergyType energy = Spell.EnergyType.None)
            : this()
        {
            Add(roll, breakdown, damage, energy);
        }

        public string Summary
        {
            get { return Total + " damage"; }
        }

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

        public override string ToString()
        {
            return Summary;
        }
    }
}
