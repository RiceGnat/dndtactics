using System;
using System.Text;
using RPGEngine;

namespace DnDEngine
{
	/// <summary>
	/// Represents a unit based on the DnD 3.5e stat system.
	/// </summary>
	[Serializable]
	public partial class DnDUnit : Unit
	{
		#region Linked stats and evaluated unit
		[NonSerialized]
		private IUnit evaluated;
		[NonSerialized]
		private IStats stats;
		[NonSerialized]
		private IStats statsAdd;
		[NonSerialized]
		private IStats statsMult;
		#endregion

		#region Extensions data
		private UnitExtensions extensions;
		private ClassType classEnum;
		private GenderType gender;
		private BodyType body;
		#endregion

		/// <summary>
		/// Gets this unit's core stats.
		/// </summary>
		public CoreStats Core { get; private set; }
		/// <summary>
		/// Gets this unit's derived stats.
		/// </summary>
		public DerivedStats Derived { get; private set; }

		/// <summary>
		/// Gets or sets this unit's experience points.
		/// </summary>
		public uint Experience { get; set; }

		/// <summary>
		/// Gets or sets this unit's unused stat points.
		/// </summary>
		public uint StatPoints { get; set; }

		#region Override properties inherited from RPGEngine.Unit
		/// <summary>
		/// Gets this unit's class as a string.
		/// </summary>
		public override string Class
		{
			get { return classEnum.ToString(); }
			protected set { }
		}
		/// <summary>
		/// Gets this unit's level.
		/// </summary>
		public override ushort Level
		{
			get { return (ushort)Stats[CoreStats.Type.LVL]; }
			protected set { }
		}

		/// <summary>
		/// Gets this unit's base stats (without any modifiers).
		/// </summary>
		public override IStats BaseStats
		{
			get { return stats; }
		}
		/// <summary>
		/// Gets this unit's stats.
		/// </summary>
		public override IStats Stats
		{
			get { return evaluated != null ? evaluated.Stats : stats; }
		}
		/// <summary>
		/// Gets the total additive modifiers applied to this unit's stats.
		/// </summary>
		public override IStats StatModsAdditive { get { return evaluated != null ? evaluated.StatModsAdditive : statsAdd; } }
		/// <summary>
		/// Gets the total multiplicative modifiers applied to this unit's stats.
		/// </summary>
		public override IStats StatModsMultiplicative { get { return evaluated != null ? evaluated.StatModsMultiplicative : statsMult; } }

		/// <summary>
		/// Gets a DnDUnit.UnitExtensions object containing additional properties or methods. Cast to supported interfaces to access different properties.
		/// </summary>
		public override object Extensions { get { return evaluated != null ? evaluated.Extensions : extensions; } }

		/// <summary>
		/// Evaluate and link all decorators assigned to this object.
		/// </summary>
		public override void Evaluate()
		{
			evaluated = null;
			IUnit unit = this;
			foreach (IDecorator equip in (extensions as IEquipped).All)
			{
				unit = equip.Bind(unit);
			}

			foreach (IDecorator buff in (unit.Extensions as IBuffs).All)
			{
				unit = buff.Bind(unit);
			}

			if (unit != this) evaluated = unit;
		}
		#endregion

		#region Expose extension interfaces for convenience
		/// <summary>
		/// Gets additional details about this unit
		/// </summary>
		public IDetails Details { get { return Extensions as IDetails; } }
		/// <summary>
		/// Gets this unit's inventory.
		/// </summary>
		public IInventory Inventory { get { return Extensions as IInventory; } }
		/// <summary>
		/// Gets this unit's equipment
		/// </summary>
		public IEquipped Equipped { get { return Extensions as IEquipped; } }
		/// <summary>
		/// Gets the buffs and debuffs applied to this unit
		/// </summary>
		public IBuffs Buffs { get { return Extensions as IBuffs; } }
		/// <summary>
		/// Gets this unit's known spells
		/// </summary>
		public ISpells Spells { get { return Extensions as ISpells; } }
		#endregion

		private void LinkStats()
		{
			Derived = new DerivedStats(Core);
			stats = new StatsRouter(Core, Derived);
			statsAdd = RPGEngine.Stats.Zero;
			statsMult = RPGEngine.Stats.Zero;
		}

		/// <summary>
		/// Relinks non-serialized dynamic properties after deserialization.
		/// </summary>
		public void Relink()
		{
			LinkStats();
			extensions.LinkUnit(this);
		}

