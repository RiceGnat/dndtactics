using UnityEngine;
using Universal.UI;

namespace DnDTactics.UI
{
	public sealed class UIManager : MonoBehaviour
	{
		#region Inspector fields
		[SerializeField]
		private Menu gameMenu;
		[SerializeField]
		private UnitDetails unitDetails;
		#endregion

		private static UIManager instance;

		#region Properties
		public static Menu GameMenu { get { return instance.gameMenu; } }

		public static UnitDetails UnitDetailsPage { get { return instance.unitDetails; } }
		#endregion

		#region Unity events
		void Awake()
		{
			if (instance != null)
			{
				// Selfdestruct if there is already a UIManager present
				Destroy(gameObject);
			}
			else
			{
				instance = this;
			}
		}
		#endregion
	}
}