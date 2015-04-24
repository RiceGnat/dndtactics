using UnityEngine;
using UnityEngine.UI;
using System;
using RPGEngine;
using DnDEngine;
using DnDEngine.Items;
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

		/// <summary>
		/// Gets this window's bound unit.
		/// </summary>
		public IUnit Unit
		{
			get
			{
				return Data as IUnit;
			}
		}

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
		public IInventory UnitInventory { get { return Unit != null ? Unit.Extensions as IInventory : null; } }

		/// <summary>
		/// Gets whether or not the panel is showing the caravan.
		/// </summary>
		public bool IsCaravan { get { return showCaravan; } }

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
			Container.Collapse();
		}

		/// <summary>
		/// Redraws the window while preserving data and state.
		/// </summary>
		public override void Refresh()
		{
			bool caravan = showCaravan;
			base.Refresh();
			showCaravan = caravan;
		}

		protected override void Awake()
		{
			base.Awake();

			//ButtonY += () =>
			//{
			//	showCaravan = !showCaravan;
			//	Deactivate();
			//	Refresh();
			//	Activate();
			//};
		}

		protected override void Start()
		{
			base.Start();

			itemButton.gameObject.SetActive(false);
		}
	}
}