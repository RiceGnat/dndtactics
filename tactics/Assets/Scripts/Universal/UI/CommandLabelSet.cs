using UnityEngine;
using System;
using System.Collections.Generic;

namespace Universal.UI
{
	/// <summary>
	/// Associates labels to the Universal.EventKeys captured by UIPanel.
	/// </summary>
	[Serializable]
	public class CommandLabelSet
	{
		[SerializeField]
		private string[] labels = new string[UIPanel.CapturedInputs.Length];

		public string this[int i]
		{
			get { return labels[i]; }
			set { labels[i] = value; }
		}
	}
}