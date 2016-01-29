using System;
using System.Text;
using DnDEngine.Logging;

namespace DnDEngine.Combat
{
	[Serializable]
	public class HitResult : ILoggable
	{
		public string Attacker { get; private set; }
		public string Defender { get; private set; }

		public DiceRoll Roll { get; private set; }
		public int AttackerBonus { get; private set; }
		public int DefenderAVD { get; private set; }
		public bool Hit { get; private set; }
		public bool CritRange { get; private set; }
		public DiceRoll CritRoll { get; private set; }
		public bool Crit { get; private set; }
		public bool CritConfirm { get; private set; }

		public HitResult(IUnitEx attacker, IUnitEx defender, int critRange)
		{
			Attacker = attacker.Name;
			Defender = defender.Name;

			AttackerBonus = attacker.Stats.Calculated[DerivedStats.HIT];
			DefenderAVD = defender.Stats.Calculated[DerivedStats.AVD];

			// Roll to-hit
			Dice hit = new Dice(DiceType.D20);
			Roll = hit.Roll();

			// If hit
			if (Roll.Total + AttackerBonus >= DefenderAVD)
			{
				Hit = true;

				// Roll for crit
				if (Roll.Total >= critRange)
				{
					Crit = true;
					CritRoll = hit.Roll();
					CritConfirm = CritRoll.Total >= critRange;
				}
			}
		}

		string ILoggable.Inline
		{
			get { return Log.LogHit(this, LogType.Inline); }
		}

		string ILoggable.Full
		{
			get { return Log.LogHit(this, LogType.Full); }
		}
	}
}
