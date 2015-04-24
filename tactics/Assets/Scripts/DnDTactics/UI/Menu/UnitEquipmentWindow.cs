using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using RPGEngine;
using DnDEngine;
using DnDEngine.Items;
using Universal.UI;

namespace DnDTactics.UI
{
	/// <summary>
	/// Shows and manages unit's equipment. Uses the Universal.UI.Window object's Data property.
	/// </summary>
	[RequireComponent(typeof(ScrollRect))]
	public class UnitEquipmentWindow : Selector
	{
		#region Inspector fields
		[SerializeField]
		private EventButton itemButton;
		#endregion

		private RectTransform container;

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
		/// Gets the bound unit's equipment.
		/// </summary>
		public IEquipped UnitEquipment { get { return Unit != null ? Unit.Extensions as IEquipped : null; } }

		/// <summary>
		/// Draws the equipment list.
		/// </summary>
		public override void Draw()
		{
			EventButton button, prev = null;
			RectTransform rectTransform;
			float offset = 0;

			// Draw equipment panel
			foreach (var slot in UnitEquipment.Slots)
			{
				var index = 0;
				foreach (Equipment equip in slot.Value)
				{
					button = Instantiate<EventButton>(itemButton);

					// Set button name and text
					button.Text = String.Format("[{0}] {1}", slot.Key, equip != null ? equip.Name : "(Empty)");
					button.Data = new EquipIndex(slot.Key, index, equip);

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

					index++;
				}
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

		protected override void Awake()
		{
			base.Awake();

		}

		protected override void Start()
		{
			base.Start();

			itemButton.gameObject.SetActive(false);
		}
	}
}