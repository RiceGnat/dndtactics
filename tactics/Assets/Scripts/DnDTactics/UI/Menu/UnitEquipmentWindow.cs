using UnityEngine;
using UnityEngine.UI;
using System;
using RPGEngine;
using DnDEngine;
using DnDEngine.Items;
using Universal.UI;

namespace DnDTactics.UI
{
	/// <summary>
	/// Shows and manages unit's equipment. Uses the Universal.UI.Window object's Data property.
	/// </summary>
	[RequireComponent(typeof(RectTransform))]
	public class UnitEquipmentWindow : Window
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
			base.Draw();

			EventButton button, prev = null;
			RectTransform rectTransform;
			float offset = 0;

			// Draw equipment panel
			foreach (var slot in UnitEquipment.Slots)
			{
				foreach (Equipment equip in slot.Value)
				{
					button = Instantiate<EventButton>(itemButton);

					// Set button name and text
					button.name = equip != null ? equip.Name : "(Empty)";
					button.SetText(String.Format("[{0}] {1}", slot.Key, button.name));

					// Adjust item offset and container height
					rectTransform = button.GetComponent<RectTransform>();
					Container.Append(rectTransform, offset);
					offset += rectTransform.rect.height;

					// Add button to list
					Buttons.Add(button);

					// Show button
					button.gameObject.SetActive(true);

					// Set button navigation
					if (prev != null) button.Selectable.BindNavigation(prev.Selectable);
					prev = button;
				}
			}
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

		protected override void Start()
		{
			base.Start();

			itemButton.gameObject.SetActive(false);
		}

		protected override void Update()
		{
			base.Update();

			if (IsActivated)
			{

			}
		}
	}
}