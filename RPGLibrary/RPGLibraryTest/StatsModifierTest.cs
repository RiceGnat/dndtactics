using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RPGLibrary;

namespace RPGLibraryTest
{
	[TestClass]
	public class StatsModifierTest
	{
		private const string STATNAME = "TEST";
		private const int STATVALUE = 123;
		private const int ADDVALUE = 10;
		private const int MULTVALUE = 20;

		[TestMethod]
		public void StatsModifier_Initialization()
		{
			StatsModifier modifier = new StatsModifier();

			Assert.IsNotNull((modifier as IUnit).Stats);
		}

		[TestMethod]
		public void StatsModifier_Add()
		{
			StatsModifier modifier = new StatsModifier();
			IUnit unit = new UnitBase();
			IUnit modifiedUnit = modifier as IUnit;
			modifier.Bind(unit);

			unit.Stats.Base[STATNAME] = STATVALUE;
			modifier.Adds[STATNAME] = ADDVALUE;
			modifier.Mults[STATNAME] = 0;

			Assert.AreEqual(133, modifiedUnit.Stats.Calculated[STATNAME]);
		}

		[TestMethod]
		public void StatsModifier_Multiply()
		{
			StatsModifier modifier = new StatsModifier();
			IUnit unit = new UnitBase();
			IUnit modifiedUnit = modifier as IUnit;
			modifier.Bind(unit);

			unit.Stats.Base[STATNAME] = STATVALUE;
			modifier.Adds[STATNAME] = 0;
			modifier.Mults[STATNAME] = MULTVALUE;

			Assert.AreEqual(147, modifiedUnit.Stats.Calculated[STATNAME]);
		}

		[TestMethod]
		public void StatsModifier_AddandMultiply()
		{
			StatsModifier modifier = new StatsModifier();
			IUnit unit = new UnitBase();
			IUnit modifiedUnit = modifier as IUnit;
			modifier.Bind(unit);

			unit.Stats.Base[STATNAME] = STATVALUE;
			modifier.Adds[STATNAME] = ADDVALUE;
			modifier.Mults[STATNAME] = MULTVALUE;

			Assert.AreEqual(159, modifiedUnit.Stats.Calculated[STATNAME]);
		}
	}
}
