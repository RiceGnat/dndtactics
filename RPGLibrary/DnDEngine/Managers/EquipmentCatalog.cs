using System;
using System.Collections.Generic;
using RPGLibrary.Utilities;

namespace DnDEngine
{
	public static class EquipmentCatalog
	{
		private static IDictionary<uint, IEquipment> equipmentCatalog = new Dictionary<uint, IEquipment>();
		private static IDictionary<uint, IWeapon> weaponCatalog = new Dictionary<uint, IWeapon>();

		public static IWeapon UnarmedStrike
		{
			get { return GetWeapon(0); }
		}

		public static void RegisterEquipment(IEquipment equipment)
		{
			if (equipment == null) throw new ArgumentException("equipment");

			IEquipment clone = Serializer.DeepClone<IEquipment>(equipment);
			equipmentCatalog.Add(clone.ID, clone);
		}

		public static void RegisterWeapon(IWeapon weapon)
		{
			if (weapon == null) throw new ArgumentException("weapon");

			IWeapon clone = Serializer.DeepClone<IWeapon>(weapon);
			weaponCatalog.Add(clone.ID, clone);
		}

		public static IEquipment GetEquipment(uint id)
		{
			if (!equipmentCatalog.ContainsKey(id)) 
				throw new ArgumentException("No equipment registered with that ID.");

			return Serializer.DeepClone<IEquipment>(equipmentCatalog[id]);
		}

		public static IWeapon GetWeapon(uint id)
		{
			if (!weaponCatalog.ContainsKey(id))
				throw new ArgumentException("No weapon registered with that ID.");

			return Serializer.DeepClone<IWeapon>(weaponCatalog[id]);
		}

		static EquipmentCatalog()
		{
			// Register unarmed strike for attacks with no weapon equipped
			Weapon unarmed = new Weapon();

			unarmed.ID = 0;
			unarmed.Name = "Unarmed Strike";
			unarmed.Description = "A basic strike with no weapon";
			unarmed.Damage = DiceType.D4;

			RegisterWeapon(unarmed);
		}
	}
}
