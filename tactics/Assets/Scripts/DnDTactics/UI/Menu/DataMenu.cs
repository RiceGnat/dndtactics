using UnityEngine;
using System.Collections;
using Universal.UI;
using DnDTactics.Data;

namespace DnDTactics.UI
{
	public class DataMenu : Universal.UI.UIPanel
	{
		#region Inspector fields
		[SerializeField]
		private EventButton saveButton;
		[SerializeField]
		private EventButton loadButton;
		#endregion

		protected override void Awake()
		{
			base.Awake();

			saveButton.Base.onClick.AddListener(DataManager.Save);
		}
	}
}