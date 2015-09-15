namespace RPGLibrary
{
	public interface IRandom<T>
	{
		T LastResult { get; }
		T Generate();
	}
}
