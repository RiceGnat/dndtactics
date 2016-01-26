using System;
using System.Collections.Generic;
using System.Linq;

namespace DnDEngine
{
	public struct DiceRoll
	{
		private int[] rolls;
		private string head;

		public IList<int> Rolls { get { return Array.AsReadOnly<int>(rolls); } }

		public string RollsString
		{
			get
			{
				return String.Format("<{0}:{1}>", head, String.Join("+", Array.ConvertAll<int, string>(rolls, x => x.ToString())));
			}
		}

		public int Total { get { return rolls.Sum(); } }

		public string TotalString
		{
			get
			{
				return String.Format("<{0}:{1}>", head, Total);
			}
		}

		public DiceRoll(Dice dice, int[] rolls)
			: this()
		{
			head = dice.ToString();
			this.rolls = rolls;
		}

		public override string ToString()
		{
			return RollsString;
		}
	}

}
