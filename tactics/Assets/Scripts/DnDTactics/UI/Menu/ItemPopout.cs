using UnityEngine;
using System.Collections;
using RPGEngine;
using Universal.UI;

namespace DnDTactics.UI
{
	/// <summary>
	/// Shows item info and options.
	/// </summary>
	[RequireComponent(typeof(EquipmentCard))]
	public class ItemPopout : Window
	{
		#region Inspector fields
		[SerializeField]
		private EventButton itemButton;
		[SerializeField]
		private RectTransform buttonArea;
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
			base.Draw();

			// Add buttons
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