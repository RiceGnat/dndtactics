using System;
using System.Collections.Generic;
using System.Text;
using RPGLibrary;
using DnDEngine.Logging;

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
		Force,
		Positive,
		Negative
	}

	public enum EnhancementType
	{

	}

	public class SpellEventArgs
	{
		public IUnit Caster { get; set; }
		public IUnit Target { get; set; }
		public bool SaveSucceeded { get; set; }
		public StringBuilder Log { get; set; }
	}

	public delegate void CastHandler(SpellEventArgs e);

	[Serializable]
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

		public List<EnhancementType> enhancements = new List<EnhancementType>();
		public ICollection<EnhancementType> Enhancements { get { return enhancements; } }

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
