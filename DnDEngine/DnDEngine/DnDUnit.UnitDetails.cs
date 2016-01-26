using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using RPGLibrary;

namespace DnDEngine
{
	public enum GenderType
	{
		None,
		Male,
		Female
	}

	public partial class DnDUnit
	{
		[Serializable]
		private class UnitDetails : IUnitAttributes, IVolatileStats, IUnitEquipment, IUnitBuffs
		{
			[NonSerialized]
			private DnDUnit unit;

			#region IUnitAttributes
			public GenderType Gender { get; set; }
			public ICollection<ISpell> SpellsKnown { get; private set; }
			#endregion

			#region IVolatileStats
			private int hp;
			private int mp;

			public int CurrentHP
			{
				get { return hp; }
				set { hp = Math.Min(Math.Max(0, value), unit.Stats.Calculated[DerivedStats.HP]); }
			}
			public int CurrentMP
			{
				get { return mp; }
				set { hp = Math.Min(Math.Max(0, value), unit.Stats.Calculated[DerivedStats.MP]); }
			}
			public int Experience { get; set; }

			public void Initialize()
			{
				hp = unit.Stats.Calculated[DerivedStats.HP];
				mp = unit.Stats.Calculated[DerivedStats.MP];
			}
			#endregion

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
				get { return equipmentManager.GetSubset<IEquipment>(); }
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
				get { return equipmentManager.GetSubset<IBuff>(); }
			}
			#endregion

			public void Rebind(DnDUnit unit)
			{
				this.unit = unit;
			}

			public UnitDetails(DnDUnit unit)
			{
				Rebind(unit);

				SpellsKnown = new List<ISpell>();

				unit.Modifiers.Add(equipmentManager);
			}
		}
	}
}
