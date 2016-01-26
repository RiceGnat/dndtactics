using System;

namespace RPGLibrary
{
	/// <summary>
	/// Exposes methods to access individual stats.
	/// </summary>
	public interface IStats
	{
		/// <summary>
		/// Gets a stat by string name.
		/// </summary>
		/// <param name="stat">The name of the stat.</param>
		/// <returns>The value of the stat.</returns>
		int Get(string stat);

		/// <summary>
		/// Gets a stat by enum name.
		/// </summary>
		/// <param name="stat">The enum for the name of the stat.</param>
		/// <returns>The value of the stat.</returns>
		int Get(Enum stat);

		/// <summary>
		/// Sets the value of a stat by string name.
		/// </summary>
		/// <param name="stat">The name of the stat.</param>
		/// <param name="value">The new value for the stat.</param>
		/// <returns>The previous value assigned to the stat.</returns>
		int Set(string stat, int value);

		/// <summary>
		/// Sets the value of a stat by enum name.
		/// </summary>
		/// <param name="stat">The enum for the name of the stat.</param>
		/// <param name="value">The new value for the stat.</param>
		/// <returns>The previous value assigned to the stat.</returns>
		int Set(Enum stat, int value);

		/// <summary>
		/// Gets or sets the value of a stat by string name.
		/// </summary>
		/// <param name="stat">The name of the stat.</param>
		/// <returns>The value of the stat.</returns>
		int this[string stat] { get; set; }

		/// <summary>
		/// Gets or sets the value of a stat by enum name.
		/// </summary>
		/// <param name="stat">The enum for the name of the stat.</param>
		/// <returns>The value of the stat.</returns>
		int this[Enum stat] { get; set; }
	}
}
