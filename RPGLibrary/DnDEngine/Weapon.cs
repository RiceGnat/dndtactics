using System;
using System.Collections.Generic;

namespace DnDEngine
{
	public enum WeaponType
	{
		Longsword,
		Shortsword,
		Spear,
		Axe
	}

	public class Weapon : BasicEquipment
	{
		public WeaponType Type { get; set; }
		public DiceType Damage { get; set; }

		public int CritRange { get; set; }
		public int CritMultiplier { get; set; }

		// WEAPON ENCHANTMENTS!


	}
}
