using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RPGLibrary;

namespace RPGLibraryTest
{
	[TestClass]
	public class DecoratorManagerTest
	{
		private const string UNITNAME = "Test Unit";
		private const string UNITCLASS = "Test Class";
		private const int UNITLEVEL = 10;

		private class TestDecorator : UnitDecorator { }
		private class NotIUnitDecorator : IDecorator<IUnit>
		{
			public IUnit Target { get { return null; } }

			public void Bind(IUnit target) { }

			public IList<IDecorator<IUnit>> Children { get { return null; } }
		}

		[TestMethod]
		public void DecoratorManager_Initialization()
		{
			UnitBase unit = new UnitBase(UNITNAME, UNITCLASS, UNITLEVEL);
			DecoratorManager<IUnit> manager = new DecoratorManager<IUnit>(unit);

			Assert.AreEqual(unit, manager.Target);
		}

		[TestMethod]
		public void DecoratorManager_Add()
		{
			UnitBase unit = new UnitBase(UNITNAME, UNITCLASS, UNITLEVEL);
			DecoratorManager<IUnit> manager = new DecoratorManager<IUnit>(unit);
			TestDecorator decorator = new TestDecorator();

			manager.Add(decorator);
			Assert.AreEqual(unit, decorator.Target);
		}

		[TestMethod]
		public void DecoratorManager_AddMultiple()
		{
			UnitBase unit = new UnitBase(UNITNAME, UNITCLASS, UNITLEVEL);
			DecoratorManager<IUnit> manager = new DecoratorManager<IUnit>(unit);
			TestDecorator decorator = new TestDecorator();
			TestDecorator decorator2 = new TestDecorator();

			manager.Add(decorator, decorator2);
			Assert.AreEqual(unit, decorator.Target);
			Assert.AreEqual(decorator, decorator2.Target);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void DecoratorManager_AddWrongInterface()
		{
			UnitBase unit = new UnitBase(UNITNAME, UNITCLASS, UNITLEVEL);
			DecoratorManager<IUnit> manager = new DecoratorManager<IUnit>(unit);
			NotIUnitDecorator decorator = new NotIUnitDecorator();

			manager.Add(decorator);
		}

		[TestMethod]
		public void DecoratorManager_Count()
		{
			UnitBase unit = new UnitBase(UNITNAME, UNITCLASS, UNITLEVEL);
			DecoratorManager<IUnit> manager = new DecoratorManager<IUnit>(unit);

			manager.Add(new TestDecorator());
			Assert.AreEqual(1, manager.Count);
			manager.Add(new TestDecorator());
			Assert.AreEqual(2, manager.Count);
			manager.Add(new TestDecorator());
			Assert.AreEqual(3, manager.Count);
		}

		[TestMethod]
		public void DecoratorManager_Last()
		{
			UnitBase unit = new UnitBase(UNITNAME, UNITCLASS, UNITLEVEL);
			DecoratorManager<IUnit> manager = new DecoratorManager<IUnit>(unit);
			TestDecorator decorator = new TestDecorator();
			TestDecorator decorator2 = new TestDecorator();

			Assert.AreEqual(unit, manager.Result);
			manager.Add(decorator);
			Assert.AreEqual(decorator, manager.Result);
			manager.Add(decorator2);
			Assert.AreEqual(decorator2, manager.Result);
		}

		[TestMethod]
		public void DecoratorManager_Remove()
		{
			UnitBase unit = new UnitBase(UNITNAME, UNITCLASS, UNITLEVEL);
			DecoratorManager<IUnit> manager = new DecoratorManager<IUnit>(unit);
			TestDecorator decorator = new TestDecorator();
			TestDecorator decorator2 = new TestDecorator();
			TestDecorator decorator3 = new TestDecorator();

			manager.Add(decorator);
			manager.Add(decorator2);
			manager.Add(decorator3);

			Assert.AreEqual(decorator2, decorator3.Target);
			manager.Remove(decorator2);
			Assert.AreEqual(decorator, decorator3.Target);
			manager.Remove(decorator);
			Assert.AreEqual(unit, decorator3.Target);
			manager.Remove(decorator3);
			Assert.AreEqual(0, manager.Count);
		}

		[TestMethod]
		public void DecoratorManager_Enumeration()
		{
			UnitBase unit = new UnitBase(UNITNAME, UNITCLASS, UNITLEVEL);
			DecoratorManager<IUnit> manager = new DecoratorManager<IUnit>(unit);
			TestDecorator[] decorators = new TestDecorator[3];
			decorators[0] = new TestDecorator();
			decorators[1] = new TestDecorator();
			decorators[2] = new TestDecorator();

			manager.Add(decorators[0]);
			manager.Add(decorators[1]);
			manager.Add(decorators[2]);

			int i = 0;
			foreach (IDecorator<IUnit> d in manager)
			{
				Assert.AreEqual(decorators[i], d);
				i++;
			}
		}
	}
}
