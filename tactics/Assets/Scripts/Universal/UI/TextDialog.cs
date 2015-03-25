using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

namespace Universal.UI
{
	public class TextDialog : Window
	{
		#region Inspector fields
		[SerializeField]
		private InputField textInput;
		#endregion

		public EventButton SubmitButton
		{
			get { return Buttons[0]; }
			set { Buttons[0] = value; }
		}

		public EventButton CancelButton
		{
			get { return Buttons[1]; }
			set { Buttons[1] = value; }
		}

		public override void Clear()
		{
			base.Clear();

			textInput.text = "";
		}

		public override void Activate()
		{
			base.Activate();

			EventSystem.current.SetSelectedGameObject(textInput.gameObject);
		}

		protected override void Awake()
		{
			base.Awake();

			textInput.onEndEdit.AddListener((string name) => { Data = name; });
		}
	}
}
