using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Universal.UI
{
	public delegate void DataBindHandler(UIElement element, int index);

	public class Repeater : UIElement
	{
		#region Inspector fields
		[SerializeField]
		private UIElement template;
		#endregion

		#region Fields
		private UIElement[] items;
		private EventButton lastSelected;
		#endregion

		#region Properties
		public int Count { get; set; }
		public IList<UIElement> Items
		{
			get { return items; }
		}
		public IEnumerable DataSource { get; set; }
		#endregion

		#region Events
		public event DataBindHandler OnDataBind;
		#endregion

		#region Methods
		public override void Draw()
		{
			items = new UIElement[Count];

			EventButton prev = null;

			IEnumerator enumerator = DataSource != null ? DataSource.GetEnumerator() : null;

			// Clone template N times;
			for (int i = 0; i < Count || (Count == 0 && DataSource != null); i++)
			{
				UIElement clone = Instantiate(template);
				clone.SetParent(this);
				items[i] = clone;

				clone.Show();

				if (enumerator != null)
				{
					clone.Data = enumerator.Current;
					if (!enumerator.MoveNext()) break;
				}

				Debug.Log(enumerator);

				if (OnDataBind != null) OnDataBind(clone, i);

				// Not sure if this should be here... consider moving to new class?
				EventButton button = clone.GetComponent<EventButton>();

				if (button != null)
				{
					button.OnSelect += SetSelected;

					if (lastSelected == null) lastSelected = button;

					button.BindNavigation(prev);
					prev = button;
				}
			}

			base.Draw();
		}

		public override void Clear()
		{
			base.Clear();

			// Destroy clones
			foreach (UIElement item in items)
			{
				item.Hide();
				Destroy(item.gameObject);
			}

			items = null;
			lastSelected = null;
		}

		public override void Focus()
		{
			base.Focus();

			lastSelected.Select();
		}

		private void SetSelected(EventButton sender)
		{
			lastSelected = sender;
		}
		#endregion

		#region Unity events
		protected override void Awake()
		{
			base.Awake();

			if (template.IsVisible) template.Hide();
		}
		#endregion
	}
}