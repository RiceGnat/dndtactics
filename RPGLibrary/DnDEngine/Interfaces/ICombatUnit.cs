using RPGLibrary;

namespace DnDEngine
{
	public interface ICombatUnit : IUnit
	{
		int CurrentHP { get; set; }
		int CurrentMP { get; set; }
		int Experience { get; set; }

		IStats BaselineStats { get; }
		IUnitEquipment Equipment { get; }
	}
}
