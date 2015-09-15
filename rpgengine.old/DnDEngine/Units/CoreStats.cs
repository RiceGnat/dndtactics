using System;
using System.Collections.Generic;
using RPGEngine;

namespace DnDEngine
{
    /// <summary>
    /// Represents the fundamental stats that make up a unit.
    /// </summary>
    [Serializable]
    public class CoreStats : Stats
    {
        /// <summary>
        /// Defines a unit's core stats.
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// Level
            /// </summary>
            LVL,
            /// <summary>
            /// Strength
            /// </summary>
            STR,
            /// <summary>
            /// Constitution
            /// </summary>
            CON,
            /// <summary>
            /// Dexterity
            /// </summary>
            DEX,
            /// <summary>
            /// Intelligence
            /// </summary>
            INT,
            /// <summary>
            /// Wisdom
            /// </summary>
            WIS,
            /// <summary>
            /// Charisma
            /// </summary>
            CHA,
            /// <summary>
            /// Movement
            /// </summary>
            MOV,
            /// <summary>
            /// Base armor class
            /// </summary>
            BaseAC,
            /// <summary>
            /// Natural armor
            /// </summary>
            NaturalArmor
        }

        #region Convenience mapping
        public short LVL
        {
            get { return Get(Type.LVL); }
            set { Set(Type.LVL, value); }
        }
        public short STR
        {
            get { return Get(Type.STR); }
            set { Set(Type.STR, value); }
        }
        public short CON
        {
            get { return Get(Type.CON); }
            set { Set(Type.CON, value); }
        }
        public short DEX
        {
            get { return Get(Type.DEX); }
            set { Set(Type.DEX, value); }
        }
        public short INT
        {
            get { return Get(Type.INT); }
            set { Set(Type.INT, value); }
        }
        public short WIS
        {
            get { return Get(Type.WIS); }
            set { Set(Type.WIS, value); }
        }
        public short CHA
        {
            get { return Get(Type.CHA); }
            set { Set(Type.CHA, value); }
        }
        public short MOV
        {
            get { return Get(Type.MOV); }
            set { Set(Type.MOV, value); }
        }
        public short BaseAC
        {
            get { return Get(Type.BaseAC); }
            set { Set(Type.BaseAC, value); }
        }
        public short NaturalArmor
        {
            get { return Get(Type.NaturalArmor); }
            set { Set(Type.NaturalArmor, value); }
        }
        #endregion

        [NonSerialized]
        private IStats source;

        /// <summary>
        /// Gets a specified stat value.
        /// </summary>
        /// <param name="stat">Enum value of the stat to be retrieved. Must be of type CoreStats.Type.</param>
        /// <returns>The retrieved stat value.</returns>
        public override short Get(Enum stat)
        {
            if (!(stat is Type)) throw enumEx;
            return stats != null ? stats[stat] : source.Get(stat);
        }

        /// <summary>
        /// Sets a specified stat value.
        /// </summary>
        /// <param name="stat">Enum value of the stat to be set. Must be of type CoreStats.Type.</param>
        /// <param name="value">The new value for the stat.</param>
        public override void Set(Enum stat, short value)
        {
            if (!(stat is Type)) throw enumEx;
            if (stats != null) base.Set(stat, value);
            else throw readonlyEx;
        }

        /// <summary>
        /// Copies stat values to a new CoreStats object. Does not copy linked stats.
        /// </summary>
        /// <returns>A new object containing the same stat values.</returns>
        public CoreStats Clone()
        {
            CoreStats core = new CoreStats();

            core.stats = new Dictionary<Enum, short>(this.stats);

            return core;
        }

        #region Constructors
        /// <summary>
        /// Creates a new CoreStats object and initializes all stats to 0.
        /// </summary>
        public CoreStats()
        {
            stats = new Dictionary<Enum, short>(6);
            foreach (Type stat in Enum.GetValues(typeof(Type)))
            {
                stats.Add(stat, 0);
            }
        }

        /// <summary>
        /// Creates a new CoreStats object with the given stats. Level, base AC, and natural armor are set to 0. Use this for UnitModifier stats.
        /// </summary>
        /// <param name="str">Strength</param>
        /// <param name="con">Constitution</param>
        /// <param name="dex">Dexterity</param>
        /// <param name="int">Intelligence</param>
        /// <param name="wis">Wisdom</param>
        /// <param name="cha">Charisma</param>
        /// <param name="mov">Movement</param>
        public CoreStats(short str, short con, short dex, short @int, short wis, short cha, short mov = 0)
            : this(0, str, con, dex, @int, wis, cha, mov) { }

        /// <summary>
        /// Creates a new CoreStats object with the given stats.
        /// </summary>
		/// <param name="lvl">Level</param>
        /// <param name="str">Strength</param>
        /// <param name="con">Constitution</param>
        /// <param name="dex">Dexterity</param>
        /// <param name="int">Intelligence</param>
        /// <param name="wis">Wisdom</param>
        /// <param name="cha">Charisma</param>
        /// <param name="mov">Movement</param>
        /// <param name="baseAC">Base armor class</param>
        /// <param name="naturalArmor">Natural armor</param>
        public CoreStats(short lvl, short str, short con, short dex, short @int, short wis, short cha, short mov, short baseAC = 0, short naturalArmor = 0)
            : this()
        {
            stats[Type.LVL] = lvl;
            stats[Type.STR] = str;
            stats[Type.CON] = con;
            stats[Type.DEX] = dex;
            stats[Type.INT] = @int;
            stats[Type.WIS] = wis;
            stats[Type.CHA] = cha;
            stats[Type.MOV] = mov;
            stats[Type.BaseAC] = baseAC;
            stats[Type.NaturalArmor] = naturalArmor;
        }

        /// <summary>
        /// Creates a new read-only CoreStats object that wraps a given IStats object. The object should contain mappings for CoreStats.Type enum values.
        /// </summary>
        /// <param name="source">Should contain mappings for CoreStats.Type enum values.</param>
        public CoreStats(IStats source)
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
