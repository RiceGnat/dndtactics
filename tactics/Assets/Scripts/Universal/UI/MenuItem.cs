using UnityEngine;
using UnityEngine.UI;
using System;

namespace Universal.UI
{
	/// <summary>
	/// Pairs a MenuButton with a MenuTarget.
	/// </summary>
	[Serializable]
	public class MenuItem
	{
		public EventButton button;
		public UIPanel target;
	}
}