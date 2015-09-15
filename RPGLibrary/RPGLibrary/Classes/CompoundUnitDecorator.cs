namespace RPGLibrary
{
	public class CompoundUnitDecorator : DecoratorManager<IUnit>, IUnit
	{
		string IUnit.Name
		{
			get
			{
				return Result.Name;
			}
			set
			{
				Result.Name = value;
			}
		}

		string IUnit.Class
		{
			get
			{
				return Result.Class;
			}
			set
			{
				Result.Class = value;
			}
		}

		int IUnit.Level
		{
			get
			{
				return Result.Level;
			}
			set
			{
				Result.Level = value;
			}
		}

		IStatsPackage IUnit.Stats
		{
			get { return Result.Stats; }
		}

		IDecoratorManager<IUnit> IUnit.Modifiers
		{
			get { return Result.Modifiers; }
		}

		T IUnit.GetDetails<T>()
		{
			return Result.GetDetails<T>();
		}
	}
}
