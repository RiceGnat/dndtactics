using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DnDEngine;

namespace DnDEngineTest
{
	[TestClass]
	public class DnDUnitTest
	{
		private const string UNITNAME = "Test Unit";
		private const string UNITCLASS = "Test Class";
		private const int UNITLEVEL = 10;
		private const string STATNAME = "TEST";
		private const int STATVALUE = 123;
		private const int ADDVALUE = 10;
		private const int MULTVALUE = 20;

		[TestMethod]
		public void DnDUnit_Initialization()
		{
			DnDUnit unit = new DnDUnit(UNITNAME, UNITCLASS, UNITLEVEL);

			Assert.AreEqual(UNITNAME, unit.Name);
			Assert.AreEqual(UNITCLASS, unit.Class);
			Assert.AreEqual(UNITLEVEL, unit.Level);
			Assert.IsNotNull(unit.Stats);
			Assert.AreEqual(unit, unit.Modifiers.Target);
		}

		[TestMethod]
		public void DnDUnit_DetailsInitialization()
		{
			DnDUnit unit = new DnDUnit(UNITNAME, UNITCLASS, UNITLEVEL);

			Assert.IsNotNull(unit.GetDetails<Object>());
		}

		[TestMethod]
		public void DnDUnit_EquipUnit()
		{
			DnDUnit unit = new DnDUnit(UNITNAME, UNITCLASS, UNITLEVEL);
			unit.Stats.Base[STATNAME] = STATVALUE;

			BasicEquipment equipment = new BasicEquipment();
			equipment.Adds[STATNAME] = ADDVALUE;
			equipment.Mults[STATNAME] = MULTVALUE;
			equipment.Slot = EquipmentSlot.Body;

			unit.GetDetails<IUnitEquipment>().Equip(equipment, 0);

			Assert.AreEqual(159, unit.Stats.Calculated[STATNAME]);
			Assert.AreEqual(equipment, unit.Modifiers.Children[1]);
			Assert.AreEqual(equipment, unit.Modifiers.GetSubset<IEquipment>()[0]);
			Assert.AreEqual(unit.Name, unit.Modifiers.Result.Name);
		}

		[TestMethod]
		public void DnDUnit_UnequipUnit()
		{
			DnDUnit unit = new DnDUnit(UNITNAME, UNITCLASS, UNITLEVEL);
			unit.Stats.Base[STATNAME] = STATVALUE;

			BasicEquipment equipment = new BasicEquipment();
			equipment.Adds[STATNAME] = ADDVALUE;
			equipment.Mults[STATNAME] = MULTVALUE;
			equipment.Slot = EquipmentSlot.Body;

			unit.GetDetails<IUnitEquipment>().Equip(equipment, 0);

			Assert.AreEqual(159, unit.Stats.Calculated[STATNAME]);
		}
	}
}
