using System;
using RPGEngine;

namespace DnDEngine.Items
{
    public partial class Equipment
    {
        /// <summary>
        /// Defines various combat properties for weapons.
        /// </summary>
        [Serializable]
        public class WeaponProps
        {
            /// <summary>
            /// Defines different types of weapons.
            /// </summary>
            public enum Type
            {
                /// <summary>
                /// An unarmed strike. This is used for attacks with no equipped weapon.
                /// </summary>
                Unarmed,
                /// <summary>
                /// General use balanced swords.
                /// </summary>
                Longsword,
                /// <summary>
                /// Light swords that are easier to hit with.
                /// </summary>
                Shortsword,
                /// <summary>
                /// Finesse weapon with high critical hit ranges.
                /// </summary>
                Rapier,
                /// <summary>
                /// General use polearms.
                /// </summary>
                Spear,
                /// <summary>
                /// Heavy thrusting weapons. Minimum range required.
                /// </summary>
                Lance,
                /// <summary>
                /// Bladed polearms designed for slashing.
                /// </summary>
                Halberd,
                /// <summary>
                /// Heavy slashing weapons. Hard to hit with.
                /// </summary>
                Axe,
                /// <summary>
                /// Heavy smashing weapons. Hard to hit with.
                /// </summary>
                Club,
                /// <summary>
                /// Long-ranged projectile weapons.
                /// </summary>
                Longbow,
                /// <summary>
                /// Light one-handed projectile weapons.
                /// </summary>
                Crossbow
            }

            /// <summary>
            /// Gets the type of this weapon.
            /// </summary>
            public Type Category { get; protected set; }

            /// <summary>
            /// Gets the base damage die of this weapon.
            /// </summary>
            public Dice.Type Damage { get; protected set; }

            /// <summary>
            /// Gets the critical hit range of this weapon.
            /// </summary>
            public byte CritRange { get; protected set; }
            
            /// <summary>
            /// Gets the critical hit damage multiplier of this weapon.
            /// </summary>
            public byte CritMultiplier { get; protected set; }

            /// <summary>
            /// Clones this properties object.
            /// </summary>
            /// <returns>A deep clone of this object.</returns>
            public WeaponProps Clone()
            {
                return this.MemberwiseClone() as WeaponProps;
            }

            /// <summary>
            /// Initializes a new WeaponProps object with the specified values.
            /// </summary>
            /// <param name="category">Weapon type</param>
            /// <param name="damage">Base damage die</param>
            /// <param name="critRange">Critical hit range</param>
            /// <param name="critMultiplier">Critical hit damage multiplier</param>
            public WeaponProps(Type category, Dice.Type damage, byte critRange, byte critMultiplier)
            {
                Category = category;
                Damage = damage;
                CritRange = critRange;
                CritMultiplier = critMultiplier;
            }

            /// <summary>
            /// Builder class for WeaponProps. Used with Equipment.Builder.
            /// </summary>
            public class Builder
            {
                /// <summary>
                /// Gets or sets the type for the new weapon.
                /// </summary>
                public Type Category { get; set; }

                /// <summary>
                /// Gets or sets the base damage die for the new weapon.
                /// </summary>
                public Dice.Type Damage { get; set; }

                /// <summary>
                /// Gets or sets the critical hit range for the new weapon.
                /// </summary>
                public byte CritRange { get; set; }

                /// <summary>
                /// Gets or sets the critical hit damage multipler for the new weapon.
                /// </summary>
                public byte CritMultiplier { get; set; }

                /// <summary>
                /// Creates a new WeaponProps object with the set properties.
                /// </summary>
                /// <returns></returns>
                public WeaponProps Build()
                {
                    return new WeaponProps(Category, Damage, CritRange, CritMultiplier);
                }
            }
        }
    }
}
