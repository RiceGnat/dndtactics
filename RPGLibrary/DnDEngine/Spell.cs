namespace DnDEngine
{
	public enum CastType
	{
		Self,
		Single,
		Multi,
		Area,
		Line,
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

	public class Spell : ISpell
	{
		public CastType Type { get; set; }
		public int Range { get; set; }
		public int Radius { get; set; }

		public EnergyType? Energy { get; set; }
		public DiceType? Damage { get; set; }

	}
}
