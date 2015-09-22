using System.Collections.Generic;
using RPGLibrary;

namespace DnDEngine
{
	public interface IUnitEquipment
	{
		IList<IEquipment> AllEquipment { get; }

		IEquipment GetSlot(EquipmentSlot equipment, int slotIndex = 0);
		IEquipment Equip(IEquipment equipment, int slotIndex = 0);
		IEquipment Unequip(EquipmentSlot slot, int slotIndex = 0);
		IWeapon GetWeapon(int slotIndex = 0);

		IUnit EquippedBaselineUnit { get; }
	}

	public interface IUnitBuffs
	{
		IList<IBuff> AllBuffs { get; }
	}

	public interface IVolatileStats
	{
		int CurrentHP { get; set; }
		int CurrentMP { get; set; }

		int Experience { get; set; }
	}
}
