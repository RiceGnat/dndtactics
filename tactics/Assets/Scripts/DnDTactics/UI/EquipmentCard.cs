using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using RPGEngine;
using DnDEngine;
using DnDEngine.Items;

namespace DnDTactics.UI
{
	/// <summary>
	/// Maps text fields to equipment information.
	/// </summary>
	public class EquipmentCard : Universal.UI.UIPanel
	{
		#region Inspector fields
		[SerializeField]
		private Text equipName;
		[SerializeField]
		private Text slot;
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
		#endregion

		/// <summary>
		/// Gets the equipment that this card is bound to.
		/// </summary>
		public IEquipment Item { get; private set; }

		/// <summary>
		/// Gets whether or not the bound equipment is a weapon.
		/// </summary>
		public bool IsWeapon
		{
			get
			{
				return Item != null ? Item.Slot == Equipment.Type.Weapon : false;
			}
		}

		/// <summary>
		/// Binds this card to an equipment.
		/// </summary>
		/// <param name="equipment">Equipment to bind to</param>
		public void Bind(IEquipment equipment)
		{
			Item = equipment;
		}

		/// <summary>
		/// Displays the unit's details.
		/// </summary>
		public override void Draw()
		{
 			base.Draw();

			if (equipName) equipName.text = Item.Name;
			if (slot) slot.text = Item.Slot.ToString();
			if (hp) hp.text = Item.DerivedStatMods.HP.ToString(Constants.ModifierFormat);
			if (mp) mp.text = Item.DerivedStatMods.MP.ToString(Constants.ModifierFormat);
			if (str) str.text = Item.CoreStatMods.STR.ToString(Constants.ModifierFormat);
			if (con) con.text = Item.CoreStatMods.CON.ToString(Constants.ModifierFormat);
			if (dex) dex.text = Item.CoreStatMods.DEX.ToString(Constants.ModifierFormat);
			if (@int) @int.text = Item.CoreStatMods.INT.ToString(Constants.ModifierFormat);
			if (wis) wis.text = Item.CoreStatMods.WIS.ToString(Constants.ModifierFormat);
			if (cha) cha.text = Item.CoreStatMods.CHA.ToString(Constants.ModifierFormat);
			if (hit) hit.text = Item.DerivedStatMods.HIT.ToString(Constants.ModifierFormat);
			if (ac) ac.text = Item.DerivedStatMods.AC.ToString(Constants.ModifierFormat);
			if (atk) atk.text = Item.DerivedStatMods.ATK.ToString(Constants.ModifierFormat);
			if (def) def.text = Item.DerivedStatMods.DEF.ToString(Constants.ModifierFormat);
			if (mag) mag.text = Item.DerivedStatMods.MAG.ToString(Constants.ModifierFormat);
			if (res) res.text = Item.DerivedStatMods.RES.ToString(Constants.ModifierFormat);
		}
	}
}