using System;
using System.Collections.Generic;
using RPGEngine;
using DnDEngine.Combat.Magic;

namespace DnDEngine.Items
{
    public partial class Equipment
    {
        public static class Armory
        {
            private static IList<Equipment> equipmentList = new List<Equipment>();

            public static uint NextID { get { return (uint)equipmentList.Count; } }

            public static void Register(Equipment equipment)
            {
                equipment.ID = NextID;
                equipmentList.Add(equipment);
            }

            public static Equipment GetNewEquipment(uint id)
            {
                try
                {
                    return equipmentList[(int)id].Clone();
                }
                catch (IndexOutOfRangeException ex)
                {
                    throw new IndexOutOfRangeException("The requested equipment ID is not registered", ex);
                }
            }

            static Armory()
            {
                Equipment.Builder e = new Equipment.Builder();

                #region Weapons
                var w = new Equipment.WeaponProps.Builder();
                e.Slot = Type.Weapon;
                e.WeaponProperties = w;

                w.Category = WeaponProps.Type.Unarmed;
                e.Name = "Unarmed strike";
                e.Description = "No weapon equipped";
                e.CoreStatMods = new CoreStats(0, 0, 0, 0, 0, 0, 0);
                e.DerivedStatMods = new DerivedStats(0, 0, 0, 0, 0, 0, 0, 0);
                w.Damage = Dice.Type.D4;
                w.CritRange = 20;
                w.CritMultiplier = 2;
                Register(e.Build());

                w.Category = WeaponProps.Type.Shortsword;
                e.Name = "Shortsword";
                e.Description = "A basic shortsword";
                e.CoreStatMods = new CoreStats(0, 0, 0, 0, 0, 0, 0);
                e.DerivedStatMods = new DerivedStats(0, 0, 2, 0, 0, 0, 0, 0);
                w.Damage = Dice.Type.D6;
                w.CritRange = 19;
                w.CritMultiplier = 2;
                Register(e.Build());

                w.Category = WeaponProps.Type.Longsword;
                e.Name = "Longsword";
                e.Description = "A basic longsword";
                e.CoreStatMods = new CoreStats(0, 0, 0, 0, 0, 0, 0);
                e.DerivedStatMods = new DerivedStats(0, 0, 1, 0, 1, 0, 0, 0);
                w.Damage = Dice.Type.D6;
                w.CritRange = 19;
                w.CritMultiplier = 2;
                Register(e.Build());

                w.Category = WeaponProps.Type.Spear;
                e.Name = "Spear";
                e.Description = "A basic spear";
                e.CoreStatMods = new CoreStats(0, 0, 0, 0, 0, 0, 0);
                e.DerivedStatMods = new DerivedStats(0, 0, 0, 0, 2, 0, 0, 0);
                w.Damage = Dice.Type.D8;
                w.CritRange = 19;
                w.CritMultiplier = 3;
				Register(e.Build());

				w.Category = WeaponProps.Type.Club;
				e.Name = "Morning Star";
				e.Description = "A spiked bludgeoning weapon";
				e.CoreStatMods = new CoreStats(0, 0, 0, 0, 0, 0, 0);
				e.DerivedStatMods = new DerivedStats(0, 0, -2, 0, 4, 0, 0, 0);
				w.Damage = Dice.Type.D8;
				w.CritRange = 19;
				w.CritMultiplier = 2;
				Register(e.Build());
                #endregion

                #region Body
                e.Slot = Type.Body;

                e.Name = "Test equipment";
                e.Description = "Just a collection of stats";
                e.CoreStatMods = new CoreStats(0, 3, 0, 2, 5, 0);
                e.DerivedStatMods = new DerivedStats(0, 0, 3, 1, 5, 0, 5, 0);
                e.GrantedBuffs.Add(new DefenseBuff(50));
                Register(e.Build());

                e.Name = "Plate armor";
                e.Description = "Heavy armor";
                e.CoreStatMods = new CoreStats(0, 0, 0, 0, 0, 0);
                e.DerivedStatMods = new DerivedStats(0, 0, -2, 8, 0, 25, 0, 0);
                Register(e.Build());
                #endregion
            }
        }
    }
}
