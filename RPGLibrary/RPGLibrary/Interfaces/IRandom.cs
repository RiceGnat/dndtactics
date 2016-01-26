namespace RPGLibrary
{
	/// <summary>
	/// Allows generation of random results of a given type <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">The type of result to generate.</typeparam>
	public interface IRandom<T>
	{
		/// <summary>
		/// Gets the last result.
		/// </summary>
		T LastResult { get; }

		/// <summary>
		/// Generates a new random result.
		/// </summary>
		/// <returns>The new randomly generated result of type <typeparamref name="T"/>.</returns>
		T Generate();
	}
}
