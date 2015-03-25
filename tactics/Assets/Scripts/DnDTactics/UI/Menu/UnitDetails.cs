using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
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

		public override void Draw()
		{
			base.Draw();

			

			Activate();
		}

		public override void Clear()
		{
			base.Clear();

			// Clear panel events
			equipmentPanel.Clear();

			// Clear existing buttons
			equipmentPanel.ClearButtons();

		}

		protected override void Awake()
		{
			base.Awake();
		}
	}
}