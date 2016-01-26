namespace DnDEngine.Combat
{
	public enum DamageType
	{
		Physical,
		Magical,
		LifeLoss
	}

	public struct Damage
	{
		public DamageType Type { get; private set; }
		public EnergyType? Energy { get; private set; }
		public int Value { get; private set; }

		public Damage(DamageType type, EnergyType? energy, int value)
			: this()
		{
			Type = type;
			Energy = energy;
			Value = value;
		}
	}
}
