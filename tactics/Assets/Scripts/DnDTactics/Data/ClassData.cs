using UnityEngine;
using System;
using DnDEngine;

namespace DnDTactics.Data
{
	/// <summary>
	/// Associates some auxiliary data to a class.
	/// </summary>
	[Serializable]
	public class ClassData
	{
		public string name;
		public Sprite artwork;
		public Sprite portrait;
		public bool unique;
		public short[] baseStats;
		public short[] pointCosts;
		public short movement;

		public CoreStats BaseStats
		{
			get { return new CoreStats(1, baseStats[0], baseStats[1], baseStats[2], baseStats[3], baseStats[4], baseStats[5], movement); }
		}
	}
}