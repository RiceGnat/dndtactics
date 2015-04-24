using UnityEngine;
using System.Collections;

namespace Universal.UI
{
	public sealed class UIManager : MonoBehaviour
	{
		#region Inspector fields
		[SerializeField]
		private Menu menu;
		[SerializeField]
		private CommandPanel commandBox;
		#endregion

		private static UIManager instance;

		public static CommandPanel CommandBox { get { return instance.commandBox; } }

		public static Menu GameMenu { get { return instance.menu; } }

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

		void Start()
		{
			GameMenu.Activate();
		}
		#endregion
	}
}