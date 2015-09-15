using System;
using System.Collections.Generic;
using RPGEngine;
using DnDEngine.Combat.Magic;

namespace DnDEngine.Items
{
	/// <summary>
	/// Contains properties for unit equipment.
	/// </summary>
    [Serializable]
    public partial class Equipment : UnitModifier, IEquipment
    {
		/// <summary>
		/// Defines the different equipment slots.
		/// </summary>
        public enum Type
        {
            Weapon,
            Head,
            Neck,
            Body,
            Arm,
            Hand,
            Foot
        }

		/// <summary>
		/// Gets this equipment's ID as registered in the armory.
		/// </summary>
        public uint ID { get; protected set; }
		/// <summary>
		/// Gets the name of this equipment.
		/// </summary>
        public string Name { get; protected set; }
		/// <summary>
		/// Gets the description of this equipment.
		/// </summary>
        public string Description { get; protected set; }

		/// <summary>
		/// Gets the slot this equipment fits in.
		/// </summary>
        public Type Slot { get; protected set; }

		/// <summary>
		/// Gets this equipment's combat properties if it is a weapon. Returns null if Slot is not Equipment.Type.Weapon.
		/// </summary>
		public WeaponProps WeaponProperties { get; protected set; }

		/// <summary>
		/// Gets the core stat modifiers for the new equipment as a read-only object. Equipment may only have additive stat multipliers.
		/// </summary>
		public CoreStats CoreStatMods { get { return new CoreStats(coreAdds); } }

		/// <summary>
		/// Gets the derived stat modifiers for the new equipment as a read-only object. Equipment may only have additive stat multipliers.
		/// </summary>
		public DerivedStats DerivedStatMods { get { return new DerivedStats(derivedAdds); } }

		/// <summary>
		/// Gets the buffs that the new equipment will grant the wearer.
		/// </summary>
		public IEnumerable<Buff> GrantedBuffs { get { return grantedBuffs; } }

		/// <summary>
		/// Returns a deep copy of this equipment.
		/// </summary>
		/// <returns></returns>
        public Equipment Clone()
        {
            Equipment e = this.MemberwiseClone() as Equipment;

            e.coreAdds = this.coreAdds.Clone();
            e.derivedAdds = this.derivedAdds.Clone();
            if (WeaponProperties != null) e.WeaponProperties = WeaponProperties.Clone();

            return e;
        }

        protected Equipment() { }

		/// <summary>
		/// Builder class to configure and create new equipment.
		/// </summary>
        public class Builder
        {
			/// <summary>
			/// Gets or sets the name of the new equipment.
			/// </summary>
            public string Name { get; set; }
			/// <summary>
			/// Gets or sets the description of the new equipment.
			/// </summary>
            public string Description { get; set; }
			/// <summary>
			/// Gets or sets the equipment slot of the new equipment.
			/// </summary>
            public Type Slot { get; set; }
			/// <summary>
			/// Gets or sets the core stat modifiers for the new equipment. Equipment may only have additive stat multipliers.
			/// </summary>
			public CoreStats CoreStatMods { get; set; }
			/// <summary>
			/// Gets or sets the derived stat modifiers for the new equipment. Equipment may only have additive stat multipliers.
			/// </summary>
            public DerivedStats DerivedStatMods { get; set; }
			/// <summary>
			/// Gets or sets the combat properties for the new equipment. This will only be set in the new object if it is of Equipment.Type.Weapon.
			/// </summary>
            public WeaponProps.Builder WeaponProperties { get; set; }

			/// <summary>
			/// Gets the collection of buffs that the new equipment will grant the wearer. 
			/// </summary>
            public ICollection<Buff> GrantedBuffs { get; private set; }

			/// <summary>
			/// Builds a new Equipment object out of the set properties.
			/// </summary>
			/// <param name="id">(optional) The ID for the new equipment. This will be overwritten when registering in the Armory, so this is only for testing.</param>
			/// <returns></returns>
            public Equipment Build(uint id = 0)
            {
                Equipment equip = new Equipment();
                equip.ID = id;
                equip.Name = Name ?? "";
                equip.Description = Description ?? "";
                equip.Slot = Slot;
                equip.coreAdds = CoreStatMods;
                equip.derivedAdds = DerivedStatMods;
                equip.coreMults = new CoreStats();
                equip.derivedMults = new DerivedStats();
                if (Slot == Type.Weapon) equip.WeaponProperties = WeaponProperties.Build();

                equip.grantedBuffs = new List<Buff>(GrantedBuffs);
                GrantedBuffs.Clear();

                return equip;
            }

			/// <summary>
			/// Creates a new Equipment.Builder.
			/// </summary>
            public Builder()
            {
                GrantedBuffs = new List<Buff>();
            }
        }
    }
}
