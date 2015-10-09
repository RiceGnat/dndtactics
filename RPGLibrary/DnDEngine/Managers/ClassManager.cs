using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DnDEngine
{
	public class ClassData
	{
		private Dictionary<CoreStats, int> baseStats = new Dictionary<CoreStats, int>();

		public string ClassName { get; set; }

		public IDictionary<CoreStats, int> BaseStats { get { return baseStats; } }
		public int BaseMovement { get; set; }

		private static readonly ClassData defaultData;
		public static ClassData Default { get { return defaultData; } }

		static ClassData()
		{
			defaultData = new ClassData();

			foreach (CoreStats stat in Enum.GetValues(typeof(CoreStats)))
			{
				defaultData.BaseStats[stat] = 10;
			}

			defaultData.BaseMovement = 5;
		}
	}

	public static class ClassManager
	{
		private static Dictionary<string, ClassData> classDict = new Dictionary<string, ClassData>();

		public static IEnumerable<ClassData> AllClasses { get { return classDict.Values; } }

		public static void AddClassData(ClassData classData)
		{
			if (classData == null) throw new ArgumentNullException("classData");
			if (String.IsNullOrEmpty(classData.ClassName))
				throw new ArgumentException("No class name specified in referenced class data.");

			classDict.Add(classData.ClassName, classData);
		}

		public static ClassData GetClassData(string className)
		{
			if (String.IsNullOrEmpty(className))
				throw new ArgumentNullException("className", "Class name is null or empty");
			if (!classDict.ContainsKey(className))
				throw new ArgumentException("There is no data for that class name.");

			return classDict[className];
		}
	}
}
