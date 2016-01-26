namespace RPGLibrary
{
	/// <summary>
	/// Represents a basic RPG unit.
	/// </summary>
	public interface IUnit
	{
		/// <summary>
		/// Gets or sets the unit's name.
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// Gets or sets the unit's class.
		/// </summary>
		string Class { get; set; }

		/// <summary>
		/// Gets or sets the unit's level.
		/// </summary>
		int Level { get; set; }

		/// <summary>
		/// Gets the unit's stats.
		/// </summary>
		IStatsPackage Stats { get; }

		/// <summary>
		/// Gets the modifiers attached to the unit.
		/// </summary>
		IDecoratorManager<IUnit> Modifiers { get; }

		/// <summary>
		/// Fetches the unit's details object as the specified type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		T GetDetails<T>() where T : class;
	}
}
