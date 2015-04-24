using UnityEngine;
using System;
using System.Collections.Generic;

namespace Universal.UI
{
	public class CommandPanel : UIPanel
	{
		#region Inspector fields
		[SerializeField]
		private EventButton commandButton;
		#endregion

		#region Fields
		private List<EventButton> buttons = new List<EventButton>();
		private UIPanel target;
		#endregion

		#region Properties
		public static bool IsEnabled { get; set; }
		#endregion

		#region Methods
		/// <summary>
		/// Binds the command box to a target UIPanel.
		/// </summary>
		/// <param name="target"></param>
		public void Bind(UIPanel target)
		{
			this.target = target;
			Refresh();
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
			for (int i = 0; i < UIPanel.CapturedInputs.Length; i++)
			{
				if (String.IsNullOrEmpty(target.CommandLabels[i])) continue;

				button = Instantiate(commandButton);

				// Set button's text
				button.Text = target.CommandLabels[i];

				// Append button to container
				rectTransform = button.GetComponent<RectTransform>();
				GetComponent<RectTransform>().Append(rectTransform, offset);
				//offset += rectTransform.rect.height;

				// Add button to list
				buttons.Add(button);

				// Show the button
				button.gameObject.SetActive(true);

				// Set button navigation
				button.Base.BindNavigation(prev != null ? prev.Base : null);
				prev = button;

				// Bind events
				button.Base.onClick.AddListener(() => { target.Delegates.Raise(UIPanel.CapturedInputs[i]); });
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

			GetComponent<RectTransform>().Collapse();
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
			base.Awake();

			commandButton.gameObject.SetActive(false);
			GetComponent<RectTransform>().Collapse();
		}
		#endregion
	}
}
