using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DnDEngine;
using DnDEngine.Combat;

namespace DnDEngineTest
{
	[TestClass]
	public class CombatManagerTest
	{
		private const string UNITNAME = "Test Unit";
		private const string UNITCLASS = "Test Class";
		private const int UNITLEVEL = 10;

		[TestMethod]
		public void CombatManager_CalculateDamageNeutral()
		{
			IUnitEx unit = UnitCreator.CreateUnit(UNITNAME, UNITCLASS, UNITLEVEL);
			unit.Stats.Base[CoreStats.STR] = 10;

			Damage damage = CombatManager.CalculateDamage(unit, DamageType.Physical, null, 10);

			Assert.AreEqual(0, unit.Stats.Calculated[DerivedStats.ATK]);
			Assert.AreEqual(10, damage.Value);
		}

		[TestMethod]
		public void CombatManager_CalculateDamageBonus()
		{
			IUnitEx unit = UnitCreator.CreateUnit(UNITNAME, UNITCLASS, UNITLEVEL);
			unit.Stats.Base[CoreStats.STR] = 14;

			Damage damage = CombatManager.CalculateDamage(unit, DamageType.Physical, null, 10);

			Assert.AreEqual(40, unit.Stats.Calculated[DerivedStats.ATK]);
			Assert.AreEqual(14, damage.Value);
		}

		[TestMethod]
		public void CombatManager_CalculateDamagePenalty()
		{
			IUnitEx unit = UnitCreator.CreateUnit(UNITNAME, UNITCLASS, UNITLEVEL);
			unit.Stats.Base[CoreStats.STR] = 8;

			Damage damage = CombatManager.CalculateDamage(unit, DamageType.Physical, null, 10);

			Assert.AreEqual(-20, unit.Stats.Calculated[DerivedStats.ATK]);
			Assert.AreEqual(9, damage.Value);
		}

		[TestMethod]
		public void CombatManager_ApplyDamageNeutral()
		{
			IUnitEx unit = UnitCreator.CreateUnit(UNITNAME, UNITCLASS, UNITLEVEL);
			unit.Stats.Base[CoreStats.STR] = 10;
			unit.Stats.Base[CoreStats.CON] = 10;
			unit.GetDetails<IVolatileStats>().Initialize();

			Damage damage = new Damage(DamageType.Physical, null, 10);
			int taken = CombatManager.ApplyDamage(unit, damage);

			Assert.AreEqual(0, unit.Stats.Calculated[DerivedStats.DEF]);
			Assert.AreEqual(10, taken);
		}

		[TestMethod]
		public void CombatManager_ApplyDamageResist()
		{
			IUnitEx unit = UnitCreator.CreateUnit(UNITNAME, UNITCLASS, UNITLEVEL);
			unit.Stats.Base[CoreStats.STR] = 14;
			unit.Stats.Base[CoreStats.CON] = 12;
			unit.GetDetails<IVolatileStats>().Initialize();

			Damage damage = new Damage(DamageType.Physical, null, 10);
			int taken = CombatManager.ApplyDamage(unit, damage);

			Assert.AreEqual(24, unit.Stats.Calculated[DerivedStats.DEF]);
			Assert.AreEqual(9, taken);
		}

		[TestMethod]
		public void CombatManager_ApplyDamageAmplify()
		{
			IUnitEx unit = UnitCreator.CreateUnit(UNITNAME, UNITCLASS, UNITLEVEL);
			unit.Stats.Base[CoreStats.STR] = 8;
			unit.Stats.Base[CoreStats.CON] = 8;
			unit.GetDetails<IVolatileStats>().Initialize();

			Damage damage = new Damage(DamageType.Physical, null, 10);
			int taken = CombatManager.ApplyDamage(unit, damage);

			Assert.AreEqual(-20, unit.Stats.Calculated[DerivedStats.DEF]);
			Assert.AreEqual(12, taken);
		}
	}
}
