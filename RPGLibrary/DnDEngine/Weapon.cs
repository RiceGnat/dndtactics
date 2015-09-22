using System;
using System.Collections.Generic;

namespace DnDEngine
{
	public enum WeaponType
	{
		Longsword,
		Shortsword,
		Spear,
		Axe,
		Bow
	}

	public class Weapon : BasicEquipment, IWeapon
	{
		public WeaponType Type { get; set; }
		public DiceType Damage { get; set; }

		public int CritRange { get; set; }
		public int CritMultiplier { get; set; }

		// WEAPON ENCHANTMENTS!


	}
}
