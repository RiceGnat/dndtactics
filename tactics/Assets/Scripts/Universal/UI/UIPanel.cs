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
		#endregion

		/// <summary>
		/// Gets the activated state of this UIPanel.
		/// </summary>
		public bool IsActivated { get; private set; }

		protected virtual void ClearEvents()
		{
			Canceled = null;
			Submitted = null;
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
		}

		/// <summary>
		/// Sets this UIPanel as deactivated to detect input events. Can be overridden for additional functionality.
		/// </summary>
		public virtual void Deactivate()
		{
			if (Debug.isDebugBuild) Debug.Log("Deactivating " + name);
			IsActivated = false;
		}

		protected void OnSubmitted()
		{
			if (Submitted != null) Submitted();
		}

		protected void OnCanceled()
		{
			if (Canceled != null) Canceled();
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
				if (Input.GetButtonDown("Cancel"))
				{
					Debug.Log("Cancel event on " + name);
					OnCanceled();
					Input.ResetInputAxes();
				}
				if (Input.GetButtonDown("Submit"))
				{
					Debug.Log("Submit event on " + name);
					OnSubmitted();
					Input.ResetInputAxes();
				}
			}
		}
		#endregion
	}
}