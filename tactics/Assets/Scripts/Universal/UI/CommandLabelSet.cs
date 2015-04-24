using UnityEngine;
using System;
using System.Collections.Generic;

namespace Universal.UI
{
	[Serializable]
	public class CommandLabelSet
	{
		[SerializeField]
		private string[] labels = new string[UIPanel.CapturedInputs.Length];

		public string this[int i]
		{
			get { return labels[i]; }
		}
	}
}