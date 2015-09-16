using System.Collections.Generic;
using RPGLibrary;

namespace DnDEngine
{
	public interface IUnitEquipment
	{
		IList<IEquipment> AllEquipment { get; }

		IEquipment GetSlot(EquipmentSlot equipment, int slotIndex);
		IEquipment Equip(IEquipment equipment, int slotIndex);
		IEquipment Unequip(EquipmentSlot slot, int slotIndex);

		IUnit EquippedBaselineUnit { get; }
	}

	public interface IVolatileStats
	{
		int CurrentHP { get; set; }
		int CurrentMP { get; set; }

		int Experience { get; set; }
	}
}
