﻿using System;

namespace RPGEngine
{
    /// <summary>
    /// Class to simulate dice rolls
    /// </summary>
    public class Dice
    {
        /// <summary>
        /// Defines die presets.
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// Default null value
            /// </summary>
            None,
            /// <summary>
            /// Defines a d4
            /// </summary>
            D4 = 4,
            /// <summary>
            /// Defines a d4
            /// </summary>
            D6 = 6,
            /// <summary>
            /// Defines a d4
            /// </summary>
            D8 = 8,
            /// <summary>
            /// Defines a d10
            /// </summary>
            D10 = 10,
            /// <summary>
            /// Defines a d12
            /// </summary>
            D12 = 12,
            /// <summary>
            /// Defines a d20
            /// </summary>
            D20 = 20
        }

        private static Random rng;

        /// <summary>
        /// Gets the number of dice.
        /// </summary>
        public byte Count { get; private set; }

        /// <summary>
        /// Gets the number of sides of each die.
        /// </summary>
        public byte Sides { get; private set; }

        /// <summary>
        /// Gets the total result of the roll.
        /// </summary>
        public ushort Result { get; private set; }

        /// <summary>
        /// Gets an array of the individual die rolls.
        /// </summary>
        public byte[] Breakdown { get; private set; }

        /// <summary>
        /// Roll the dice
        /// </summary>
        /// <returns>Result of the roll</returns>
        public ushort Roll()
        {
            Result = 0;
            Breakdown = new byte[Count];
            for (int i = 0; i < Count; i++)
            {
                Breakdown[i] = (byte)(rng.Next(Sides) + 1);
                Result += Breakdown[i];
            }
            return Result;
        }

        /// <summary>
        /// Create a new preset die
        /// </summary>
        /// <param name="d">Die type</param>
        public Dice(Type d)
            : this((int)d) { }

        /// <summary>
        /// Create a new set of preset dice
        /// </summary>
        /// <param name="count">Number of dice</param>
        /// <param name="d">Die type</param>
        public Dice(int count, Type d)
            : this(count, (int)d) { }

        /// <summary>
        /// Create a new die with a specified number of sides
        /// </summary>
        /// <param name="sides">Number of sides</param>
        public Dice(int sides)
            : this(1, sides) { }

        /// <summary>
        /// Create a new set of dice with a specified number of sides
        /// </summary>
        /// <param name="count">Number of dice</param>
        /// <param name="sides">Number of sides on each die</param>
        public Dice(int count, int sides)
        {
            if (count < 1 || count > 255) throw new ArgumentOutOfRangeException("count", count, "Must have at least one die and no more than 255");
            if (sides < 1 || sides > 255) throw new ArgumentOutOfRangeException("sides", sides, "Number of sides must be from 1 to 255");

            Sides = (byte)sides;
            Count = (byte)count;
            if (rng == null) rng = new Random();
        }

        /// <summary>
        /// Prints a summary of the roll.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string result = Count + "d" + Sides;
            if (Result > 0)
            {
                result += ": ";
                for (int i = 0; i < Count; i++)
                {
                    result += Breakdown[i] + (i < Count - 1 ? "+" : "");
                }
            }
            return result;
        }
    }
}
