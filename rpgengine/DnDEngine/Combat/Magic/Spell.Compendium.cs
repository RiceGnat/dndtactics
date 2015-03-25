using System.Collections.Generic;
using RPGEngine;

namespace DnDEngine.Combat.Magic
{
    public abstract partial class Spell
    {
        public static class Compendium
        {
            private static List<Spell> spells = new List<Spell>();

            private static uint NextID { get { return (uint)spells.Count; } }

            private static void Register(Spell spell)
            {
                spell.ID = NextID;
                spells.Add(spell);
            }

            public static Spell GetSpell(uint id)
            {
                return spells[(int)id];
            }

            #region Spells
            private sealed class LightingBolt : Spell
            {
                public LightingBolt()
                {
                    Name = "Lightning Bolt";
                    Description = "Lightning Bolt deals 3 damage to target creature or player";
                    Magic = MagicType.Arcane;
                    School = SchoolType.Evocation;
                    CastMethod = CastType.Line;
                    Save = SaveType.Reflex;

                    DamageDie = Dice.Type.D8;
                    Energy = EnergyType.Electricity;
                }
            }
            #endregion

            static Compendium()
            {
                Register(new LightingBolt());
            }
        }
    }
}
