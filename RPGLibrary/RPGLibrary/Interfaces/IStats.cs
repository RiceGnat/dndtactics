using System;

namespace RPGLibrary
{
	public interface IStats
	{
		int Get(string stat);
		int Get(Enum stat);
		int Set(string stat, int value);
		int Set(Enum stat, int value);

		int this[string stat] { get; set; }
		int this[Enum stat] { get; set; }
	}
}
