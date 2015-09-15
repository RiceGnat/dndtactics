using RPGLibrary;

namespace DnDEngine
{
	public static class CombatManager
	{
		public static Damage CalculateDamage(IUnit unit, DamageType type, EnergyType energy, int baseValue)
		{
			// Damage calculation goes here
			return new Damage(unit, type, energy, baseValue);
		}

		public static int ApplyDamage(IUnit unit, Damage damage)
		{
			IVolatileStats unitVals = unit.GetDetails<IVolatileStats>();

			int initial = unitVals.CurrentHP;

			// Damage reduction formula goes here
			unitVals.CurrentHP -= damage.Value;

			return initial - unitVals.CurrentHP;
		}
	}
}
