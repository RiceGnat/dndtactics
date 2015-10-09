using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DnDEngine
{
	public static class UnitCreator
	{
		private static IUnitEx InstantiateUnit(string name, string unitClass, int unitLevel)
		{
			return new UnitAdapter(new DnDUnit(name, unitClass, unitLevel));
		}

		public static IUnitEx CreateUnit(string name, string unitClass, int unitLevel)
		{
			IUnitEx unit = InstantiateUnit(name, unitClass, unitLevel);

			// Get class info from ClassManager
			ClassData classData;
			try
			{
				classData = ClassManager.GetClassData(unitClass);
			}
			catch (ArgumentException)
			{
				classData = ClassData.Default;
			}

			// Set initial base stats
			foreach (CoreStats stat in Enum.GetValues(typeof(CoreStats)))
			{
				unit.Stats.Base[stat] = classData.BaseStats[stat];
			}

			unit.Stats.Base[OtherStats.MOV] = classData.BaseMovement;

			unit.Initialize();

			return unit;
		}

		public static IUnitEx CreateObject(string name)
		{
			IUnitEx unit = InstantiateUnit(name, "Object", 0);

			unit.Initialize();

			return unit;
		}
	}
}
