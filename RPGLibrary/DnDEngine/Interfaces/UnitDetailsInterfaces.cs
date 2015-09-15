using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DnDEngine
{
	public interface IUnitEquipment
	{
		IList<IEquipment> Equipped { get; }

		void Equip(IEquipment equipment);
		void Unequip(IEquipment equipment);
		void UnequipAt(int index);
	}

	public interface IVolatileStats
	{
		int CurrentHP { get; set; }
		int CurrentMP { get; set; }

		int Experience { get; set; }
	}
}
