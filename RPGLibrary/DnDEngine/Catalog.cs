using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPGLibrary.Utilities;

namespace DnDEngine
{
	public static class Catalog
	{
		private static IDictionary<uint, IEquipment> equipmentCatalog = new Dictionary<uint, IEquipment>();
		private static IDictionary<uint, IWeapon> weaponCatalog = new Dictionary<uint, IWeapon>();

		public static IWeapon UnarmedStrike
		{
			get { return GetWeapon(0); }
		}

		public static void RegisterEquipment(IEquipment equipment)
		{
			IEquipment clone = Serializer.DeepClone<IEquipment>(equipment);

			try
			{
				equipmentCatalog.Add(clone.ID, clone);
			}
			catch (ArgumentException ex)
			{
				throw new ArgumentException("Equipment with that ID is already registered in the catalog.", ex);
			}
		}

		public static void RegisterWeapon(IWeapon weapon)
		{
			IWeapon clone = Serializer.DeepClone<IWeapon>(weapon);

			try
			{
				weaponCatalog.Add(clone.ID, clone);
			}
			catch (ArgumentException ex)
			{
				throw new ArgumentException("Weapon with that ID is already registered in the catalog.", ex);
			}
		}

		public static IEquipment GetEquipment(uint id)
		{
			if (!equipmentCatalog.ContainsKey(id))
				throw new ArgumentException("No equipment registered with that ID");

			return Serializer.DeepClone<IEquipment>(equipmentCatalog[id]);
		}

		public static IWeapon GetWeapon(uint id)
		{
			if (!equipmentCatalog.ContainsKey(id))
				throw new ArgumentException("No weapon registered with that ID");

			return Serializer.DeepClone<IWeapon>(weaponCatalog[id]);
		}

		static Catalog()
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
