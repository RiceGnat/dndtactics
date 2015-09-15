using System;
using System.Collections.Generic;
using RPGEngine;
using DnDEngine.Combat.Magic;

namespace DnDEngine
{
    /// <summary>
    /// Base class for unit modifiers.
    /// </summary>
    [Serializable]
    public class UnitModifier : UnitDecorator, IUnit
    {
        #region Properties of this modifier
        protected CoreStats coreAdds;
        protected CoreStats coreMults;
        protected DerivedStats derivedAdds;
        protected DerivedStats derivedMults;
        protected ICollection<Buff> grantedBuffs;
        #endregion

        #region Calculated values
        [NonSerialized]
        private CoreStats core;
        [NonSerialized]
        private DerivedStats derived;

        [NonSerialized]
        private IStats stats;
        IStats IUnit.Stats { get { return stats; } }

        [NonSerialized]
        private IStats statAdds;
        IStats IUnit.StatModsAdditive { get { return statAdds; } }

        [NonSerialized]
        private IStats statMults;
        IStats IUnit.StatModsMultiplicative { get { return statMults; } }

        [NonSerialized]
        private object extensions;
        object IUnit.Extensions { get { return extensions; } }
        #endregion

        // Internal class to sum stats
        private class StatModifier : IStats
        {
            private IStats baseStats;
            private IStats adds;
            private IStats mults;

            private short Get(Enum stat) {
                int added = baseStats.Get(stat) + adds.Get(stat);
                return (short)(added + Math.Truncate(added * mults.Get(stat) / 100f));
            }

            short IStats.Get(Enum stat)
            {
                return Get(stat);
            }

            short IStats.this[Enum stat]
            {
                get { return Get(stat); }
            }

            public StatModifier(IStats baseStats, IStats additiveMods, IStats multiplicativeMods = null)
            {
                this.baseStats = baseStats;
                adds = additiveMods;
                mults = multiplicativeMods ?? Stats.Zero;
            }
        }

        /// <summary>
        /// Bind this modifier to the given unit.
        /// </summary>
        /// <param name="baseUnit">Unit that this modifier will be bound to.</param>
        /// <returns>The bound modifier cast as an IUnit.</returns>
        public override IUnit Bind(IUnit baseUnit)
        {
            base.Bind(baseUnit);

            // Link cumulative multiplier values
            statAdds = new StatModifier(baseUnit.StatModsAdditive, new StatsRouter(coreAdds, derivedAdds));
            statMults = new StatModifier(baseUnit.StatModsMultiplicative, new StatsRouter(coreMults, derivedMults));

            // Link calculated stat values
            core = new CoreStats(new StatModifier(baseUnit.BaseStats, statAdds, statMults));
            derived = new DerivedStats(new StatModifier(new DerivedStats(core), statAdds, statMults));
            stats = new StatsRouter(core, derived, true);

            // Add buffs to extensions object
            extensions = (baseUnit.Extensions as ICloneable).Clone();
            if (grantedBuffs != null)
            {
                foreach (Buff buff in grantedBuffs)
                {
                    (extensions as IBuffs).All.Add(buff);
                }
            }

            return this;
        }

        protected UnitModifier() { }

		/// <summary>
		/// Creates a new generic UnitModifier with the specified stat modifiers.
		/// </summary>
		/// <param name="coreAdds">Additive core stat modifiers</param>
		/// <param name="coreMults">Multiplicative core stat modifiers</param>
		/// <param name="derivedAdds">Additive derived stat modifiers</param>
		/// <param name="derivedMults">Multiplicative derived stat modifiers</param>
        public UnitModifier(CoreStats coreAdds, CoreStats coreMults, DerivedStats derivedAdds, DerivedStats derivedMults)
        {
            this.coreAdds = coreAdds;
            this.coreMults = coreMults;
            this.derivedAdds = derivedAdds;
            this.derivedMults = derivedMults;
        }
    }
}
