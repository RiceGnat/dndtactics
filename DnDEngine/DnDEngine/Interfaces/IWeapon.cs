namespace DnDEngine
{
	public interface IWeapon : IEquipment
	{
		WeaponType Type { get; }
		DiceType Damage { get; }

		int Range { get; }

		int CritRange { get; }
		int CritMultiplier { get; }
	}
}
