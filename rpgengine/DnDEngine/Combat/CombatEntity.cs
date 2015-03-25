using System;
using System.Text;
using RPGEngine;

namespace DnDEngine.Combat
{
    /// <summary>
    /// Wraps an IUnit object and adds volatile stats for use in battle. May also be transparently used as an IUnit.
    /// </summary>
    public class CombatEntity : IUnit, ILoggable
    {
        private IUnit unit;

        #region IUnit
        /// <summary>
        /// Gets the unit's name.
        /// </summary>
        public string Name { get { return unit.Name; } }
        /// <summary>
        /// Gets the unit's class.
        /// </summary>
        public string Class { get { return unit.Class; } }
        /// <summary>
        /// Gets the unit's level.
        /// </summary>
        public ushort Level { get { return unit.Level; } }

        /// <summary>
        /// Gets this unit's base stats (without any modifiers).
        /// </summary>
        public IStats BaseStats { get { return unit.BaseStats; } }
        /// <summary>
        /// Gets this unit's stats.
        /// </summary>
        public IStats Stats { get { return unit.Stats; } }
        /// <summary>
        /// Gets the total additive modifiers applied to this unit's stats.
        /// </summary>
        public IStats StatModsAdditive { get { return unit.StatModsAdditive; } }
        /// <summary>
        /// Gets the total multiplicative modifiers applied to this unit's stats.
        /// </summary>
        public IStats StatModsMultiplicative { get { return unit.StatModsMultiplicative; } }

        /// <summary>
        /// Gets a DnDUnit.UnitExtensions object containing additional properties or methods. Cast to supported interfaces to access different properties.
        /// </summary>
        public object Extensions { get { return unit.Extensions; } }

        void IUnit.Evaluate() { unit.Evaluate(); }
        #endregion

        #region Expose extension interfaces for convenience
        /// <summary>
        /// Gets additional details about this unit
        /// </summary>
        public IDetails Details { get { return unit.Extensions as IDetails; } }
        /// <summary>
        /// Gets this unit's inventory.
        /// </summary>
        public IInventory Inventory { get { return unit.Extensions as IInventory; } }
        /// <summary>
        /// Gets this unit's equipment
        /// </summary>
        public IEquipped Equipped { get { return unit.Extensions as IEquipped; } }
        /// <summary>
        /// Gets the buffs and debuffs applied to this unit
        /// </summary>
        public IBuffs Buffs { get { return unit.Extensions as IBuffs; } }
        /// <summary>
        /// Gets this unit's known spells
        /// </summary>
        public ISpells Spells { get { return unit.Extensions as ISpells; } }
        #endregion

        /// <summary>
        /// Gets or sets the ID of the team this unit is on.
        /// </summary>
        public int TeamID { get; set; }

        /// <summary>
        /// Gets whether or not this unit can take actions.
        /// </summary>
        public bool CanAct { get; private set; }
        
        /// <summary>
        /// Gets or sets and object containing this unit's initiative roll and modifier
        /// </summary>
        public InitiativeDetails Initiative { get; set; }

        private short currentHP;
        /// <summary>
        /// Gets or sets this unit's current HP. Clamped between 0 and the unit's max HP stat.
        /// </summary>
        public short CurrentHP
        {
            get { return currentHP; }
            set { currentHP = Math.Max((short)0, Math.Min(value, Stats[DerivedStats.Type.HP])); }
        }

        private short currentMP;
        /// <summary>
        /// Gets or sets this unit's current MP. Clamped between 0 and the unit's max MP stat.
        /// </summary>
        public short CurrentMP
        {
            get { return currentMP; }
            set { currentMP = Math.Max((short)0, Math.Min(value, Stats[DerivedStats.Type.MP])); }
        }

        /// <summary>
        /// Gets if this unit is alive.
        /// </summary>
        public bool IsAlive { get { return currentHP > 0; } }

        /// <summary>
        /// Gets or sets the experience that this unit has gained.
        /// </summary>
        public int ExpGained { get; set; }

