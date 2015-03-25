using System;
using System.Collections.Generic;

namespace RPGEngine
{
    [Serializable]
    public abstract class Stats : IStats
    {
        protected IDictionary<Enum, short> stats;

        public virtual short Get(Enum stat)
        {
            return stats[stat];
        }

        public virtual short this[Enum stat]
        {
            get { return Get(stat); }
        }

        public virtual void Set(Enum stat, short value)
        {
            stats[stat] = value;
        }

        private class StatsZero : Stats
        {
            public override short Get(Enum stat)
            {
                return 0;
            }
        }
        private static readonly StatsZero zero = new StatsZero();

        /// <summary>
        /// Gets a reference to an object that returns 0 for all stats
        /// </summary>
        public static IStats Zero { get { return zero; } }
    }
}
