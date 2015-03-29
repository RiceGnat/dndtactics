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
		private Window equipmentPanel;
		[SerializeField]
		private Window inventoryPanel;
		#endregion

		private IEquipped unitEquipment { get { return Unit.Extensions as IEquipped; } }

		private RectTransform equipmentContainer { get { return equipmentPanel.GetComponent<ScrollRect>().content; } }

		public override void Draw()
		{
			base.Draw();

			// Draw equipment panel
			equipmentPanel.Data = Unit;
			equipmentPanel.Draw();
			
			// Draw inventory panel
			inventoryPanel.Data = Unit;
			inventoryPanel.Draw();

			// Activate input handlers
			Activate();
		}

		public override void Clear()
		{
			base.Clear();

			// Deactivate input handlers
			Deactivate();

			// Clear equipment panel
			equipmentPanel.Clear();
			inventoryPanel.Clear();
		}

		public override void Activate()
		{
			base.Activate();

			EventSystem.current.SetSelectedGameObject(null);
		}

		#region Unity events
		protected override void Awake()
		{
			base.Awake();

			// Bind window flow events
			Submitted += () =>
			{
				Deactivate();
				// Activate equipment if A button is pressed
				equipmentPanel.Activate();
			};

			equipmentPanel.Canceled += () =>
			{
				equipmentPanel.Deactivate();
				Activate();
			};

			equipmentPanel.BumperR += () =>
			{
				equipmentPanel.Deactivate();
				inventoryPanel.Activate();
			};

			inventoryPanel.Canceled += () =>
			{
				inventoryPanel.Deactivate();
				Activate();
			};

			inventoryPanel.BumperL += () =>
			{
				inventoryPanel.Deactivate();
				equipmentPanel.Activate();
			};
		}

		protected override void Update()
		{
			base.Update();

			if (IsActivated)
			{

			}
		}
		#endregion
	}
}