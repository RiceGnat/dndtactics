using UnityEngine;
using System;
using DnDEngine;

namespace DnDTactics
{
	/// <summary>
	/// Associates some auxiliary data to a class.
	/// </summary>
	[Serializable]
	public class ClassData
	{
		public DnDUnit.ClassType @class;
		public string className;
		public Sprite classArtwork;
		public Sprite classPortrait;
		public bool unique;
	}
}