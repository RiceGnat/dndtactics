using System;
using RPGEngine;

namespace DnDEngine.Combat
{
    /// <summary>
    /// Contains details about an initiative roll.
    /// </summary>
    public class InitiativeDetails : ILoggable
    {
        /// <summary>
        /// Gets the name of the unit.
        /// </summary>
        public string UnitName { get; private set; }

        /// <summary>
        /// Gets the total initiative value after bonus.
        /// </summary>
        public int Total
        {
            get
            {
                return Roll + Bonus;
            }
        }

        /// <summary>
        /// Gets the die roll value.
        /// </summary>
        public int Roll { get; private set; }

        /// <summary>
        /// Gets the unit's bonus to initiative.
        /// </summary>
        public int Bonus { get; private set; }

        /// <summary>
        /// Gets a randomly generated value for initiative tiebreakers. Used when sorting by initiative.
        /// </summary>
        public static int Tiebreaker
        {
            get
            {
                int val = 0;
                Dice d = new Dice(Dice.Type.D20);
                while (val != 0)
                {
                    val = d.Roll().CompareTo(d.Roll());
                }
                return val;
            }
        }

        /// <summary>
        /// Gets a zero-value initiative.
        /// </summary>
        /// <param name="unit">(optional) The unit whose name will be used</param>
        /// <returns>A new zero-valued InitiativeDetails object</returns>
        public static InitiativeDetails Zero(IUnit unit = null)
        {
            return new InitiativeDetails(unit, 0, 0);
        }

        /// <summary>
        /// Creates a new InititativeDetails object with the provided values.
        /// </summary>
        /// <param name="unit">The rolling unit</param>
        /// <param name="roll">The die roll value</param>
        /// <param name="bonus">The unit's bonus to initiative</param>
        public InitiativeDetails(IUnit unit, int roll, int bonus)
        {
            if (unit != null) UnitName = unit.Name;
            Roll = roll;
            Bonus = bonus;
        }

        /// <summary>
        /// Gets a description of the initiative roll (values only).
        /// </summary>
        public string Summary
        {
            get { return String.Format("{0} (<{1}>{2})", Total, Roll, Bonus.ToString(Constants.ModifierFormat)); }
        }

        /// <summary>
        /// Gets a description of the initiative roll.
        /// </summary>
        public string Full
        {
            get { return String.Format("{0} rolls {1} for initiative", UnitName, Summary); }
        }

        /// <summary>
        /// Gets the initiative summary.
        /// </summary>
        /// <returns>Summary string</returns>
        public override string ToString()
        {
            return Full;
        }
    }
}
