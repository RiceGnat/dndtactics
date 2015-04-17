using System;

namespace DnDTactics
{
	/// <summary>
	/// Class to map equipment slots to indices
	/// </summary>
	[Serializable]
	public sealed class EquipIndex
	{
		public DnDEngine.Items.Equipment.Type Slot { get; set; }

		public int Index { get; set; }

		public DnDEngine.Items.Equipment Equip { get; set; }

		public EquipIndex(DnDEngine.Items.Equipment.Type slot, int index, DnDEngine.Items.Equipment equipment)
		{
			Slot = slot;
			Index = index;
			Equip = equipment;
		}
	}
}
