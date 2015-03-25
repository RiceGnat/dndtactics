using System;

namespace RPGEngine
{
    /// <summary>
    /// Represents a collection of stats mapped to enum values.
    /// </summary>
    public interface IStats
    {
        /// <summary>
        /// Gets a specified stat value.
        /// </summary>
        /// <param name="stat">Enum value of the stat to be retrieved.</param>
        /// <returns>The retrieved stat value.</returns>
        short Get(Enum stat);

        /// <summary>
        /// Gets a specified stat value.
        /// </summary>
        /// <param name="stat">Enum value of the stat to be retrieved.</param>
        /// <returns>The retrieved stat value.</returns>
        short this[Enum stat] { get; }
    }
}
