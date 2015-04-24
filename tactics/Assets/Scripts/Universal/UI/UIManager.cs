using UnityEngine;
using System.Collections;

namespace Universal.UI
{
	public sealed class UIManager : MonoBehaviour
	{
		#region Inspector fields
		[SerializeField]
		private CommandPanel commandBox;
		#endregion

		private static UIManager instance;

		public static CommandPanel CommandBox { get { return instance.commandBox; } }

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