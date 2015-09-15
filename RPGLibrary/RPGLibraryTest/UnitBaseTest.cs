using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RPGLibrary;

namespace RPGLibraryTest
{
	[TestClass]
	public class UnitBaseTest
	{
		private const string UNITNAME = "Test Unit";
		private const string UNITCLASS = "Test Class";
		private const int UNITLEVEL = 10;
		private const string STATNAME = "TEST";
		private const int STATVALUE = 123;
		private const int ADDVALUE = 10;
		private const int MULTVALUE = 20;

		[TestMethod]
		public void UnitBase_Initialization()
		{
			UnitBase unit = new UnitBase(UNITNAME, UNITCLASS, UNITLEVEL);

			Assert.AreEqual(UNITNAME, unit.Name);
			Assert.AreEqual(UNITCLASS, unit.Class);
			Assert.AreEqual(UNITLEVEL, unit.Level);
			Assert.IsNotNull(unit.Stats);
			Assert.AreEqual(unit, unit.Modifiers.Target);
		}

		[TestMethod]
		public void UnitBase_Stats()
		{
			UnitBase unit = new UnitBase(UNITNAME, UNITCLASS, UNITLEVEL);
			unit.Stats.Base[STATNAME] = STATVALUE;

			Assert.AreEqual(STATVALUE, unit.Stats.Base[STATNAME]);
			Assert.AreEqual(STATVALUE, unit.Stats.Calculated[STATNAME]);
			Assert.AreEqual(0, unit.Stats.Additions[STATNAME]);
			Assert.AreEqual(0, unit.Stats.Multiplications[STATNAME]);
		}

		[TestMethod]
		public void UnitBase_Modifiers()
		{
			IUnit unit = new UnitBase(UNITNAME, UNITCLASS, UNITLEVEL);
			unit.Stats.Base[STATNAME] = STATVALUE;

			StatsModifier modifier1 = new StatsModifier();
			modifier1.Adds[STATNAME] = ADDVALUE;
			modifier1.Mults[STATNAME] = 0;

			StatsModifier modifier2 = new StatsModifier();
			modifier2.Adds[STATNAME] = ADDVALUE;
			modifier2.Mults[STATNAME] = MULTVALUE;

			unit.Modifiers.Add(modifier1);
			unit.Modifiers.Add(modifier2);

			IUnit modifiedUnit = unit.Modifiers.Result;

			Assert.AreEqual(171, modifiedUnit.Stats.Calculated[STATNAME]);
			Assert.AreEqual(unit.Stats.Base[STATNAME], modifiedUnit.Stats.Base[STATNAME]);
			Assert.AreEqual(ADDVALUE * 2, modifiedUnit.Stats.Additions[STATNAME]);
			Assert.AreEqual(MULTVALUE, modifiedUnit.Stats.Multiplications[STATNAME]);
		}
	}
}
