using UnityEngine;
using System;
using System.Collections.Generic;
using RPGEngine;
using DnDEngine;

namespace DnDTactics.Data
{
	[Serializable]
	public sealed class SaveData
	{
		public DateTime Timestamp { get; set; }

		public List<DnDUnit> Party { get; set; }

		public List<ICatalogable> Caravan { get; set; }

		public SettingsData Settings { get; set; }
	}
}