        /// <summary>
        /// Set this unit's volatile stats to their initial states.
        /// </summary>
        /// <returns></returns>
        public CombatEntity Initialize()
        {
            unit.Evaluate();
            CurrentHP = unit.Stats[DerivedStats.Type.HP];
            CurrentMP = unit.Stats[DerivedStats.Type.MP];
            ExpGained = 0;

            return this;
        }

        /// <summary>
        /// Creates a new CombatEntity instance for a given unit.
        /// </summary>
        /// <param name="unit">The IUnit interface that the CombatEntity will encapsulate.</param>
        /// <param name="canAct">(optional) Whether or not this unit can take actions.</param>
        public CombatEntity(IUnit unit, bool canAct = true)
        {
            this.unit = unit;
            CanAct = canAct;
        }

        /// <summary>
        /// Compare two units by initiative.
        /// </summary>
        public static int Compare(CombatEntity a, CombatEntity b)
        {
            if (b.Initiative == null) return -1;
            else if (a.Initiative == null) return 1;
            else return a.Initiative.Total < b.Initiative.Total ? 1 : (a.Initiative.Total > b.Initiative.Total ? -1 : (a.Initiative.Bonus < b.Initiative.Bonus ? 1 : (a.Initiative.Bonus > b.Initiative.Bonus ? -1 : InitiativeDetails.Tiebreaker)));
        }

        /// <summary>
        /// Gets a short description of this unit.
        /// </summary>
        public string Summary
        {
            get
            {
                var gender = (unit.Extensions as IDetails).Gender;
                return String.Format("{0} (Level {1} {2}{3})", unit.Name, unit.Level, gender != DnDUnit.GenderType.None ? gender + " " : "", unit.Class);
            }
        }

        /// <summary>
        /// Gets a full description of this unit.
        /// </summary>
        public string Full
        {
            get
            {
                StringBuilder full = new StringBuilder(Summary);
                full.AppendLine();
                full.AppendFormat("HP {0}/{1}\tMP {2}/{3}",
                    currentHP, unit.Stats[DerivedStats.Type.HP],
                    currentMP, unit.Stats[DerivedStats.Type.MP]);
                full.AppendLine();
                full.AppendFormat("HIT {0}\tAC {1}\tMOV {2}",
                    unit.Stats[DerivedStats.Type.HIT].ToString(Constants.ModifierFormat),
                    unit.Stats[DerivedStats.Type.AC],
                    unit.Stats[CoreStats.Type.MOV]);
                full.AppendLine();
                full.AppendFormat("STR {0}\tCON {1}\tDEX {2}\t", unit.Stats[CoreStats.Type.STR], unit.Stats[CoreStats.Type.CON], unit.Stats[CoreStats.Type.DEX]);
                full.AppendFormat("INT {0}\tWIS {1}\tCHA {2}", unit.Stats[CoreStats.Type.INT], unit.Stats[CoreStats.Type.WIS], unit.Stats[CoreStats.Type.CHA]);
                full.AppendLine();
                full.AppendFormat("ATK {0}\tDEF {1}\t",
                    unit.Stats[DerivedStats.Type.ATK].ToString(Constants.ModifierFormat),
                    unit.Stats[DerivedStats.Type.DEF].ToString(Constants.ModifierFormat));
                full.AppendFormat("MAG {0}\tRES {1}",
                    unit.Stats[DerivedStats.Type.MAG].ToString(Constants.ModifierFormat),
                    unit.Stats[DerivedStats.Type.RES].ToString(Constants.ModifierFormat));

                IBuffs buffs = unit.Extensions as IBuffs;
                if (buffs.Count > 0)
                {
                    full.AppendLine();
                    full.AppendLine();
                    full.Append("Buffs/Debuffs");
                    foreach (var b in buffs.All)
                    {
                        full.AppendLine();
                        full.AppendFormat("\t{0}", b.ToString());
                        if (b.Duration > 0) full.AppendFormat(" ({0} turns remaining)", b.Remaining);
                    }
                }
                return full.ToString();
            }
        }
    }
}
