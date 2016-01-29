using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DnDEngine.Combat;

namespace DnDEngine.Logging
{
	public enum LogType {
		Inline, Full
	}
	public interface ILogger
	{
		string ModifierFormat { get; }
		string DescribeUnit(IUnitEx unit, LogType length);
		string LogHit(HitResult result, LogType length);
		string LogAttack(AttackResult result, LogType length);
		string LogSpell(SpellResult result, LogType length);
	}
}
