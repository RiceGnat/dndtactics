using System.Collections.Generic;
using RPGEngine;

namespace DnDEngine.Items
{
	/// <summary>
	/// Defines equipment properties.
	/// </summary>
	public interface IEquipment : ICatalogable
	{
		/// <summary>
		/// Gets the slot this equipment fits in.
		/// </summary>
		Equipment.Type Slot { get; }

		/// <summary>
		/// Gets this equipment's combat properties if it is a weapon.
		/// </summary>
		Equipment.WeaponProps WeaponProperties { get; }

		/// <summary>
		/// Gets the core stat modifiers for the new equipment.
		/// </summary>
		CoreStats CoreStatMods { get; }

		/// <summary>
		/// Gets the derived stat modifiers for the new equipment.
		/// </summary>
		DerivedStats DerivedStatMods { get; }

		/// <summary>
		/// Gets the buffs that the new equipment will grant the wearer.
		/// </summary>
		IEnumerable<DnDEngine.Combat.Magic.Buff> GrantedBuffs { get; }
	}
}
