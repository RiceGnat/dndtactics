using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using RPGLibrary;

namespace DnDEngine
{
	public partial class DnDUnit
	{
		[Serializable]
		private class UnitDetails : IUnitEquipment, IUnitBuffs, IVolatileStats
		{
			[NonSerialized]
			private DnDUnit unit;

			#region IUnitEquipment
			[Serializable]
			private class EquipmentManager : CompoundUnitDecorator
			{
				private List<EquipmentKey> keys = new List<EquipmentKey>();

				public override void Add(IDecorator<IUnit> decorator)
				{
					if (!(decorator is IEquipment))
					{
						throw new ArgumentException("Decorator must inherit from IEquipment.");
					}

					Add(decorator as IEquipment, 0);
				}

				public void Add(IEquipment equipment, int slotIndex = 0)
				{
					EquipmentKey key = new EquipmentKey(equipment.Slot, slotIndex);
					if (keys.Contains(key))
					{
						throw new ArgumentException("Slot is occupied.");
					}
					base.Add(equipment);
					keys.Add(key);
				}

				public override IDecorator<IUnit> RemoveAt(int index)
				{
					keys.RemoveAt(index);
					return base.RemoveAt(index);
				}

				public IEquipment RemoveSlot(EquipmentSlot slot, int slotIndex = 0)
				{
					return RemoveAt(keys.IndexOf(new EquipmentKey(slot, slotIndex))) as IEquipment;
				}

				public IEquipment GetSlot(EquipmentSlot slot, int slotIndex = 0)
				{
					IEquipment equipment = null;
					int index = keys.IndexOf(new EquipmentKey(slot, slotIndex));
					if (index > -1) equipment = this[index] as IEquipment;
					return equipment;
				}
			}

			private EquipmentManager equipmentManager = new EquipmentManager();

			public IList<IEquipment> AllEquipment
			{
				get { return equipmentManager.GetSubsetOfType<IEquipment>(); }
			}

			public IEquipment GetSlot(EquipmentSlot slot, int slotIndex)
			{
				return equipmentManager.GetSlot(slot, slotIndex);
			}

			public IEquipment Equip(IEquipment equipment, int slotIndex)
			{
				IEquipment removed = null;

				if (equipmentManager.GetSlot(equipment.Slot, slotIndex) != null)
				{
					removed = Unequip(equipment.Slot, slotIndex);
				}
				equipmentManager.Add(equipment, slotIndex);

				return removed;
			}

			public IEquipment Unequip(EquipmentSlot slot, int slotIndex)
			{
				return equipmentManager.RemoveSlot(slot, slotIndex);
				// Move removed equipment to inventory
			}

			public IWeapon GetWeapon(int slotIndex)
			{
				return equipmentManager.GetSlot(EquipmentSlot.Weapon, slotIndex) as IWeapon;
			}

			public IUnit EquippedBaselineUnit
			{
				get { return equipmentManager.Result; }
			}
			#endregion

			#region IUnitBuffs
			public IList<IBuff> AllBuffs
			{
				get { return equipmentManager.GetSubsetOfType<IBuff>(); }
			}
			#endregion

			#region IVolatileStats
			private int hp;
			private int mp;

			public int CurrentHP
			{
				get { return hp; }
				set { hp = Math.Max(0, hp - value); }
			}
			public int CurrentMP
			{
				get { return mp; }
				set { hp = Math.Max(0, mp - value); }
			}
			public int Experience { get; set; }
			#endregion

			public void Rebind(DnDUnit unit)
			{
				this.unit = unit;
			}

			public UnitDetails(DnDUnit unit)
			{
				Rebind(unit);

				unit.Modifiers.Add(equipmentManager);

			}
		}
	}
}
