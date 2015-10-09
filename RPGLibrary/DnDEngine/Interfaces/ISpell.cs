using RPGLibrary;
using DnDEngine.Logging;

namespace DnDEngine
{
	public interface ISpell
	{
		string Name { get; }

		CastType Type { get; }
		int Range { get; }
		int Radius { get; }

		MagicType Magic { get; }
		EnergyType? Energy { get; }
		DiceType? Damage { get; }
		CoreStats? Save { get; }

		ILoggable Cast(IUnit caster, IUnit target, bool saved);
	}
}
