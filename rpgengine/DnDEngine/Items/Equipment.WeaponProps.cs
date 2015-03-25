using System;
using RPGEngine;

namespace DnDEngine.Items
{
    public partial class Equipment
    {
        [Serializable]
        public class WeaponProps
        {
            public enum Type
            {
                Unarmed,
                Longsword,
                Shortsword,
                Rapier,
                Spear,
                Lance,
                Halberd,
                Axe,
                Club
            }

            public Type Category { get; protected set; }
            public Dice.Type Damage { get; protected set; }

            public byte CritRange { get; protected set; }
            public byte CritMultiplier { get; protected set; }

            public WeaponProps Clone()
            {
                return this.MemberwiseClone() as WeaponProps;
            }

            public WeaponProps(Type category, Dice.Type damage, byte critRange, byte critMultiplier)
            {
                Category = category;
                Damage = damage;
                CritRange = critRange;
                CritMultiplier = critMultiplier;
            }

            public class Builder
            {
                public Type Category { get; set; }
                public Dice.Type Damage { get; set; }

                public byte CritRange { get; set; }
                public byte CritMultiplier { get; set; }

                public WeaponProps Build()
                {
                    return new WeaponProps(Category, Damage, CritRange, CritMultiplier);
                }
            }
        }
    }
}
