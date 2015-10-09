using RPGLibrary;

namespace DnDEngine
{
	public interface IUnitEx : IUnit
	{
		IVolatileStats VolatileStats { get; }
		int CurrentHP { get; set; }
		int CurrentMP { get; set; }
		int Experience { get; set; }

		IUnitEquipment Equipment { get; }
		IStats BaselineStats { get; }

		void Initialize();
	}
}
