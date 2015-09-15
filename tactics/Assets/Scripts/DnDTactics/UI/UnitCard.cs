using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using RPGEngine;
using DnDEngine;
using DnDTactics.Data;

namespace DnDTactics.UI
{
	/// <summary>
	/// Maps text fields to unit information.
	/// </summary>
	public class UnitCard : Universal.UI.UIElement
	{
		#region Inspector fields
		[SerializeField]
		private Text unitName;
		[SerializeField]
		private Text unitClass;
		[SerializeField]
		private Text level;
		[SerializeField]
		private Text exp;
		[SerializeField]
		private Text next;
		[SerializeField]
		private Text hp;
		[SerializeField]
		private Text mp;
		[SerializeField]
		private Text str;
		[SerializeField]
		private Text con;
		[SerializeField]
		private Text dex;
		[SerializeField]
		private Text @int;
		[SerializeField]
		private Text wis;
		[SerializeField]
		private Text cha;
		[SerializeField]
		private Text hit;
		[SerializeField]
		private Text ac;
		[SerializeField]
		private Text atk;
		[SerializeField]
		private Text def;
		[SerializeField]
		private Text mag;
		[SerializeField]
		private Text res;

		[SerializeField]
		private Image artwork;
		[SerializeField]
		private Image portrait;
		#endregion

		/// <summary>
		/// Gets the unit that this card is bound to.
		/// </summary>
		public IUnit Unit
		{
			get
			{
				return GetData<IUnit>();
			}
		}

		/// <summary>
		/// Displays the unit's details.
		/// </summary>
		public override void Draw()
		{
			base.Draw();

			if (unitName) unitName.text = Unit.Name;
			if (unitClass) unitClass.text = Unit.Class;
			if (level) level.text = Unit.Level.ToString();
			if (exp) exp.text = (Unit.Extensions as IDetails).Experience.ToString();
			if (next) next.text = (Unit.Extensions as IDetails).NextLevel.ToString();
			if (hp) hp.text = Unit.Stats[DerivedStats.Type.HP].ToString();
			if (mp) mp.text = Unit.Stats[DerivedStats.Type.MP].ToString();
			if (str) str.text = Unit.Stats[CoreStats.Type.STR].ToString();
			if (con) con.text = Unit.Stats[CoreStats.Type.CON].ToString();
			if (dex) dex.text = Unit.Stats[CoreStats.Type.DEX].ToString();
			if (@int) @int.text = Unit.Stats[CoreStats.Type.INT].ToString();
			if (wis) wis.text = Unit.Stats[CoreStats.Type.WIS].ToString();
			if (cha) cha.text = Unit.Stats[CoreStats.Type.CHA].ToString();
			if (hit) hit.text = Unit.Stats[DerivedStats.Type.HIT].ToString(Constants.ModifierFormat);
			if (ac) ac.text = Unit.Stats[DerivedStats.Type.AC].ToString();
			if (atk) atk.text = Unit.Stats[DerivedStats.Type.ATK].ToString(Constants.ModifierFormat);
			if (def) def.text = Unit.Stats[DerivedStats.Type.DEF].ToString(Constants.ModifierFormat);
			if (mag) mag.text = Unit.Stats[DerivedStats.Type.MAG].ToString(Constants.ModifierFormat);
			if (res) res.text = Unit.Stats[DerivedStats.Type.RES].ToString(Constants.ModifierFormat);

			//if (equipment)
			//{
			//	equipment.text = "";

			//	foreach (var slot in (Unit.Extensions as IEquipped).Slots)
			//	{
			//		foreach (var equip in slot.Value)
			//		{
			//			equipment.text += String.Format("[{0}] {1}\n", slot.Key, equip != null ? equip.Name : "");
			//		}
			//	}
			//}

			//if (spells)
			//{
			//	spells.text = "";

			//	foreach (var spell in (Unit.Extensions as ISpells).All)
			//	{
			//		spells.text += string.Format("{0}\n", DnDEngine.Combat.Magic.Spell.Compendium.GetSpell(spell).Name);
			//	}
			//}

			//if (enchantments)
			//{
			//	enchantments.text = "";

			//	foreach (var buff in (Unit.Extensions as IBuffs).All)
			//	{
			//		enchantments.text += string.Format("{0}\n", buff.Name);
			//	}
			//}

			if (artwork) artwork.sprite = DataManager.Classes[Unit.Class].artwork;
			if (portrait) portrait.sprite = DataManager.Classes[Unit.Class].portrait;
		}
	}
}