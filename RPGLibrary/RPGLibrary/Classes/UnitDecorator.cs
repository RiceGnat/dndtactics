using System;
using System.Collections.Generic;

namespace RPGLibrary
{
	/// <summary>
	/// Base class for a unit decorator. Anything that modifies a unit's properties can inherit from this class.
	/// </summary>
	[Serializable]
	public abstract class UnitDecorator : IUnit, IDecorator<IUnit>
	{
		[NonSerialized]
		protected IUnit baseUnit;

		public IUnit Target
		{
			get { return baseUnit; }
		}

		public virtual void Bind(IUnit unit)
		{
			baseUnit = unit;
		}

		public virtual IList<IDecorator<IUnit>> Children
		{
			get { return null; }
		}

		string IUnit.Name
		{
			get
			{
				return baseUnit.Name;
			}
			set
			{
				baseUnit.Name = value;
			}
		}

		string IUnit.Class
		{
			get
			{
				return baseUnit.Class;
			}
			set
			{
				baseUnit.Class = value;
			}
		}

		int IUnit.Level
		{
			get
			{
				return baseUnit.Level;
			}
			set
			{
				baseUnit.Level = value;
			}
		}

		IStatsPackage IUnit.Stats
		{
			get { return baseUnit.Stats; }
		}

		IDecoratorManager<IUnit> IUnit.Modifiers
		{
			get { return baseUnit.Modifiers; }
		}

		T IUnit.GetDetails<T>()
		{
			return baseUnit.GetDetails<T>();
		}
	}
}
