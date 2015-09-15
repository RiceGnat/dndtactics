using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RPGLibrary;
using DnDEngine;

namespace DnDEngineTest
{
	[TestClass]
	public class StatsTest
	{
		[TestMethod]
		public void Stats_InitializationLinked()
		{
			IStats stats = new DnDStats();

			foreach (string stat in Enum.GetNames(typeof(DnDStats.Core)))
			{
				Assert.AreEqual(0, stats.Get(stat));
			}
		}

		[TestMethod]
		public void Stats_InitializationUnlinked()
		{
			IStats stats = new DnDStats(false);

			foreach (string stat in Enum.GetNames(typeof(DnDStats.Derived)))
			{
				Assert.AreEqual(0, stats.Get(stat));
			}
		}

		[TestMethod]
		public void Stats_SetCore()
		{
			IStats stats = new DnDStats();
			int i = 1;
			
			foreach (string stat in Enum.GetNames(typeof(DnDStats.Core)))
			{
				stats.Set(stat, i);
				Assert.AreEqual(i, stats.Get(stat));
				i++;
			}
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void Stats_SetDerivedLinked()
		{
			IStats stats = new DnDStats();

			stats.Set(DnDStats.Derived.HP, 1);
		}

		[TestMethod]
		public void Stats_SetDerivedUnlinked()
		{
			IStats stats = new DnDStats(false);
			int i = 1;

			foreach (string stat in Enum.GetNames(typeof(DnDStats.Derived)))
			{
				stats.Set(stat, i);
				Assert.AreEqual(i, stats.Get(stat));
				i++;
			}
		}
	}
}
