using UnityEngine;
using System;
using System.Collections.Generic;

namespace Universal.UI
{
	/// <summary>
	/// Displays labels for possible actions on the active panel. Buttons are displayed in the order of UIPanel.CapturedInputs.
	/// </summary>
	public class CommandPanel : UIPanel
	{
		#region Inspector fields
		[SerializeField]
		private EventButton commandButton;
		#endregion

		#region Fields
		private static CommandPanel instance;
		private List<EventButton> buttons = new List<EventButton>();
		private UIPanel target;
		#endregion

		private static bool isEnabled = true;

		#region Properties
		/// <summary>
		/// Gets or sets the enabled state of the command box
		/// </summary>
		public static bool IsEnabled
		{
			get
			{
				return isEnabled;
			}
			set
			{
				isEnabled = value;
				instance.gameObject.SetActive(isEnabled);
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Binds the singleton command box instance to a target UIPanel. If CommandPanel.IsEnabled is false, silently fails.
		/// </summary>
		/// <param name="target"></param>
		public static void Bind(UIPanel target)
		{
			if (isEnabled)
			{
				instance.target = target;
				instance.Refresh();
			}
		}

		/// <summary>
		/// Draws the command buttons.
		/// </summary>
		public override void Draw()
		{
			base.Draw();

			EventButton button, prev = null;
			RectTransform rectTransform;
			float offset = 0;
			for (int i = UIPanel.CapturedInputs.Length - 1; i >= 0; i--)
			{
				if (String.IsNullOrEmpty(target.CommandLabels[i])) continue;

				button = Instantiate(commandButton);

				// Set button's text
				button.Text = target.CommandLabels[i];

				// Append button to container
				rectTransform = button.GetComponent<RectTransform>();
				GetComponent<RectTransform>().Append(rectTransform, offset);
				offset -= rectTransform.rect.height;

				// Add button to list
				button.ID = buttons.Count;
				buttons.Add(button);

				// Show the button
				button.gameObject.SetActive(true);

				// Set button navigation
				button.Base.BindNavigation(prev != null ? prev.Base : null);
				prev = button;

				// Bind events
				int index = i;
				button.Base.onClick.AddListener(() => { target.Delegates.Raise(UIPanel.CapturedInputs[index]); });
			}
		}

		/// <summary>
		/// Clears the command buttons.
		/// </summary>
		public override void Clear()
		{
			base.Clear();

			// Clear existing buttons
			foreach (EventButton button in buttons)
			{
				Destroy(button.gameObject);
			}
			buttons.Clear();

			GetComponent<RectTransform>().CollapseDown();
		}

		/// <summary>
		/// Command box may not be activated. This should not be used.
		/// </summary>
		public override void Activate()
		{
			throw new InvalidOperationException("Command box may not be activated. Activate the target UIPanel instead.");
		}

		/// <summary>
		/// Command box may not be activated. This should not be used.
		/// </summary>
		public override void Deactivate()
		{
			Debug.LogWarning("Command box should not be activated. Deactivating will do nothing.");
		}
		#endregion

		#region Unity events
		protected override void Awake()
		{
			if (instance != null)
			{
				// Selfdestruct if there is already a CommandPanel present
				Destroy(gameObject);
			}
			else
			{
				instance = this;

				base.Awake();

				commandButton.gameObject.SetActive(false);
			}
		}
		#endregion
	}
}
