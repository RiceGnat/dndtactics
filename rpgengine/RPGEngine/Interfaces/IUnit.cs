namespace RPGEngine
{
    /// <summary>
    /// Defines properties and methods for handling units.
    /// </summary>
    public interface IUnit
    {
        /// <summary>
        /// Gets this unit's name.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Gets this unit's class.
        /// </summary>
        string Class { get; }
        /// <summary>
        /// Gets this unit's level.
        /// </summary>
        ushort Level { get; }

        /// <summary>
        /// Gets this unit's base stats (without any modifiers).
        /// </summary>
        IStats BaseStats { get; }
        /// <summary>
        /// Gets this unit's stats.
        /// </summary>
        IStats Stats { get; }
        /// <summary>
        /// Gets the total additive modifiers applied to this unit's stats.
        /// </summary>
        IStats StatModsAdditive { get; }
        /// <summary>
        /// Gets the total multiplicative modifiers applied to this unit's stats.
        /// </summary>
        IStats StatModsMultiplicative { get; }

        /// <summary>
        /// Gets an object containing additional properties or methods.
        /// </summary>
        object Extensions { get; }

        /// <summary>
        /// Evaluate and link all decorators assigned to this object.
        /// </summary>
        void Evaluate();
    }
}
