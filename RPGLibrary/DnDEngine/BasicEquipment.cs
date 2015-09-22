using RPGLibrary;

namespace DnDEngine
{
	public enum EquipmentSlot
	{
		Weapon,
		Head,
		Neck,
		Body,
		Hand,
		Feet
	}

	public class BasicEquipment : StatsModifier, IEquipment
	{
		public uint ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public EquipmentSlot Slot { get; set; }
		public int SlotsRequired { get; set; }

		public BasicEquipment() : base(new DnDStats(false), new DnDStats(false)) { }
	}
}
