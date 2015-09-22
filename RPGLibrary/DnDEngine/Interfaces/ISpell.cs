using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DnDEngine
{
	public interface ISpell
	{
		EnergyType? Energy { get; }
		DiceType? Damage { get; }

		int Range { get; }
		int Radius { get; }

	}
}
