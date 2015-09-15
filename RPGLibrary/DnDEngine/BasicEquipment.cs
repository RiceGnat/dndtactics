using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPGLibrary;

namespace DnDEngine
{
	public enum EquipmentSlot
	{

	}

	public class BasicEquipment : StatsModifier, IEquipment
	{
		public uint ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public EquipmentSlot Slot { get; set; }

		public BasicEquipment() : base(new DnDStats(false), new DnDStats(false)) { }
	}
}
