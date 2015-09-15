using System;
using System.Collections.Generic;
using RPGEngine;
using DnDEngine.Items;
using DnDEngine.Combat.Magic;

namespace DnDEngine
{
    #region Interfaces
    /// <summary>
    /// Allows access to additional unit properties.
    /// </summary>
    public interface IDetails
    {
		/// <summary>
		/// Gets the unit's class enum value.
		/// </summary>
        DnDUnit.ClassType Class { get; }

		/// <summary>
		/// Gets the unit's body shape enum value.
		/// </summary>
        DnDUnit.BodyType Body { get; }

		/// <summary>
		/// Gets the unit's gender enum value.
		/// </summary>
        DnDUnit.GenderType Gender { get; }

		/// <summary>
		/// Gets the unit's experience points.
		/// </summary>
		uint Experience { get; }

		/// <summary>
		/// Gets the experience threshold for the unit's next level up.
		/// </summary>
		uint NextLevel { get; }
    }

    /// <summary>
    /// Allows access to unit's inventory.
    /// </summary>
    public interface IInventory
    {
		IList<ICatalogable> All { get; }
    }

    /// <summary>
    /// Allows access to unit's equipment.
    /// </summary>
    public interface IEquipped
    {
		/// <summary>
		/// Gets the unit's equipment organized by equipment slot.
		/// </summary>
        IDictionary<Equipment.Type, IList<Equipment>> Slots { get; }

		/// <summary>
		/// Gets all the unit's equipment without organization.
		/// </summary>
        ICollection<Equipment> All { get; }

		/// <summary>
		/// Gets the unit's equipped weapon.
		/// </summary>
        Equipment Weapon { get; }

		/// <summary>
		/// Gets the number of available equipment slots.
		/// </summary>
        int SlotCount { get; }

		/// <summary>
		/// Gets the equipment in a given slot.
		/// </summary>
		/// <param name="slot">Equipment slot</param>
		/// <returns>List of equipment in the given slot</returns>
        IList<Equipment> this[Equipment.Type slot] { get; }

		/// <summary>
		/// Assigns an equipment to a slot.
		/// </summary>
		/// <param name="equipment">Equipment to be assigned</param>
		/// <param name="slot">Target slot</param>
		/// <param name="position">(optional) Target position in the slot</param>
        void Equip(Equipment equipment, Equipment.Type slot, ushort position = 0);
    }

    /// <summary>
    /// Allows access to unit's buffs and debuffs.
    /// </summary>
    public interface IBuffs
    {
		/// <summary>
		/// Gets all the buffs and debuffs on this unit.
		/// </summary>
        ICollection<Buff> All { get; }

		/// <summary>
		/// Gets the number of buffs and debuffs on this unit.
		/// </summary>
        int Count { get; }

		/// <summary>
		/// Adds a new buff or debuff to the unit.
		/// </summary>
		/// <param name="buff">Buff or debuff to be added</param>
        void AddBuffToUnit(Buff buff);

		/// <summary>
		/// Attempts to remove a particular Buff object from the unit.
		/// </summary>
		/// <param name="buff"></param>
        void RemoveBuffFromUnit(Buff buff);
    }

    /// <summary>
    /// Allows access to unit's known spells
    /// </summary>
    public interface ISpells
    {
		/// <summary>
		/// Gets all the spells known by the unit.
		/// </summary>
        ICollection<uint> All { get; }

		/// <summary>
		/// Gets the number of spells known by the unit.
		/// </summary>
        int Count { get; }
    }
    #endregion

    public partial class DnDUnit
    {
        /// <summary>
        /// Internal class to hold additional unit data. Cast to interfaces to expose different properties.
        /// </summary>
        [Serializable]
        private class UnitExtensions : IDetails, IInventory, IEquipped, IBuffs, ISpells, ICloneable
        {
			[NonSerialized]
            private DnDUnit unit;
            private IList<ICatalogable> inventory = new List<ICatalogable>();
            private IDictionary<Equipment.Type, IList<Equipment>> equips = new Dictionary<Equipment.Type, IList<Equipment>>();
            private ICollection<Buff> buffs = new List<Buff>();
            private ICollection<uint> spells = new List<uint>();

