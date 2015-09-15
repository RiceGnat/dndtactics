using System;
using System.Collections.Generic;

namespace DnDEngine
{
	public partial class DnDUnit
	{
		[Serializable]
		private class UnitDetails : IUnitEquipment, IVolatileStats
		{
			[NonSerialized]
			private DnDUnit unit;

			#region IUnitEquipment
			public IList<IEquipment> Equipped
			{
				get { return unit.Modifiers.GetSubsetOfType<IEquipment>(); }
			}

			void IUnitEquipment.Equip(IEquipment equipment)
			{
				unit.Modifiers.Add(equipment);
			}

			void IUnitEquipment.Unequip(IEquipment equipment)
			{
				unit.Modifiers.Remove(equipment);
			}

			void IUnitEquipment.UnequipAt(int index)
			{
				unit.Modifiers.Remove(Equipped[index]);
			}
			#endregion

			#region IVolatileStats
			public int CurrentHP { get; set; }
			public int CurrentMP { get; set; }
			public int Experience { get; set; }
			#endregion

			public void Rebind(DnDUnit unit)
			{
				this.unit = unit;
			}

			public UnitDetails(DnDUnit unit)
			{
				Rebind(unit);
			}
		}
	}
}
