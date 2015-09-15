using UnityEngine;
using System.Collections;
using Universal;
using Universal.UI;

namespace DnDTactics.UI.Menu
{
	public class MainMenu : UIElement
	{
		#region Inspector fields
		[SerializeField]
		private ButtonLink[] menuItems;
		#endregion

		#region Fields
		private UIElement selected;
		#endregion

		public override void Focus()
		{
			base.Focus();

			if (selected == null) menuItems[0].button.Select();
		}

		#region Unity events
		protected override void Start()
		{
			base.Start();

			EventButton prev = null;

			foreach (ButtonLink link in menuItems)
			{
				EventButton button = link.button;
				UIElement target = link.target;

				button.gameObject.SetActive(true);
				target.gameObject.SetActive(true);

				button.OnSelect += (EventButton sender) =>
				{
					if (selected != null)
					{
						selected.Clear();
						selected.Hide();
					}
					target.Show();
					target.Draw();
					selected = target;
				};

				button.OnClick += (EventButton sender) =>
				{
					target.Focus();
				};

				target.CommandDelegates.Add(EventKey.Cancel, () =>
				{
					button.Select();
					Focus();
				});

				button.BindNavigation(prev);
				prev = button;

				target.Hide();
			}
		}
		#endregion
	}
}