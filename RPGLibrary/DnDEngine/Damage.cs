using RPGLibrary;

namespace DnDEngine
{
	public enum DamageType {
		Physical,
		Magical,
		LifeLoss
	}

	public enum EnergyType
	{
		Fire,
		Lightning,
		Ice,
		Corrosive,
		Force,
		Positive,
		Negative
	}

	public struct Damage
	{
		public IUnit Source { get; private set; }
		public DamageType Type { get; private set; }
		public EnergyType Energy { get; private set; }
		public int Value { get; private set; }

		public Damage(IUnit source, DamageType type, EnergyType energy, int value) : this()
		{
			Source = source;
			Type = type;
			Energy = energy;
			Value = value;
		}
	}
}
