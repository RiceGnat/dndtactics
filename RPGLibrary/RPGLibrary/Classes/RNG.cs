using System;

namespace RPGLibrary
{
	public class RNG : IRandom<int>
	{
		private Random rng = new Random();

		public int Min { get; private set; }
		public int Max { get; private set; }

		public int LastResult { get; private set; }

		public int Generate()
		{
			return LastResult = rng.Next(Min, Max);
		}

		public RNG(int max) : this(0, max) { }

		public RNG(int min, int max)
		{
			Min = min;
			Max = max;
		}
	}
}