            #region IDetails
            ClassType IDetails.Class
            {
                get { return unit.classEnum; }
            }
            BodyType IDetails.Body
            {
                get { return unit.body; }
            }

            GenderType IDetails.Gender
            {
                get { return unit.gender; }
            }

			uint IDetails.Experience
			{
				get { return unit.Experience; }
			}

			uint IDetails.NextLevel
			{
				get { return (uint)((unit.Level + 1) * unit.Level) * 50; }
			}
			#endregion

			#region IInventory
			IList<ICatalogable> IInventory.All
			{
				get { return inventory; }
			}
			#endregion

			#region IEquipped
			IDictionary<Equipment.Type, IList<Equipment>> IEquipped.Slots { get { return equips; } }

            ICollection<Equipment> IEquipped.All
            {
                get
                {
                    var l = new List<Equipment>();
                    foreach (var slot in equips.Values)
                    {
                        foreach (var e in slot)
                        {
                            if (e != null) l.Add(e);
                        }
                    }
                    return l;
                }
            }

            Equipment IEquipped.Weapon { get { return equips[Equipment.Type.Weapon][0]; } }

            int IEquipped.SlotCount { get { return equips.Count; } }

            IList<Equipment> IEquipped.this[Equipment.Type slot] { get { return equips[slot]; } }

            void IEquipped.Equip(Equipment equipment, Equipment.Type slot, ushort position)
            {
                try
                {
                    equips[slot][position] = equipment;
                }
                catch (KeyNotFoundException ex)
                {
                    throw new KeyNotFoundException("The specified equipment slot does not exist", ex);
                }
                catch (IndexOutOfRangeException ex)
                {
                    throw new IndexOutOfRangeException("The specified position is out of range for that slot", ex);
                }
            }
            #endregion

            #region IBuffs
            ICollection<Buff> IBuffs.All { get { return buffs; } }

            int IBuffs.Count { get { return buffs.Count; } }

            void IBuffs.AddBuffToUnit(Buff buff)
            {
                (unit.extensions as UnitExtensions).buffs.Add(buff);
                if (unit.evaluated != null) unit.Evaluate();
            }

            void IBuffs.RemoveBuffFromUnit(Buff buff)
            {
                (unit.extensions as UnitExtensions).buffs.Remove(buff);
                if (unit.evaluated != null) unit.Evaluate();
            }
            #endregion

            #region ISpells
            ICollection<uint> ISpells.All { get { return spells; } }
            int ISpells.Count { get { return spells.Count; } }
            #endregion

            #region ICloneable
            object ICloneable.Clone()
            {
                UnitExtensions ex = new UnitExtensions();
                ex.unit = unit;
                ex.inventory = inventory;
                ex.equips = equips;
                ex.buffs = new List<Buff>(buffs);
                ex.spells = new List<uint>(spells);

                return ex;
            }
            #endregion

			/// <summary>
			/// Links a DnDUnit to this object. Used to re-link dynamic properties following post-serialization.
			/// </summary>
			/// <param name="unit"></param>
			public void LinkUnit(DnDUnit unit)
			{
				this.unit = unit;
			}

            #region Constructors
            private UnitExtensions() { }

            /// <summary>
            /// Create a new UnitExtensions object for the given unit.
            /// </summary>
            /// <param name="unit">The unit this object will be attached to.</param>
            public UnitExtensions(DnDUnit unit)
            {
				LinkUnit(unit);

                // Set up equipment slots
                if (unit.body == DnDUnit.BodyType.Humanoid)
                {
                    equips.Add(Equipment.Type.Weapon, new Equipment[1]);
                    equips.Add(Equipment.Type.Head, new Equipment[1]);
                    equips.Add(Equipment.Type.Neck, new Equipment[1]);
                    equips.Add(Equipment.Type.Body, new Equipment[1]);
                    equips.Add(Equipment.Type.Arm, new Equipment[2]);
                    equips.Add(Equipment.Type.Foot, new Equipment[2]);
                }
            }
            #endregion
		}
    }
}
