using RPGLibrary;

namespace DnDEngine
{
	public interface IEquipment : IDecorator<IUnit>, ICatalogable
	{
		EquipmentSlot Slot { get; }
		int SlotsRequired { get; }
	}
}
