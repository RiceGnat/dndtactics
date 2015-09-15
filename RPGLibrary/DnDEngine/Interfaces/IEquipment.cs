using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPGLibrary;

namespace DnDEngine
{
	public interface IEquipment : IDecorator<IUnit>, ICatalogable
	{
	}
}
