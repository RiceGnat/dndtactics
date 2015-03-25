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

			equipList = new Equipment[(int)Equipment.Armory.NextID - 1];

			EventButton button, prev = null;
			RectTransform rectTransform;
			float offset;
			for (uint i = 0; i < equipList.Length; i++)
			{
				equipList[i] = Equipment.Armory.GetNewEquipment(i + 1);

				button = Instantiate(itemButton);

				button.transform.SetParent(scrollRegion.content.transform, false);

				button.name = equipList[i].Name;
				button.SetText(String.Format("{0} ({1})", equipList[i].Name, equipList[i].Description));

				rectTransform = button.GetComponent<RectTransform>();
				offset = rectTransform.rect.height * itemPanel.Buttons.Count;
				rectTransform.offsetMin -= new Vector2(0, offset);
				rectTransform.offsetMax -= new Vector2(0, offset);

				itemPanel.Buttons.Add(button);

				button.gameObject.SetActive(true);

				// Set button navigation
				var nav = button.Selectable.navigation;
				nav.mode = Navigation.Mode.Explicit;
				if (prev != null)
				{
					nav.selectOnUp = prev.Selectable;
					var prevNav = prev.Selectable.navigation;
					prevNav.selectOnDown = button.Selectable;
					prev.Selectable.navigation = prevNav;
				}
				button.Selectable.navigation = nav;
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