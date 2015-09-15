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
		public void Unit_Initialization()
		{
			DnDUnit unit = new DnDUnit(UNITNAME, UNITCLASS, UNITLEVEL);

			Assert.AreEqual(UNITNAME, unit.Name);
			Assert.AreEqual(UNITCLASS, unit.Class);
			Assert.AreEqual(UNITLEVEL, unit.Level);
			Assert.IsNotNull(unit.Stats);
			Assert.AreEqual(unit, unit.Modifiers.Target);
		}

		[TestMethod]
		public void Unit_Details()
		{
			DnDUnit unit = new DnDUnit(UNITNAME, UNITCLASS, UNITLEVEL);

			Assert.IsNotNull(unit.GetDetails<Object>());
		}
	}
}
