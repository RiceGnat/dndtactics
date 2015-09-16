using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DnDEngine;

namespace DnDEngineTest
{
	[TestClass]
	public class EquipmentTest
	{
		[TestMethod]
		public void EquipmentKey_Equals()
		{
			EquipmentKey key1 = new EquipmentKey(EquipmentSlot.HumanoidHand, 0);
			EquipmentKey key2 = new EquipmentKey(EquipmentSlot.HumanoidHand, 0);

			Assert.AreEqual(key1, key2);
		}
	}
}
