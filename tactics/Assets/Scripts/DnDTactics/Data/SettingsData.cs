using UnityEngine;
using System;
using Universal.UI;

namespace DnDTactics.Data
{
	[Serializable]
	public class SettingsData
	{
		public bool showCommandBox = true;

		public void Apply()
		{
			CommandPanel.IsEnabled = showCommandBox;
		}
	} 
}
