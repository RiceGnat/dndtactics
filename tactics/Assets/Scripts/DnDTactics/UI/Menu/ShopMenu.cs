using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using DnDEngine.Items;
using Universal.UI;

namespace DnDTactics.UI
{
	public class ShopMenu : Universal.UI.UIPanel
	{
		#region Inspector fields
		[SerializeField]
		private EventButton itemButton;
		[SerializeField]
		private Window itemPanel;
		[SerializeField]
		private ScrollRect scrollRegion;
		[SerializeField]
		private EquipmentCard equipmentCard;
		#endregion

		private Equipment[] equipList;
		private int lastSelected;

		private RectTransform itemContainer { get { return itemPanel.GetComponent<ScrollRect>().content; } }

		private void BindDetails(int index)
		{
			lastSelected = index;
			equipmentCard.Bind(equipList[index]);
			equipmentCard.Draw();
		}

		private void ShowConfirm(int index, object data)
		{
			Deactivate();
			ModalDialog.Confirm(String.Format("Purchase {0}?", equipList[lastSelected].Name), Buy, ReturnToList);
		}

		private void Buy()
		{
			DataManager.Caravan.Add(equipList[lastSelected].Clone());
			ReturnToList();
		}

		private void ReturnToList()
		{
			var last = itemPanel.Buttons[lastSelected].gameObject;
			Activate();
			EventSystem.current.SetSelectedGameObject(last);
		}

		/// <summary>
		/// Selects the first item.
		/// </summary>
		public override void Activate()
		{
			itemPanel.Activate();
		}

		/// <summary>
		/// Cancels out of the shop.
		/// </summary>
		public override void Deactivate()
		{
			itemPanel.Deactivate();
		}

		#region Unity events
		protected override void Awake()
		{
			base.Awake();

			// This menu will be static so it only needs to be drawn once
			equipList = new Equipment[(int)Equipment.Armory.NextID - 1];

			EventButton button, prev = null;
			RectTransform rectTransform;
			float offset = 0;
			for (uint i = 0; i < equipList.Length; i++)
			{
				equipList[i] = Equipment.Armory.GetNewEquipment(i + 1);

				button = Instantiate(itemButton);

				// Set button name and text
				button.SetText(String.Format("{0} ({1})", equipList[i].Name, equipList[i].Description));

				// Adjust item offset and container height
				rectTransform = button.GetComponent<RectTransform>();
				itemContainer.Append(rectTransform, offset);
				offset += rectTransform.rect.height;

				// Add button to list
				itemPanel.Buttons.Add(button);

				// Show button
				button.gameObject.SetActive(true);

				// Set button navigation
				button.Selectable.BindNavigation(prev != null ? prev.Selectable : null);
				prev = button;
			}

			itemPanel.Clicked += ShowConfirm;
			itemPanel.Selected += BindDetails;
			itemPanel.Canceled += OnCanceled;
			itemPanel.Draw();
		}

		protected override void Start()
		{
			base.Start();
			itemButton.gameObject.SetActive(false);
		}
		#endregion
	}
}