using System;
using System.Text;
using RPGLibrary;
using DnDEngine.Combat;

namespace DnDEngine.Logging
{
	public static class Log
	{
		private static ILogger l = new Logger();

		public static void SetLogger(ILogger newLogger)
		{
			l = newLogger;
		}

		public static string ModifierFormat
		{
			get { return l.ModifierFormat; }
		}

		public static string DescribeUnit(IUnitEx unit)
		{
			return l.DescribeUnit(unit, LogType.Full);
		}

		public static string LogHit(HitResult result, LogType length)
		{
			return l.LogHit(result, length);
		}

		public static string LogAttack(AttackResult result, LogType length)
		{
			return l.LogAttack(result, length);
		}

		public static string LogSpell(SpellResult result, LogType length)
		{
			return l.LogSpell(result, length);
		}
	}
}
