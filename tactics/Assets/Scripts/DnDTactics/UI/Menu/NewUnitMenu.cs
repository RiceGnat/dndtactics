using UnityEngine;
using System;
using Universal;
using Universal.UI;
using DnDTactics.Data;

namespace DnDTactics.UI.Menu
{
	public class NewUnitMenu : UIElement
	{
		#region Inspector fields
		[SerializeField]
		private Repeater classRepeater;
		#endregion

		#region Fields
		private EventButton lastSelected;
		#endregion

		#region Methods
		public override void Draw()
		{
			// Replace class enum in the future with editor-defined classes
			classRepeater.Count = DataManager.Classes.Count;

			base.Draw();
		}

		public override void Focus()
		{
			classRepeater.Focus();
		}

		private void BindClass(UIElement element, int index)
		{
			//element.Data = DataManager.Classes.Values[index];
			element.name = element.GetData<ClassData>().name;

			EventButton button = element.GetComponent<EventButton>();

			button.ID = index;
			button.SetText(element.GetData<ClassData>().name);
			//button.OnClick += OpenUnitDetails;
		}
		#endregion

		#region Unity events
		protected override void Awake()
		{
			base.Awake();

			classRepeater.DataSource = DataManager.Classes.Values;
			classRepeater.OnDataBind += BindClass;

			classRepeater.CommandDelegates.Add(EventKey.Cancel, () =>
			{
				CommandDelegates.Raise(EventKey.Cancel);
			});
		}
		#endregion
	}
}