using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Universal.UI
{
	public delegate void EventButtonHandler(EventButton sender);

	[RequireComponent(typeof(Button))]
	public class EventButton : MonoBehaviour, ISelectHandler, IDeselectHandler, IGenericData
	{
		#region Events
		public event EventButtonHandler OnSelect;
		public event EventButtonHandler OnDeselect;
		public event EventButtonHandler OnClick;
		#endregion

		private Button inner;
		private Text text;
		private bool isSelected;

		public int ID { get; set; }
		public object Data { get; set; }

		public T GetData<T>()
		{
			return Data != null ? (T)Data : default(T);
		}

		public void SetText(string text)
		{
			if (this.text != null)
			{
				this.text.text = text;
			}
		}

		void ISelectHandler.OnSelect(BaseEventData eventData)
		{
			if (Debug.isDebugBuild) Debug.Log(name + " selected");
			isSelected = true;
			OnSelect.Raise(this);
		}

		void IDeselectHandler.OnDeselect(BaseEventData eventData)
		{
			if (Debug.isDebugBuild) Debug.Log(name + " deselected");
			isSelected = false;
			OnDeselect.Raise(this);
		}

		private void ClickHandler()
		{
			if (Debug.isDebugBuild) Debug.Log(name + " clicked");
			OnClick.Raise(this);
		}

		public void Select()
		{
			if (!isSelected)
			{
				inner.Select();
			}
		}

		public void Deselect()
		{
			if (isSelected)
			{
				EventSystem.current.SetSelectedGameObject(null);
			}
		}

		public void Click()
		{
			if (!isSelected) Select();
			ClickHandler();
		}

		public void BindNavigation(EventButton previousButton)
		{
			Navigation nav = inner.navigation;
			nav.mode = Navigation.Mode.Explicit;
			if (previousButton != null)
			{
				Button prev = previousButton.inner;
				nav.selectOnUp = prev;
				var prevNav = prev.navigation;
				prevNav.selectOnDown = inner;
				prev.navigation = prevNav;
			}
			inner.navigation = nav;
		}

		#region Unity events
		protected virtual void Awake()
		{
			inner = GetComponent<Button>();
			inner.onClick.AddListener(ClickHandler);

			text = GetComponent<Text>();
		}
		#endregion
	}
}