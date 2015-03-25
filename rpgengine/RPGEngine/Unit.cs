using System;

namespace RPGEngine
{
    [Serializable]
    public abstract class Unit : IUnit
    {
        /// <summary>
        /// This unit's name.
        /// </summary>
        public virtual string Name { get; protected set; }
        /// <summary>
        /// Gets this unit's class.
        /// </summary>
        public virtual string Class { get; protected set; }
        /// <summary>
        /// Gets this unit's level.
        /// </summary>
        public virtual ushort Level { get; protected set; }

        public abstract IStats BaseStats { get; }
        public abstract IStats Stats { get; }
        public abstract IStats StatModsAdditive { get; }
        public abstract IStats StatModsMultiplicative { get; }

        public abstract object Extensions { get; }

        public abstract void Evaluate();
    }
}
