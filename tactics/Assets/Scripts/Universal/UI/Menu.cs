using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Universal.UI
{
	/// <summary>
	/// Manages MenuItem associations and handles the drawing of their associated UI elements.
	/// </summary>
	public class Menu : UIPanel
	{
		[SerializeField]
		private List<MenuItem> items;
		[SerializeField]
		private bool manualButtonNavigation = false;

		private MenuItem selected;

		/// <summary>
		/// Gets a list of the menu items.
		/// </summary>
		public IList<MenuItem> Items
		{
			get { return items; }
		}

		private MenuItem FindMenuItem(Button button) {
			// Iterate through item list
			foreach (MenuItem item in items)
			{
				// Return item if button object matches
				if (item.button.Selectable == button) return item;
			}

			// If nothing found return null
			return null;
		}

		private void SetSelected(MenuItem item)
		{
			// Hide currently selected menu
			if (selected != null)
				selected.target.Hide();

			// Set selected menu
			selected = item;

			// Show new selected menu
			if (selected != null)
			{
				selected.target.Show();
			}
		}

		private void ItemSelected(Button sender, BaseEventData eventData)
		{
			SetSelected(FindMenuItem(sender));
		}

		private void ItemClicked()
		{
			Deactivate();
			// Clicked implies selected event has already occurred
			selected.target.Activate();
		}

		private void ItemExited()
		{
			selected.target.Deactivate();
			Activate();
			EventSystem.current.SetSelectedGameObject(selected.button.gameObject);
		}

		#region Unity events
		protected override void Start()
		{
			base.Start();

			MenuItem prev = null;
			foreach (MenuItem item in items)
			{
				// Hide target UI element initially
				item.target.Hide();

				// Bind events
				item.button.Select += ItemSelected;
				item.button.Selectable.onClick.AddListener(ItemClicked);
				item.target.Canceled += ItemExited;

				if (!manualButtonNavigation)
				{
					// Set button navigation
					var nav = item.button.Selectable.navigation;
					nav.mode = Navigation.Mode.Explicit;
					if (prev != null)
					{
						nav.selectOnUp = prev.button.Selectable;
						var prevNav = prev.button.Selectable.navigation;
						prevNav.selectOnDown = item.button.Selectable;
						prev.button.Selectable.navigation = prevNav;
					}
					item.button.Selectable.navigation = nav;
					prev = item;
				}
			}
		}
		#endregion
	}
}
