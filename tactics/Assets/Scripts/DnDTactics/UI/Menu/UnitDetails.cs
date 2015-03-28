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

		private void ActivateEquipment()
		{
			Deactivate();
			equipmentPanel.Activate();
		}

		public override void Draw()
		{
			base.Draw();

			// Draw equipment panel
			equipmentPanel.Data = Unit;
			equipmentPanel.Draw();

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
				equipmentPanel.Activate();
			};

			equipmentPanel.Canceled += () =>
			{
				equipmentPanel.Deactivate();
				Activate();
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