using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Universal.UI
{
	public class Selector : UIPanel
	{
		#region Inspector fields
		[SerializeField]
		private Text message;
		[SerializeField]
		private List<EventButton> buttons;
		[SerializeField]
		private EventButton cancelButton;
		#endregion

		#region Events
		public event UnityAction<int, object> Clicked;
		public event UnityAction<int> Selected;
		#endregion

		#region Fields
		private int selectedButtonIndex;
		#endregion

		#region Properties
		/// <summary>
		/// Gets the list of buttons in the window.
		/// </summary>
		public virtual IList<EventButton> Buttons
		{
			get { return buttons; }
		}

		/// <summary>
		/// Gets the last selected EventButton
		/// </summary>
		public virtual EventButton LastSelected { get; private set; }

		/// <summary>
		/// Gets the cancel button for the window.
		/// </summary>
		public virtual EventButton CancelButton
		{
			get { return cancelButton; }
		}

		/// <summary>
		/// Gets or sets the displayed text prompt.
		/// </summary>
		public virtual string Message
		{
			get { return message ? message.text : ""; }
			set { if (message) message.text = value; }
		}

		/// <summary>
		/// Gets or sets the window's data object.
		/// </summary>
		public virtual object Data { get; set; }
		#endregion

		#region Methods
		protected virtual void OnButtonClicked()
		{
			if (Clicked != null) Clicked(selectedButtonIndex, Data);
		}

		protected virtual void OnButtonSelected(Button sender, BaseEventData eventData)
		{
			if (!IsActivated)
			{
				DeactivateCurrent();
				Activate();
			}

			selectedButtonIndex = sender.GetComponent<EventButton>().ID;
			LastSelected = sender.GetComponent<EventButton>();

			if (Selected != null) Selected(selectedButtonIndex);
		}

		protected virtual void OnButtonCanceled()
		{
			Delegates.Raise(EventKey.Cancel);
		}

		/// <summary>
		/// Gets the window's data field as a specific type.
		/// </summary>
		/// <typeparam name="T">Type to cast the data to</typeparam>
		/// <returns>Data cast as T</returns>
		public T GetData<T>()
		{
			return Data != null ? (T)Data : default(T);
		}

		/// <summary>
		/// Bind select and click events to buttons.
		/// </summary>
		public virtual void BindButtonEvents()
		{
			// Bind callbacks to buttons
			for (int i = 0; i < buttons.Count; i++)
			{
				EventButton button = buttons[i];

				button.ID = i;

				button.Select += OnButtonSelected;
				button.Base.onClick.AddListener(OnButtonClicked);
			}
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
		/// Clears text and data.
		/// </summary>
		public override void Clear()
		{
			base.Clear();

			Data = null;
			Message = "";
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

		#region Unity events
		protected override void Awake()
		{
			base.Awake();

			if (cancelButton)
			{
				cancelButton.Base.onClick.AddListener(OnButtonCanceled);
			}
		}
		#endregion
	}
}