using System;
using System.Collections.Generic;

namespace DnDEngine
{
    public class UnitClass
    {
        private static Dictionary<string, UnitClass> classList = new Dictionary<string, UnitClass>();

        public static void AddClass(string name, CoreStats baseStats, CoreStats pointCosts)
        {
            UnitClass newClass = new UnitClass();

            newClass.Name = name;
            newClass.BaseStats = baseStats;
            newClass.PointCosts = pointCosts;

            try
            {
                classList.Add(name, newClass);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Class name already exists.", ex);
            }
        }

        public static UnitClass Get(string name)
        {
            try
            {
                return classList[name];
            }
            catch (KeyNotFoundException ex)
            {
                throw new ArgumentException("Class name does not exist.", ex);
            }
        }

        /// <summary>
        /// Gets the class's name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the object representing the class's base stats.
        /// </summary>
        public CoreStats BaseStats { get; private set; }

        /// <summary>
        /// Gets the object representing the cost to level each stat.
        /// </summary>
        public CoreStats PointCosts { get; private set; }

        private UnitClass() {}
    }
}
