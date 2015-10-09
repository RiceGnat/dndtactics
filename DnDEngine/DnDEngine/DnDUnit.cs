using System;
using System.Text;
using RPGLibrary;

namespace DnDEngine
{
	[Serializable]
	public partial class DnDUnit : UnitBase
	{
		public override int Level
		{
			get
			{
				return Stats.Calculated[OtherStats.LVL];
			}
			set
			{
				baseStats[OtherStats.LVL] = value;
			}
		}

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
