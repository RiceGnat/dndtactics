using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

namespace Universal.UI
{
	/// <summary>
	/// Manages drawing of UI elements.
	/// </summary>
	public class UIPanel : MonoBehaviour
	{
		#region Events
		/// <summary>
		/// Called when the user cancels out of the UIPanel.
		/// </summary>
		public event UnityAction Canceled;

		/// <summary>
		/// Called when the user submits the UIPanel.
		/// </summary>
		public event UnityAction Submitted;

		public event UnityAction ButtonX;
		public event UnityAction ButtonY;
		public event UnityAction BumperL;
		public event UnityAction BumperR;
		#endregion

		protected static UIPanel activeWindow;

		protected static void DeactivateCurrent()
		{
			if (activeWindow) activeWindow.Deactivate();
		}

		/// <summary>
		/// Gets the activated state of this UIPanel.
		/// </summary>
		public bool IsActivated { get; private set; }

		protected virtual void ClearEvents()
		{
			Canceled = null;
			Submitted = null;
			ButtonX = null;
			ButtonY = null;
			BumperL = null;
			BumperR = null;
		}

		/// <summary>
		/// When overridden in a derived class, draws the UI elements for this menu item.
		/// </summary>
		public virtual void Draw()
		{
			//if (Debug.isDebugBuild) Debug.Log("Drawing " + name);
		}

		/// <summary>
		/// When overridden in a derived class, clears the UI elements for this menu item.
		/// </summary>
		public virtual void Clear()
		{
			//if (Debug.isDebugBuild) Debug.Log("Clearing " + name);
		}

		/// <summary>
		/// Calls Clear() then Draw(). Can be overridden for additional functionality.
		/// </summary>
		public virtual void Refresh()
		{
			Clear();
			Draw();
		}

		/// <summary>
		/// Shows the GameObject associated with this Component. Automatically calls Draw(). Can be overridden for additional functionality.
		/// </summary>
		public virtual void Show()
		{
			//if (Debug.isDebugBuild) Debug.Log("Showing " + name);
			gameObject.SetActive(true);
			Draw();
		}

		/// <summary>
		/// Hides the GameObject associated with this Component. Automatically calls Clear(). Can be overridden for additional functionality.
		/// </summary>
		public virtual void Hide()
		{
			//if (Debug.isDebugBuild) Debug.Log("Hiding " + name);
			gameObject.SetActive(false);
			Clear();
		}

		/// <summary>
		/// Sets this UIPanel as activated to detect input events. Can be overridden for additional functionality.
		/// </summary>
		public virtual void Activate()
		{
			if (Debug.isDebugBuild) Debug.Log("Activating " + name);
			IsActivated = true;
			activeWindow = this;
		}

		/// <summary>
		/// Sets this UIPanel as deactivated to detect input events. Can be overridden for additional functionality.
		/// </summary>
		public virtual void Deactivate()
		{
			if (Debug.isDebugBuild) Debug.Log("Deactivating " + name);
			IsActivated = false;
			if (this.Equals(activeWindow)) activeWindow = null;
		}

		protected virtual void OnSubmitted()
		{
			if (Submitted != null) Submitted();
		}

		protected virtual void OnCanceled()
		{
			if (Canceled != null) Canceled();
		}

		protected virtual void OnButtonX()
		{
			if (ButtonX != null) ButtonX();
		}

		protected virtual void OnButtonY()
		{
			if (ButtonY != null) ButtonY();
		}

		protected virtual void OnBumperL()
		{
			if (BumperL != null) BumperL();
		}

		protected virtual void OnBumperR()
		{
			if (BumperR != null) BumperR();
		}

		private void MapEvents(string inputName)
		{

		}

		#region Unity events
		protected virtual void Awake()
		{

		}

		protected virtual void Start()
		{

		}

		protected virtual void Update()
		{
			if (IsActivated)
			{
				if (Input.GetButtonDown("Submit"))
				{
					Debug.Log("Submit event on " + name);
					OnSubmitted();
					Input.ResetInputAxes();
				}
				if (Input.GetButtonDown("Cancel"))
				{
					Debug.Log("Cancel event on " + name);
					OnCanceled();
					Input.ResetInputAxes();
				}
				if (Input.GetButtonDown("Button X"))
				{
					OnButtonX();
					Input.ResetInputAxes();
				}
				if (Input.GetButtonDown("Button Y"))
				{
					OnButtonY();
					Input.ResetInputAxes();
				}
				if (Input.GetButtonDown("Bumper L"))
				{
					OnBumperL();
					Input.ResetInputAxes();
				}
				if (Input.GetButtonDown("Bumper R"))
				{
					OnBumperR();
					Input.ResetInputAxes();
				}
			}
		}
		#endregion
	}
}