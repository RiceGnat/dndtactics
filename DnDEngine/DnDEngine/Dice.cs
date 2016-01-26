using System;
using RPGLibrary;

namespace DnDEngine
{
	public enum DiceType
	{
		D4 = 4,
		D6 = 6,
		D8 = 8,
		D10 = 10,
		D12 = 12,
		D20 = 20
	}

	public sealed class Dice : IRandom<int>
	{
		private RNG rng;

		public int Count { get; private set; }

		public int Sides { get { return rng.Max; } }

		public DiceRoll Result { get; private set; }

		public DiceRoll Roll()
		{
			int[] rolls = new int[Count];

			for (int i = 0; i < Count; i++)
			{
				rolls[i] = rng.Generate();
			}

			Result = new DiceRoll(this, rolls);

			return Result;
		}

		#region IRandom
		int IRandom<int>.LastResult
		{
			get { return Result.Total; }
		}

		int IRandom<int>.Generate()
		{
			Roll();
			return Result.Total;
		}
		#endregion

		public Dice(int sides, int count = 1)
		{
			rng = new RNG(1, sides);
			Count = count;
		}

		public Dice(DiceType type, int count = 1)
			: this((int)type, count) { }

		public override string ToString()
		{
			return String.Format("{0}d{1}", Count, Sides);
		}
	}
}
