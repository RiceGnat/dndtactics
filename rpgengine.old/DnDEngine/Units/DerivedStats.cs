using System;
using System.Collections.Generic;
using RPGEngine;

namespace DnDEngine
{
    /// <summary>
    /// Represents the stats that are calculated from the core stats.
    /// </summary>
    [Serializable]
    public class DerivedStats : Stats
    {
        /// <summary>
        /// Defines a unit's derived stats.
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// Health points
            /// </summary>
            HP,
            /// <summary>
            /// Mana points
            /// </summary>
            MP,
            /// <summary>
            /// Hit modifier
            /// </summary>
            HIT,
            /// <summary>
            /// Armor class with natural armor only
            /// </summary>
            AC_Natural,
            /// <summary>
            /// Flat-footed armor class (no DEX bonus)
            /// </summary>
            AC_Flat,
            /// <summary>
            /// Touch armor class (no armor bonus)
            /// </summary>
            AC_Touch,
            /// <summary>
            /// Armor class (hit evasion)
            /// </summary>
            AC,
            /// <summary>
            /// Physical damage bonus
            /// </summary>
            ATK,
            /// <summary>
            /// Physical damage resistance
            /// </summary>
            DEF,
            /// <summary>
            /// Magical damage bonus
            /// </summary>
            MAG,
            /// <summary>
            /// Magical damage bonus
            /// </summary>
            RES
        }

        #region Convenience mapping
        public short HP
        {
            get { return Get(Type.HP); }
            set { Set(Type.HP, value); }
        }
        public short MP
        {
            get { return Get(Type.MP); }
            set { Set(Type.MP, value); }
        }
        public short HIT
        {
            get { return Get(Type.HIT); }
            set { Set(Type.HIT, value); }
        }
        public short AC_Natural
        {
            get { return Get(Type.AC_Natural); }
            set { Set(Type.AC_Natural, value); }
        }
        public short AC_Flat
        {
            get { return Get(Type.AC_Flat); }
            set { Set(Type.AC_Flat, value); }
        }
        public short AC_Touch
        {
            get { return Get(Type.AC_Touch); }
            set { Set(Type.AC_Touch, value); }
        }
        public short AC
        {
            get { return Get(Type.AC); }
            set { Set(Type.AC, value); }
        }
        public short ATK
        {
            get { return Get(Type.ATK); }
            set { Set(Type.ATK, value); }
        }
        public short DEF
        {
            get { return Get(Type.DEF); }
            set { Set(Type.DEF, value); }
        }
        public short MAG
        {
            get { return Get(Type.MAG); }
            set { Set(Type.MAG, value); }
        }
        public short RES
        {
            get { return Get(Type.RES); }
            set { Set(Type.RES, value); }
        }
        #endregion

        [NonSerialized]
        private CoreStats core;
        [NonSerialized]
        private IStats source;

        /// <summary>
        /// Gets a specified stat value.
        /// </summary>
        /// <param name="stat">Enum value of the stat to be retrieved. Must be of type DerivedStats.Type.</param>
        /// <returns>The retrieved stat value.</returns>
        public override short Get(Enum stat)
        {
            if (!(stat is Type)) throw enumEx;

            if (stats != null) return stats[stat];
            else if (source != null) return source.Get(stat);
            else
            {
                switch ((Type)stat)
                {
                    case Type.HP: return (short)(10 + (core.CON + 1) * core.LVL);
                    case Type.MP: return (short)((core.INT + core.WIS / 2) * core.LVL);
                    case Type.HIT: return (short)((core.DEX - 10) + (core.STR - 10) / 4 + core.LVL - 1);
                    case Type.AC_Natural: return (short)(core.BaseAC + core.NaturalArmor);
                    case Type.AC_Flat: return AC_Natural;
                    case Type.AC_Touch: return (short)Math.Max(0, AC_Natural + core.DEX - 10);
                    case Type.AC: return (short)Math.Max(0, AC_Flat + core.DEX - 10);
                    case Type.ATK: return (short)((core.STR - 10) * 10);
                    case Type.DEF: return (short)((core.CON - 10) * 5 + (core.STR - 10) * 5);
                    case Type.MAG: return (short)((core.INT - 10) * 10);
                    case Type.RES: return (short)((core.WIS - 10) * 10 + (core.CHA - 10) * 5);
                    default: throw new Exception("You should never get here");
                }
            }
        }

        /// <summary>
        /// Sets a specified stat value.
        /// </summary>
        /// <param name="stat">Enum value of the stat to be set. Must be of type DerivedStats.Type.</param>
        /// <param name="value">The new value for the stat.</param>
        public override void Set(Enum stat, short value)
        {
            if (!(stat is Type)) throw enumEx;
            if (stats != null) base.Set(stat, value);
            else throw readonlyEx;
        }

        /// <summary>
        /// Copies stat values to a new DerivedStats object. Does not copy linked stats.
        /// </summary>
        /// <returns>A new object containing the same stat values.</returns>
        public DerivedStats Clone()
        {
            DerivedStats derived = new DerivedStats();

            derived.stats = new Dictionary<Enum, short>(this.stats);

            return derived;
        }

        #region Constructors
        /// <summary>
        /// Creates a new DerivedStats object and initializes all stats to 0.
        /// </summary>
        public DerivedStats()
        {
            stats = new Dictionary<Enum, short>(6);
            foreach (Type stat in Enum.GetValues(typeof(Type)))
            {
                stats.Add(stat, 0);
            }
        }

        /// <summary>
        /// Creates a new DerivedStats object with the given stats. Use this for UnitModifier stats.
        /// </summary>
        /// <param name="hp">HP bonus</param>
        /// <param name="mp">MP bonus</param>
        /// <param name="hit">Hit bonus</param>
        /// <param name="ac">Armor bonus</param>
        /// <param name="atk">Attack bonus</param>
        /// <param name="def">Defense bonus</param>
        /// <param name="mag">Magic bonus</param>
        /// <param name="res">Resistance bonus</param>
        public DerivedStats(short hp, short mp, short hit, short ac, short atk, short def, short mag, short res)
            : this()
        {
            stats[Type.HP] = hp;
            stats[Type.MP] = mp;
            stats[Type.HIT] = hit;
            stats[Type.AC] = ac;
            stats[Type.AC_Flat] = ac;
            stats[Type.ATK] = atk;
            stats[Type.DEF] = def;
            stats[Type.MAG] = mag;
            stats[Type.RES] = res;
        }

        /// <summary>
        /// Creates a new read-only DerivedStats object linked to the given CoreStats object.
        /// </summary>
        /// <param name="coreStats">Derived stat values will be calculated based on these core stats.</param>
        public DerivedStats(CoreStats coreStats)
        {
            core = coreStats;
        }

        /// <summary>
        /// Creates a new read-only DerivedStats object that wraps a given IStats object. The object should contain mappings for DerivedStats.Type enum values.
        /// </summary>
        /// <param name="source">Should contain mappings for DerivedStats.Type enum values.</param>
        public DerivedStats(IStats source)
        {
            this.source = source;
        }
        #endregion

        #region Exceptions
        private static ArgumentException enumEx = new ArgumentException("Parameter must be of type " + typeof(Type));
        private static InvalidOperationException readonlyEx = new InvalidOperationException("Linked stats are read-only");
        #endregion
    }
}
