namespace RPGLibrary
{
	/// <summary>
	/// Represents anything that can be catalogued or indexed in some way. Used for objects like items, equipment, etc.
	/// </summary>
	public interface ICatalogable
	{
		/// <summary>
		/// Gets the object's ID.
		/// </summary>
		uint ID { get; }

		/// <summary>
		/// Gets the object's name.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Gets a description of the object.
		/// </summary>
		string Description { get; }
	}
}
