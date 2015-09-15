namespace RPGLibrary
{
	public interface IStatsPackage
	{
		IStats Calculated { get; }
		IStats Base { get; }
		IStats Additions { get; }
		IStats Multiplications { get; }
	}
}
