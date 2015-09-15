using UnityEngine;
using UnityEngine.UI;
using System;
using RPGEngine;
using DnDEngine;
using DnDEngine.Items;
using Universal;
using Universal.UI;
using DnDTactics.Data;

namespace DnDTactics.UI
{
	/// <summary>
	/// Shows and manages unit's equipment. Uses the Universal.UI.Window object's Data property.
	/// </summary>
	[RequireComponent(typeof(ScrollRect))]
	public class UnitInventoryWindow : Selector
	{
		#region Inspector fields
		[SerializeField]
		private EventButton itemButton;
		#endregion

		private RectTransform container;
		private bool showCaravan = false;

		#region Properties
		/// <summary>
		/// Gets this window's content container.
		/// </summary>
		public RectTransform Container
		{
			get
			{
				if (container == null) container = GetComponent<ScrollRect>().content;
				return container;
			}
		}

		/// <summary>
		/// Gets the bound unit's inventory.
		/// </summary>
		public IInventory UnitInventory { get { return Data != null ? GetData<IUnit>().Extensions as IInventory : null; } }

		/// <summary>
		/// Gets whether or not the panel is showing the caravan.
		/// </summary>
		public bool IsCaravan { get { return showCaravan; } }
		#endregion

		#region Methods
		/// <summary>
		/// Draws the equipment list.
		/// </summary>
		public override void Draw()
		{
			EventButton button, prev = null;
			RectTransform rectTransform;
			float offset = 0;

			var list = showCaravan ? DataManager.Caravan : UnitInventory.All;
			Message = showCaravan ? "Caravan" : "Inventory";
			CommandLabels[Array.IndexOf<EventKey>(UIPanel.CapturedInputs, EventKey.ButtonY)] = showCaravan ? "Inventory" : "Caravan";
			CommandPanel.Bind(this);

			// Draw inventory panel
			foreach (var item in list)
			{
				button = Instantiate<EventButton>(itemButton);

				// Set button name and text
				button.Text = item.Name;
				button.Data = item;

				// Adjust item offset and container height
				rectTransform = button.GetComponent<RectTransform>();
				Container.Append(rectTransform, offset);
				offset += rectTransform.rect.height;

				// Add button to list
				Buttons.Add(button);

				// Show button
				button.gameObject.SetActive(true);

				// Set button navigation
				button.Base.BindNavigation(prev != null ? prev.Base : null);
				prev = button;
			}

			// Add empty placeholder if empty inventory
			if (Buttons.Count == 0)
			{
				button = Instantiate<EventButton>(itemButton);
				button.Text = "(No items)";
				Container.Append(button.GetComponent<RectTransform>());
				Buttons.Add(button);
				button.gameObject.SetActive(true);
			}

			base.Draw();
		}

		/// <summary>
		/// Clears the equipment list.
		/// </summary>
		public override void Clear()
		{
			base.Clear();
			ClearButtons();
			Container.CollapseUp();
		}
		#endregion

		#region Unity events
		protected override void Awake()
		{
			base.Awake();

			Delegates.Add(EventKey.ButtonY, () =>
			{
				showCaravan = !showCaravan;
				Deactivate();
				Refresh();
				Activate();
			});

			Delegates.Add(EventKey.Cancel, () =>
			{
				showCaravan = false;
			});
		}

		protected override void Start()
		{
			base.Start();

			itemButton.gameObject.SetActive(false);
		}
		#endregion
	}
}