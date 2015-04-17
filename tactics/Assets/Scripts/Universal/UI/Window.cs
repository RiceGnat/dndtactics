using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Universal.UI
{
	/// <summary>
	/// Contains events for managing button events in a window.
	/// </summary>
	public class Window : UIPanel
	{
		#region Inspector fields
		[SerializeField]
		private Text text;
		[SerializeField]
		private List<EventButton> buttons;
		[SerializeField]
		private EventButton closeButton;
		#endregion

		#region Events
		public event UnityAction<int, object> Clicked;
		public event UnityAction<int> Selected;
		//public event UnityAction Canceled;
		#endregion

		private int selectedButton;

		/// <summary>
		/// Gets the list of buttons in the window.
		/// </summary>
		public virtual IList<EventButton> Buttons
		{
			get { return buttons; }
		}

		/// <summary>
		/// Gets the close button for the window.
		/// </summary>
		public virtual EventButton CloseButton
		{
			get { return closeButton; }
		}

		/// <summary>
		/// Gets or sets the message to be displayed on the window.
		/// </summary>
		public virtual string Message
		{
			get { return text ? text.text : ""; }
			set { if (text) text.text = value; }
		}

		/// <summary>
		/// Gets or sets the window's data object.
		/// </summary>
		public virtual object Data { get; set; }

		protected virtual void OnButtonClicked()
		{
			if (Clicked != null) Clicked(selectedButton, Data);
		}

		protected virtual void OnButtonSelected(Button sender, BaseEventData eventData)
		{
			if (!IsActivated)
			{
				DeactivateCurrent();
				Activate();
			}

			selectedButton = sender.GetComponent<EventButton>().ID;

			if (Selected != null) Selected(selectedButton);
		}

		protected virtual void OnButtonCanceled(Button sender, BaseEventData eventData)
		{
			OnCanceled();
		}

		public override void ClearEvents()
		{
			base.ClearEvents();

			// Clear window event handlers
			Selected = null;
			Clicked = null;

			ClearButtonEvents();
		}

		/// <summary>
		/// Removes all buttons.
		/// </summary>
		public void ClearButtons()
		{
			// Clear existing buttons
			foreach (EventButton button in Buttons)
			{
				Destroy(button.gameObject);
			}
			Buttons.Clear();
		}

		/// <summary>
		/// Clears all button events.
		/// </summary>
		public void ClearButtonEvents()
		{
			foreach (EventButton button in Buttons)
			{
				if (button != null)
				{
					button.Select -= OnButtonSelected;
					button.Base.onClick.RemoveAllListeners();
				}
			}
			if (closeButton)
			{
				closeButton.Submit -= OnButtonCanceled;
			}
		}

		/// <summary>
		/// Redraws the window while preserving data.
		/// </summary>
		public override void Refresh()
		{
			object data = Data;
			string msg = Message;
			Clear();
			Data = data;
			Message = msg;
			Draw();
		}

		#region UIPanel events
		/// <summary>
		/// Draws window UI elements and binds button events. Does not instantiate new buttons.
		/// </summary>
		public override void Draw()
		{
			base.Draw();

			// Bind callbacks to buttons
			for (int i = 0; i < buttons.Count; i++)
			{
				EventButton button = buttons[i];

				button.ID = i;

				button.Select += OnButtonSelected;
				button.Base.onClick.AddListener(OnButtonClicked);
				//button.Cancel += OnButtonCanceled;
			}
			if (closeButton)
			{
				closeButton.Submit += OnButtonCanceled;
				//closeButton.Cancel += OnButtonCanceled;
			}
		}

		/// <summary>
		/// Clears window UI elements and button events. Does not clear buttons.
		/// </summary>
		public override void Clear()
		{
			base.Clear();

			Data = null;

			if (text) text.text = "";

			ClearButtonEvents();
		}

		/// <summary>
		/// Selects the first button.
		/// </summary>
		public override void Activate()
		{
			base.Activate();

			if (!EventSystem.current.alreadySelecting)
				EventSystem.current.SetSelectedGameObject(buttons.Count > 0 ? buttons[0].gameObject : null);
		}
		#endregion
	}
}