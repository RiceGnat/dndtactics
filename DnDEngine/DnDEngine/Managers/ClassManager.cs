using System;
using System.Collections.Generic;

namespace DnDEngine
{
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
