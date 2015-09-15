using UnityEngine;
using System.Collections;
using Universal;
using Universal.UI;

namespace DnDTactics.UI
{
	public sealed class UIManager : SingletonMonoBehaviour
	{
		#region Singleton
		private static SingletonMonoBehaviour instance;

		protected override SingletonMonoBehaviour Instance
		{
			get { return instance; }
			set { instance = value; }
		}
		#endregion

		#region Inspector fields
		[SerializeField]
		private UIElement mainMenu;
		#endregion

		private void Start()
		{
			mainMenu.Focus();
		}
	}
}