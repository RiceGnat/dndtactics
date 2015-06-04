using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

namespace Universal.UI
{
	/// <summary>
	/// Manages display of modal dialog windows.
	/// </summary>
	public class ModalDialog : UIPanel
	{
		#region Inspector fields
		[SerializeField]
		private CanvasRenderer overlay;
		[SerializeField]
		private AlertDialog alertWindow;
		[SerializeField]
		private ConfirmDialog confirmWindow;
		[SerializeField]
		private TextDialog textWindow;
		#endregion

		private Queue<UnityAction> callQueue = new Queue<UnityAction>();

		private static Selector window;

		private static ModalDialog instance;

		private static ModalDialog Instance
		{
			get
			{
				if (!instance)
				{
					instance = FindObjectOfType<ModalDialog>();
					if (!instance) throw new NullReferenceException("No instance of ModalDialog found in scene.");
				}

				return instance;
			}
		}

		/// <summary>
		/// Gets a value indication if a window is already open.
		/// </summary>
		public static bool IsOpen
		{
			get { return window != null; }
		}

		private static void SetCallback(Button button, UnityAction callback)
		{
			if (callback != null) button.onClick.AddListener(callback);
			button.onClick.AddListener(Instance.Hide);
		}

		/// <summary>
		/// Opens an alert window. Calling this function while a window is already open will add it to a queue that will be checked whenever the current window is closed.
		/// </summary>
		/// <param name="message">The message to be displayed.</param>
		/// <param name="callback">The callback function to be run once the window is closed.</param>
		public static void Alert(string message, UnityAction callback)
		{
			if (IsOpen)
			{
				Instance.callQueue.Enqueue(() => { Alert(message, callback); });
				return;
			}

			window = Instantiate(Instance.alertWindow);
			window.Message = message;
			Instance.Show();

			SetCallback((window as AlertDialog).OkButton.Base, callback);

			window.Delegates.Add(EventKey.Cancel, callback);
			window.Delegates.Add(EventKey.Cancel, Instance.Hide);
		}

		/// <summary>
		/// Opens a confirmation dialog. Calling this function while a window is already open will add it to a queue that will be checked whenever the current window is closed.
		/// </summary>
		/// <param name="message">The message to be displayed.</param>
		/// <param name="yesCallback">The callback function to be run if the user selects Yes.</param>
		/// <param name="noCallback">The callback function to be run if the user selects No or cancels.</param>
		public static void Confirm(string message, UnityAction yesCallback, UnityAction noCallback)
		{
			if (IsOpen)
			{
				Instance.callQueue.Enqueue(() => { Confirm(message, yesCallback, noCallback); });
				return;
			}

			window = Instantiate(Instance.confirmWindow);
			window.Message = message;
			Instance.Show();

			SetCallback((window as ConfirmDialog).YesButton.Base, yesCallback);
			SetCallback((window as ConfirmDialog).NoButton.Base, noCallback);

			window.Delegates.Add(EventKey.Cancel, noCallback);
			window.Delegates.Add(EventKey.Cancel, Instance.Hide);
		}

		/// <summary>
		/// Opens a text input dialog. Calling this function while a window is already open will add it to a queue that will be checked whenever the current window is closed.
		/// </summary>
		/// <param name="message">The message to be displayed.</param>
		/// <param name="submitCallback">The callback function to be run once the user submits their input.</param>
		/// <param name="cancelCallback">The callback function to be run if the user cancels.</param>
		public static void TextDialog(string message, UnityAction<int, object> submitCallback, UnityAction cancelCallback)
		{
			if (IsOpen)
			{
				Instance.callQueue.Enqueue(() => { TextDialog(message, submitCallback, cancelCallback); });
				return;
			}

			window = Instantiate(Instance.textWindow);
			window.Message = message;
			Instance.Show();

			window.Clicked += (int index, object data) =>
			{
				submitCallback(index, data);
				Instance.Hide();
			};

			window.Delegates.Add(EventKey.Cancel, cancelCallback);
			window.Delegates.Add(EventKey.Cancel, Instance.Hide);
		}

		//public static void Dialog(Window dialog)
		//{
		//	if (IsOpen) throw alreadyOpenEx;

		//	window = dialog;

		//	Instance.Show();

		//	window.Delegates.Add(EventKey.Cancel, Instance.Hide;
		//}

		#region UIPanel methods
		/// <summary>
		/// Show the modal dialog with the overlay to block input.
		/// </summary>
		public override void Show()
		{
			overlay.gameObject.SetActive(true);
			window.transform.SetParent(overlay.transform, false);
			window.transform.SetAsLastSibling();
			window.Show();
			window.BindButtonEvents();
			window.Activate();
		}

		/// <summary>
		/// Hide the modal dialog and overlay.
		/// </summary>
		public override void Hide()
		{
			if (window)
			{
				window.Deactivate();
				Destroy(window.gameObject);
			}
			overlay.gameObject.SetActive(false);
			window = null;

			if (callQueue.Count > 0) callQueue.Dequeue()();
		}
		#endregion

		#region Unity events
		protected override void Start()
		{
			Hide();
			if (alertWindow.gameObject.activeSelf) alertWindow.Hide();
			if (confirmWindow.gameObject.activeSelf) confirmWindow.Hide();
			if (textWindow.gameObject.activeSelf) textWindow.Hide();
			transform.SetAsLastSibling();
		}
		#endregion
	}
}