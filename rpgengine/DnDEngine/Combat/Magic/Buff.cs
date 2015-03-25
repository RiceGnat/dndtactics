using System;
using System.Text;
using RPGEngine;

namespace DnDEngine.Combat.Magic
{
    /// <summary>
    /// Represents a buff or debuff.
    /// </summary>
    [Serializable]
    public class Buff : UnitModifier
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }

        public ushort Duration { get; protected set; }
        public int Remaining { get; protected set; }

        public Buff Clone()
        {
            return this.MemberwiseClone() as Buff;
        }

        public virtual ILoggable Upkeep(CombatEntity entity)
        {
            if (Duration > 0) Remaining--;
            return null;
        }

        #region Constructors
        private Buff()
        {
            coreAdds = new CoreStats();
            coreMults = new CoreStats();
            derivedAdds = new DerivedStats();
            derivedMults = new DerivedStats();
        }

        protected Buff(ushort duration)
            : this()
        {
            Duration = duration;
            Remaining = Duration > 0 ? Duration : 1;
        }

        public Buff(string name, string desc)
            : this()
        {
            Name = name;
            Description = desc;
        }
        #endregion

        public override string ToString()
        {
            return String.Format("{0} ({1})", Name, Description);
        }
    }

    [Serializable]
    public class AttackBuff : Buff
    {
        public AttackBuff(short value, ushort duration = 0)
            : base(duration)
        {
            Name = "Attack Buff";
            Description = String.Format("Grants +{0}% ATK", value);
            derivedMults.ATK = value;
        }
    }

    [Serializable]
    public class DefenseBuff : Buff
    {
        public DefenseBuff(short value, ushort duration = 0)
            : base(duration)
        {
            Name = "Defense Buff";
            Description = String.Format("Grants +{0}% DEF", value);
            derivedMults.DEF = value;
        }
    }

    [Serializable]
    public class Poisoned : Buff
    {
        private short value;
        private ushort dc;

        public Poisoned(short value, ushort dc = 10)
            : base(0)
        {
            Name = "Poisoned";
            Description = String.Format("Each turn DC {0} Fortitude save to recover or lose {1} HP", dc, value);
            this.value = value;
            this.dc = dc;
        }

        public override ILoggable Upkeep(CombatEntity entity)
        {
            base.Upkeep(entity);

            StringBuilder sb = new StringBuilder();

            if (Remaining > 0)
            {
                Dice d20 = new Dice(Dice.Type.D20);
                int save = entity.Stats[CoreStats.Type.CON] - 10;
                int roll = d20.Roll() + save;
                sb.AppendFormat("{0} rolls {1} (<{2}>{3}), ", entity.Name, roll, d20.Result, save.ToString(Constants.ModifierFormat));
                if (roll >= dc)
                {
                    Remaining = 0;
                    sb.AppendLine("succeeding the save");
                }
                else
                {
                    sb.AppendLine("failing the save");
                }
            }

            if (Remaining > 0)
            {
                entity.CurrentHP -= value;
                sb.AppendFormat("{0} loses {1} HP to the poison", entity.Name, value);
            }
            else
            {
                sb.AppendFormat("{0} recovers from the poison", entity.Name);
            }

            return new LoggableString(sb.ToString());
        }
    }

    [Serializable]
    public class Burning : Buff
    {
        private Dice.Type dice;
        private ushort num;

        public Burning(Dice.Type dice = Dice.Type.D4, ushort numDice = 2, ushort duration = 0)
            : base(duration)
        {
            Name = "Burning";
            Description = String.Format("Takes {0}{1} fire damage each turn", numDice, dice.ToString().ToLower());
            this.dice = dice;
            this.num = numDice;
        }

        public override ILoggable Upkeep(CombatEntity entity)
        {
            base.Upkeep(entity);

            Dice d = new Dice(num, dice);
            d.Roll();
            StringBuilder sb = new StringBuilder(String.Format("{0} rolls <{1}>, taking {2} [Fire] damage", entity.Name, d.ToString(), CombatManager.ApplyDamage(entity, new DamageDetails(d.Result, d.ToString(), d.Result))));

            if (Remaining <= 0)
            {
                sb.AppendLine();
                sb.AppendFormat("{0} is no longer on fire", entity.Name);
            }

            return new LoggableString(sb.ToString());
        }
    }
}
