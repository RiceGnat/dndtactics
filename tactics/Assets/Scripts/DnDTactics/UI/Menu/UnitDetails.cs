using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using DnDEngine;
using DnDEngine.Items;
using Universal.UI;

namespace DnDTactics.UI
{
	public class UnitDetails : UnitCard
	{
		#region Inspector fields
		[SerializeField]
		private EventButton activator;
		[SerializeField]
		private EventButton itemButton;
		[SerializeField]
		private Window equipmentPanel;
		#endregion

		private IEquipped unitEquipment { get { return Unit.Extensions as IEquipped; } }

		private RectTransform equipmentContainer { get { return equipmentPanel.GetComponent<ScrollRect>().content; } }

		public override void Draw()
		{
			base.Draw();

			EventButton button;
			RectTransform rectTransform;
			float offset = 0;
			foreach (var slot in unitEquipment.Slots)
			{
				foreach (Equipment equip in slot.Value)
				{
					button = Instantiate<EventButton>(itemButton);
					
					// Set button name and text
					button.name = equip != null ? equip.Name : "(Empty)";
					button.SetText(String.Format("[{0}] {1}", slot.Key, button.name));

					// Adjust item offset and container height
					rectTransform = button.GetComponent<RectTransform>();
					equipmentContainer.Append(rectTransform, offset);
					offset += rectTransform.rect.height;

					// Add button to list
					equipmentPanel.Buttons.Add(button);

					// Show button
					button.gameObject.SetActive(true);
				}
			}

			equipmentPanel.Draw();

			Activate();
		}

		public override void Clear()
		{
			base.Clear();

			// Clear panel events
			equipmentPanel.Clear();

			// Clear existing buttons
			equipmentPanel.ClearButtons();

			// Reset scroll height
			equipmentContainer.offsetMin = Vector2.Scale(equipmentContainer.offsetMin, new Vector2(1, 0));
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