using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using RPGEngine;
using DnDEngine.Items;
using Universal.UI;

namespace DnDTactics.UI
{
	/// <summary>
	/// Shows item info and options.
	/// </summary>
	[RequireComponent(typeof(EquipmentCard))]
	public class ItemPopout : Selector
	{
		#region Inspector fields
		[SerializeField]
		private Text description;
		[SerializeField]
		private EventButton useButton;
		[SerializeField]
		private EventButton equipButton;
		#endregion

		/// <summary>
		/// Gets the item for this window.
		/// </summary>
		public ICatalogable Item
		{
			get
			{
				return Data as ICatalogable;
			}
		}

		/// <summary>
		/// Displays item info and buttons for actions.
		/// </summary>
		public override void Draw()
		{
			if (Item != null)
			{
				// Show/hide buttons
				if (Item is Equipment)
				{
					Buttons[0] = equipButton;
					GetComponent<EquipmentCard>().Bind(Item as IEquipment);
					GetComponent<EquipmentCard>().Draw();
				}
				else
				{
					Buttons[0] = useButton;
				}

				Message = Item.Name;
				description.text = Item.Description;

				base.Draw();
			}
		}

		/// <summary>
		/// Clears the popout.
		/// </summary>
		public override void Clear()
		{
			base.Clear();


		}
	}
}