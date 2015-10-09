using System;
using RPGLibrary.Utilities;
using DnDEngine;
using DnDEngine.Combat;
using DnDEngine.Logging;

namespace DnDEngineDriver
{
	class Program
	{
		private const string SAVEFILE = "save.bin";

		static void Main(string[] args)
		{
			IUnitEx unit = new UnitAdapter(new DnDUnit("Kaleina", "Fighter", 3));
			unit.Stats.Base[CoreStats.STR] = 14;
			unit.Stats.Base[CoreStats.CON] = 12;
			unit.Stats.Base[CoreStats.DEX] = 10;
			unit.Stats.Base[CoreStats.INT] = 12;
			unit.Stats.Base[CoreStats.WIS] = 10;
			unit.Stats.Base[CoreStats.CHA] = 11;

			BasicEquipment equip = new BasicEquipment();
			equip.Name = "Helmet";
			equip.Slot = EquipmentSlot.Head;
			equip.Adds[DerivedStats.DEF] = 10;
			equip.Mults[DerivedStats.HP] = 10;

			Weapon weapon = new Weapon();
			weapon.Name = "Longsword";
			weapon.Type = WeaponType.Longsword;
			weapon.Damage = DiceType.D6;
			weapon.CritRange = 19;
			weapon.CritMultiplier = 2;
			weapon.Adds[DerivedStats.ATK] = 20;

			unit.Equipment.Equip(weapon, 0);
			unit.Equipment.Equip(equip, 0);

			unit.Initialize();

			Console.WriteLine(Log.DescribeUnit(unit));
			foreach (IEquipment equipment in unit.GetDetails<IUnitEquipment>().AllEquipment)
			{
				Console.WriteLine(equipment.Name);
			}
			Console.ReadKey(true);

			Serializer.WriteToFile(SAVEFILE, unit);
			unit = Serializer.ReadFromFile<IUnitEx>(SAVEFILE);
			Console.WriteLine(Log.DescribeUnit(unit));
			unit.Stats.Base[CoreStats.STR]++;

			IUnitEx enemy = new UnitAdapter(new DnDUnit("Goblin", "Fighter", 5));
			enemy.Stats.Base[CoreStats.STR] = 14;
			enemy.Stats.Base[CoreStats.CON] = 10;
			enemy.Stats.Base[CoreStats.DEX] = 11;
			enemy.Stats.Base[CoreStats.INT] = 8;
			enemy.Stats.Base[CoreStats.WIS] = 9;
			enemy.Stats.Base[CoreStats.CHA] = 8;

			enemy.Initialize();

			Console.WriteLine(Log.DescribeUnit(enemy));
			Console.ReadKey(true);

			/*
			Dice dice = new Dice(DiceType.D8, 4);
			Damage d = new Damage(DamageType.Physical, EnergyType.Fire, 5);
			DamageReport rpt = new DamageReport(unit, unit);
			rpt.AddDamage(dice.Roll(), d, 5);
			rpt.AddDamage(dice.Roll(), d, 5);
			rpt.AddDamage(dice.Roll(), d, 5);
			Console.Write((rpt as ILoggable).Full);
			Console.ReadKey();
			*/
			do
			{
				Console.WriteLine(CombatManager.RollAttack(unit, enemy).Full);
			} while (Console.ReadKey(true).Key != ConsoleKey.Q);

			Spell bolt = new Spell();
			bolt.Name = "Lightning Bolt";
			bolt.Type = CastType.Line;
			bolt.Magic = MagicType.Arcane;
			bolt.Energy = EnergyType.Lightning;
			bolt.Damage = DiceType.D8;
			bolt.Save = CoreStats.DEX;

			do
			{
				Console.WriteLine(CombatManager.RollSpell(unit, bolt, enemy).Full);
			} while (Console.ReadKey(true).Key != ConsoleKey.Q);

		}
	}
}
