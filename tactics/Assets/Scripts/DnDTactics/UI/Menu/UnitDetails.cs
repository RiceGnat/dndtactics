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
		private Window equipmentWindow;
		#endregion

		public override void Draw()
		{
			base.Draw();

			Activate();
		}

		public override void Clear()
		{
			base.Clear();

			ClearEvents();
		}

		protected override void Awake()
		{
			base.Awake();
		}
	}
}