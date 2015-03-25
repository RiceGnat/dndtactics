using System;
using RPGEngine;

namespace DnDEngine.Combat
{
    public class InitiativeDetails : ILoggable
    {
        public string UnitName { get; private set; }
        public int Total
        {
            get
            {
                return Roll + Bonus;
            }
        }
        public int Roll { get; private set; }
        public int Bonus { get; private set; }

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

        public static InitiativeDetails Zero(IUnit unit)
        {
            return new InitiativeDetails(unit, 0, 0);
        }

        public InitiativeDetails(IUnit unit, int roll, int bonus)
        {
            UnitName = unit.Name;
            Roll = roll;
            Bonus = bonus;
        }

        public string Summary
        {
            get { return Full; }
        }

        public string Full
        {
            get { return String.Format("{0} rolls {1} (<{2}>{3}) for initiative", UnitName, Total, Roll, Bonus.ToString(Constants.ModifierFormat)); }
        }

        public override string ToString()
        {
            return Full;
        }
    }
}
