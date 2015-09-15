using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RPGLibrary;

namespace RPGLibraryTest
{
	[TestClass]
	public class UnitDecoratorTest
	{
		private const string UNITNAME = "Test Unit";
		private const string UNITCLASS = "Test Class";
		private const int UNITLEVEL = 10;

		private class TestDecorator : UnitDecorator { }

		[TestMethod]
		public void UnitDecorator_PassthroughBinding()
		{
			IUnit unit = new UnitBase(UNITNAME, UNITCLASS, UNITLEVEL);
			TestDecorator decorator = new TestDecorator();
			IUnit decoratedUnit = decorator as IUnit;

			decorator.Bind(unit);
			Assert.AreEqual(unit.Name, decoratedUnit.Name);
			Assert.AreEqual(unit.Class, decoratedUnit.Class);
			Assert.AreEqual(unit.Level, decoratedUnit.Level);
		}

		[TestMethod]
		public void UnitDecorator_NoChildren()
		{
			IDecorator<IUnit> decorator = new TestDecorator();

			Assert.IsNull(decorator.Children);
		}
	}
}
