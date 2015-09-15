using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using RPGLibrary;
using DnDEngine;

namespace DnDEngineDriver
{
	class Program
	{
		private const string SAVEFILE = "save.bin";

		private const string UNITNAME = "Test Unit";
		private const string UNITCLASS = "Test Class";
		private const int UNITLEVEL = 10;
		private const string STATNAME = "TEST";
		private const int STATVALUE = 123;
		private const int ADDVALUE = 10;
		private const int MULTVALUE = 20;

		private static IFormatter formatter = new BinaryFormatter();
		private static Stream stream;

		private static void OpenWriteStream()
		{
			stream = new FileStream(SAVEFILE, FileMode.Create, FileAccess.Write, FileShare.None);
		}

		private static void OpenReadStream()
		{
			stream = new FileStream(SAVEFILE, FileMode.Open, FileAccess.Read, FileShare.Read);
		}

		private static void CloseStream()
		{
			stream.Close();
		}

		private static void Serialize(object obj)
		{
			OpenWriteStream();
			formatter.Serialize(stream, obj);
			CloseStream();
		}

		private static T Deserialize<T>()
		{
			OpenReadStream();
			T obj = (T)formatter.Deserialize(stream);
			CloseStream();
			return obj;
		}

		static void Main(string[] args)
		{
			IUnit unit = new DnDUnit(UNITNAME, UNITCLASS, UNITLEVEL);
			unit.Stats.Base[STATNAME] = STATVALUE;

			StatsModifier modifier1 = new StatsModifier();
			modifier1.Adds[STATNAME] = ADDVALUE;
			modifier1.Mults[STATNAME] = 0;

			StatsModifier modifier2 = new StatsModifier();
			modifier2.Adds[STATNAME] = ADDVALUE;
			modifier2.Mults[STATNAME] = MULTVALUE;

			unit.Modifiers.Add(modifier1);
			unit.Modifiers.Add(modifier2);

			IUnit unit1 = unit.Modifiers.Result;

			Console.WriteLine(unit.ToString());
			Console.WriteLine(unit1.ToString());

			Serialize(unit);
			IUnit unit2 = Deserialize<DnDUnit>().Modifiers.Result;

			Console.WriteLine(String.Format("{0}\t{1}", unit1.Name, unit2.Name));
			Console.WriteLine(String.Format("{0}\t{1}", unit1.Modifiers.Count, unit2.Modifiers.Count));
			Console.WriteLine(String.Format("{0}\t{1}", unit.Stats.Base[STATNAME], unit.Stats.Calculated[STATNAME]));
			Console.WriteLine(String.Format("{0}\t{1}", unit.Stats.Calculated[STATNAME], unit1.Stats.Calculated[STATNAME]));
			Console.WriteLine(String.Format("{0}\t{1}", unit1.Stats.Calculated[STATNAME], unit2.Stats.Calculated[STATNAME]));
			unit1.Stats.Base[STATNAME] += 10;
			unit2.Stats.Base[STATNAME] += 10;
			Console.WriteLine(String.Format("{0}\t{1}", unit1.Stats.Calculated[STATNAME], unit2.Stats.Calculated[STATNAME]));

			Dice dice = new Dice(6, 4);
			Console.WriteLine(dice.Roll());
			Console.WriteLine(dice.Roll());
			Console.WriteLine(dice.Roll());
			Console.WriteLine(dice.Roll());
			Console.ReadKey();
		}
	}
}
