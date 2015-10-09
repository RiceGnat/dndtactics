using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPGLibrary;
using DnDEngine.Logging;

namespace DnDEngine.Combat
{
	public enum DamageType
	{
		Physical,
		Magical,
		LifeLoss
	}

	public struct Damage
	{
		public DamageType Type { get; private set; }
		public EnergyType? Energy { get; private set; }
		public int Value { get; private set; }

		public Damage(DamageType type, EnergyType? energy, int value)
			: this()
		{
			Type = type;
			Energy = energy;
			Value = value;
		}
	}

	[Serializable]
	public class DamageReport : ILoggable
	{
		public string Source { get; private set; }
		public string Target { get; private set; }

		private List<DiceResult> rolls = new List<DiceResult>();
		private List<Damage> damageDealt = new List<Damage>();
		private List<int> damageTaken = new List<int>();

		public int TotalDealt { get { return damageDealt.Sum(x => x.Value); } }
		public int TotalTaken { get { return damageTaken.Sum(); } }

		public void AddDamage(DiceResult roll, Damage dealt, int taken)
		{
			rolls.Add(roll);
			damageDealt.Add(dealt);
			damageTaken.Add(taken);
		}

		public DamageReport(IUnit source, IUnit target)
		{
			Source = source.Name;
			Target = target.Name;
		}

		string ILoggable.Inline
		{
			get
			{
				return String.Format("{0} takes {1} damage from {2}.", Source, TotalTaken, Target);
			}
		}

		string ILoggable.Full
		{
			get
			{
				StringBuilder s = new StringBuilder();
				int totalLost = 0;

				for (int i = 0; i < rolls.Count; i++)
				{
					s.AppendFormat("{0} rolls {1}, ", Source, rolls[i].RollsString);
					if (damageDealt[i].Energy == EnergyType.Positive || damageDealt[i].Energy == EnergyType.Negative)
					{
						if (damageTaken[i] < 0)
						{
							s.AppendFormat("healing {0} for {1}.", Target, -damageTaken[i]);
						}
						else if (damageTaken[i] > 0)
						{
							s.AppendFormat("causing {0} to lose {1} health ({2} total).", Target, damageTaken[i], totalLost += damageTaken[i]);
						}
						else s.Append("with no effect.");
						s.AppendLine();
					}
					else
					{
						s.AppendLine(String.Format("dealing {0}{1} damage.", damageDealt[i].Value, damageDealt[i].Energy != null ? " " + damageDealt[i].Energy : ""));
						s.AppendLine(String.Format("{0} {1} {2} health ({3} total).", Target, damageTaken[i] < 0 ? "gains" : "loses", damageTaken[i], totalLost += damageTaken[i]));
					}
				}

				return s.ToString();
			}
		}
	}
}