		#region Constructors
		/// <summary>
		/// Creates a new DnDUnit object with the specified parameters. Name is set to the class.
		/// </summary>
		/// <param name="class">The unit's class.</param>
		/// <param name="coreStats">The unit's core stats.</param>
		/// <param name="body">(optional) The unit's body shape. This is used to set up equipment slots.</param>
		/// <param name="gender">(optional) The unit's gender.</param>
		public DnDUnit(ClassType @class, CoreStats coreStats, BodyType body = BodyType.Inanimate, GenderType gender = GenderType.None)
			: this(@class.ToString(), @class, coreStats, body, gender) { }

		/// <summary>
		/// Creates a new DnDUnit object with the specified parameters.
		/// </summary>
		/// <param name="name">The unit's name.</param>
		/// <param name="class">The unit's class.</param>
		/// <param name="coreStats">The unit's core stats.</param>
		/// <param name="body">(optional) The unit's body shape. This is used to set up equipment slots.</param>
		/// <param name="gender">(optional) The unit's gender.</param>
		public DnDUnit(string name, ClassType @class, CoreStats coreStats, BodyType body = BodyType.Inanimate, GenderType gender = GenderType.None)
		{
			Name = name;
			classEnum = @class;
			this.body = body;
			this.gender = gender;
			Core = coreStats;

			extensions = new UnitExtensions(this);
			LinkStats();

			Experience = (uint)(Level * (Level - 1)) * 50;
		}
		#endregion

		/// <summary>
		/// Gets a short summary of this unit.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return Describe(this).Summary;
		}

		/// <summary>
		/// Gets print-friendly descriptions of the given unit.
		/// </summary>
		/// <param name="unit">The unit to be described.</param>
		/// <returns>ILoggable object containing the summary and full descriptions.</returns>
		public static ILoggable Describe(IUnit unit)
		{
			var gender = (unit.Extensions as IDetails).Gender;
			string summary = String.Format("{0} (Level {1} {2}{3})", unit.Name, unit.Level, gender != GenderType.None ? gender + " " : "", unit.Class);

			StringBuilder full = new StringBuilder(summary);
			full.AppendLine();
			full.AppendFormat("HP {0}\tMP {1}\tHIT {2}\tAC {3}\tMOV {4}",
				unit.Stats[DerivedStats.Type.HP],
				unit.Stats[DerivedStats.Type.MP],
				unit.Stats[DerivedStats.Type.HIT].ToString(Constants.ModifierFormat),
				unit.Stats[DerivedStats.Type.AC],
				unit.Stats[CoreStats.Type.MOV]);
			full.AppendLine();
			full.AppendLine();
			full.AppendFormat("STR {0}\tCON {1}\tDEX {2}", unit.Stats[CoreStats.Type.STR], unit.Stats[CoreStats.Type.CON], unit.Stats[CoreStats.Type.DEX]);
			full.AppendLine();
			full.AppendFormat("INT {0}\tWIS {1}\tCHA {2}", unit.Stats[CoreStats.Type.INT], unit.Stats[CoreStats.Type.WIS], unit.Stats[CoreStats.Type.CHA]);
			full.AppendLine();
			full.AppendLine();
			full.AppendFormat("ATK {0}\tDEF {1}",
				unit.Stats[DerivedStats.Type.ATK].ToString(Constants.ModifierFormat),
				unit.Stats[DerivedStats.Type.DEF].ToString(Constants.ModifierFormat));
			full.AppendLine();
			full.AppendFormat("MAG {0}\tRES {1}",
				unit.Stats[DerivedStats.Type.MAG].ToString(Constants.ModifierFormat),
				unit.Stats[DerivedStats.Type.RES].ToString(Constants.ModifierFormat));

			IEquipped equipList = unit.Extensions as IEquipped;
			if (equipList.SlotCount > 0)
			{
				full.AppendLine();
				full.AppendLine();
				full.Append("Equipment");
				foreach (var kvp in equipList.Slots)
				{
					full.AppendLine();
					full.AppendFormat("\t{0}", kvp.Key);
					foreach (var e in kvp.Value)
					{
						full.AppendLine();
						full.Append("\t\t");
						if (e != null) full.AppendFormat("{0} ({1})", e.Name, e.Description);
						else full.Append("(Empty)");
					}
				}
			}

			IBuffs buffs = unit.Extensions as IBuffs;
			if (buffs.Count > 0)
			{
				full.AppendLine();
				full.AppendLine();
				full.Append("Buffs/Debuffs");
				foreach (var b in buffs.All)
				{
					full.AppendLine();
					full.AppendFormat("\t{0} ({1})", b.Name, b.Description);
				}
			}

			return new LoggableString(summary, full.ToString());
		}
	}
}
