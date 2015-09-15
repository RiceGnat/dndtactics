using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPGLibrary;

namespace DnDEngine
{
	public class ComplexEquipment : CompoundUnitDecorator, IEquipment
	{
		public uint ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public ComplexEquipment()
		{
			StatsModifier mods = new StatsModifier(new DnDStats(false), new DnDStats(false));
			Add(mods);
		}
	}
}
