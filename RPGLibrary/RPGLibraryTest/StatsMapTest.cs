using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RPGLibrary;

namespace RPGLibraryTest
{
	[TestClass]
	public class StatsMapTest
	{
		private const int TESTVALUE = 123;

		[TestMethod]
		public void StatsMap_Set()
		{
			StatsMap statsMap = new StatsMap();

			Assert.AreEqual(TESTVALUE, statsMap.Set("SET", TESTVALUE));
		}

		[TestMethod]
		public void StatsMap_Get()
		{
			StatsMap statsMap = new StatsMap();

			statsMap.Set("GET", TESTVALUE);

			Assert.AreEqual(TESTVALUE, statsMap.Get("GET"));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void StatsMap_Get_DoesntExist()
		{
			StatsMap statsMap = new StatsMap();
			statsMap.Get("NOPE");
		}
	}
}
