using System.Text;
using RPGLibrary;

namespace DnDEngine
{
	public enum MagicType
	{
		Arcane,
		Divine
	}

	public enum CastType
	{
		Self,
		Single,
		Multi,
		Area,
		Line,
		Ray
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

	public class SpellEventArgs
	{
		public IUnit Caster { get; set; }
		public IUnit Target { get; set; }
		public bool SaveSucceeded { get; set; }
		public StringBuilder Log { get; set; }
	}

	public delegate void CastHandler(SpellEventArgs e);

	public class Spell : ISpell
	{
		public string Name { get; set; }

		public CastType Type { get; set; }
		public int Range { get; set; }
		public int Radius { get; set; }

		public MagicType Magic { get; set; }
		public EnergyType? Energy { get; set; }
		public DiceType? Damage { get; set; }
		public CoreStats? Save { get; set; }

		public event CastHandler OnCast;

		public ILoggable Cast(IUnit caster, IUnit target, bool saved)
		{
			SpellEventArgs e = new SpellEventArgs();
			e.Caster = caster;
			e.Target = target;
			e.SaveSucceeded = saved;
			e.Log = new StringBuilder();

			if (OnCast != null) OnCast(e);

			if (e.Log.Length > 0)
			{
				return new LogString(e.Log.ToString());
			}
			else return null;
		}
	}
}
