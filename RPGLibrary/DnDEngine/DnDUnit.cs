using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DnDEngine
{
	[Serializable]
	public partial class DnDUnit : RPGLibrary.UnitBase
	{
		protected override void Link()
		{
			base.Link();

			if (details == null) details = new UnitDetails(this);
			else GetDetails<UnitDetails>().Rebind(this);
		}

		public DnDUnit(string name, string unitClass, int level)
			: base (name, unitClass, level, new DnDStats())
		{
		}
	}
}
