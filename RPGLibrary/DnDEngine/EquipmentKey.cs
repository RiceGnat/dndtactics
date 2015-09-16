using System;

namespace DnDEngine
{
	[Serializable]
	public struct EquipmentKey
	{
		public EquipmentSlot Slot { get; private set; }
		public int Index { get; private set; }

		public EquipmentKey(EquipmentSlot slot, int index)
			: this()
		{
			Slot = slot;
			Index = index;
		}
	}
}
