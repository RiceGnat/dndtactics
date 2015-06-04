using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace Universal.UI
{
	/// <summary>
	/// Manages drawing of UI elements.
	/// </summary>
	[RequireComponent(typeof(RectTransform))]
	public class UIPanel : MonoBehaviour
	{
		public void OnCanceled() {  }
		public virtual void ClearEvents() { }
		#region Inspector fields
		[SerializeField]
		private CommandLabelSet commandLabels;
		#endregion

		#region Fields
		private static EventKey[] capturedInputs = new EventKey[] { EventKey.Submit, EventKey.Cancel, EventKey.ButtonX, EventKey.ButtonY, EventKey.BumperL, EventKey.BumperR };
		private UnityActionSet delegates = new UnityActionSet();

		protected static UIPanel activeWindow;
		#endregion

		#region Properties
		/// <summary>
		/// Gets the input event keys that UIPanel objects will check.
		/// </summary>
		public static EventKey[] CapturedInputs { get { return capturedInputs; } }

		/// <summary>
		/// Gets the activated state of this UIPanel.
		/// </summary>
		public bool IsActivated { get; private set; }
		
		/// <summary>
		/// Gets the delegate set for this UIPanel.
		/// </summary>
		public UnityActionSet Delegates { get { return delegates; } }

		/// <summary>
		/// Gets the set of command labels.
		/// </summary>
		public CommandLabelSet CommandLabels { get { return commandLabels; } }
		#endregion

		#region Methods
		protected static void DeactivateCurrent()
		{
			if (activeWindow) activeWindow.Deactivate();
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
		/// Calls Clear() then Draw(). Can be overridden for additional/alternate functionality.
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

			// Only one panel should be active and capturing inputs
			// This is kind of a failsafe, should Deactivate() properly when possible
			DeactivateCurrent();

			IsActivated = true;
			activeWindow = this;

			if (CommandPanel.IsEnabled)
			{
				UIManager.CommandBox.Bind(this);
			}
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
		#endregion

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
				// Check captured inputs
				foreach (EventKey key in capturedInputs)
				{
					if (Input.GetButtonDown(key.Name))
					{
						if (Debug.isDebugBuild) Debug.Log(String.Format("{0} event on {1}", key.Name, name));
						Delegates.Raise(key);

						// Reset inputs for the rest of this frame to prevent cascading 
						Input.ResetInputAxes();
					}
				}
			}
		}
		#endregion
	}
}