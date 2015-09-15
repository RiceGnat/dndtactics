namespace RPGLibrary
{
	public interface IUnit
	{
		string Name { get; set; }
		string Class { get; set; }
		int Level { get; set; }

		IStatsPackage Stats { get; }
		IDecoratorManager<IUnit> Modifiers { get; }

		T GetDetails<T>() where T : class;
	}
}
