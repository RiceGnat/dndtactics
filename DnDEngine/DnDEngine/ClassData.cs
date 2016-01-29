using System;
using System.Collections.Generic;

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
}
