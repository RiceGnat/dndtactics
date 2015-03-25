using System;
using RPGEngine;

namespace DnDEngine
{
    /// <summary>
    /// Consolidates CoreStats and DerivedStats into a single IStats interface.
    /// </summary>
    public class StatsRouter : IStats
    {
        private IStats core;
        private IStats derived;
        private bool final;

        /// <summary>
        /// Gets a specified stat value.
        /// </summary>
        /// <param name="stat">Enum value of the stat to be retrieved. Must be of type CoreStats.Type or DerivedStats.Type.</param>
        /// <returns>The retrieved stat value.</returns>
        public short Get(Enum stat)
        {
            if (stat is CoreStats.Type)
            {
                return final ? Math.Max((short)0, core.Get(stat)) : core.Get(stat);
            }
            else if (stat is DerivedStats.Type)
            {
                return derived.Get(stat);
            }
            else
            {
                throw new ArgumentException("Parameter must be Enum of type CoreStats.Type or DerivedStats.Type");
            }
        }

        public short this[Enum stat]
        {
            get { return Get(stat); }
        }

        /// <summary>
        /// Creates a new StatsRouter object from the given CoreStats and DerivedStats objects.
        /// </summary>
        /// <param name="coreStats">Core stats will be retrieved from this object.</param>
        /// <param name="derivedStats">Derived stats will be retrieved from this object.</param>
        /// <param name="final">(optional) If set to true, core stats will have a lower bound of 0.</param>
        public StatsRouter(CoreStats coreStats, DerivedStats derivedStats, bool final = false)
        {
            core = coreStats;
            derived = derivedStats;
            this.final = final;
        }
    }
}
