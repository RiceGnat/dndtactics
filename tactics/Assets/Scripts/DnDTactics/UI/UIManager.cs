using UnityEngine;
using System.Collections;
using Universal.UI;

namespace DnDTactics.UI
{
	public sealed class UIManager : MonoBehaviour
	{
		#region Inspector fields
		private Window commandBox;
		#endregion

		private static UIManager instance;

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