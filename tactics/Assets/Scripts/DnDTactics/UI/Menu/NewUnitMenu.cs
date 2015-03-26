using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using DnDEngine;
using Universal.UI;

namespace DnDTactics.UI
{
	/// <summary>
	/// Sets up UI for new unit creation.
	/// </summary>
	public class NewUnitMenu : UIPanel
	{
		#region Inspector fields
		[SerializeField]
		private Window classPanel;
		[SerializeField]
		private ClassButton classButton;
		[SerializeField]
		private Window genderPanel;
		[SerializeField]
		private Window nameDialog;
		#endregion

		private ClassButton maleButton { get { return genderPanel.Buttons[0] as ClassButton; } }
		private ClassButton femaleButton { get { return genderPanel.Buttons[1] as ClassButton; } }
		private List<ClassButton> buttonList;
		private ClassButton lastClass;
		private ClassButton lastGender;
		private DnDUnit.ClassType selectedClass { get { return lastClass.Class; } }
		private DnDUnit.GenderType selectedGender { get { return lastGender.Gender; } }

		private void SetSelectedClass(int index)
		{
			lastClass = classPanel.Buttons[index] as ClassButton;

			// Will load unit artwork here
			maleButton.GetComponentInChildren<Text>().text = selectedClass + " placeholder\nMale";
			femaleButton.GetComponentInChildren<Text>().text = selectedClass + " placeholder\nFemale";
		}

		private void ActivateGender(int index, object data)
		{
			Deactivate();
			genderPanel.Activate();
		}

		private void SetSelectedGender(int index)
		{
			lastGender = genderPanel.Buttons[index] as ClassButton;
		}

		private void OnGenderCanceled()
		{
			genderPanel.Deactivate();
			var last = lastClass.gameObject;
			Activate();
			EventSystem.current.SetSelectedGameObject(last);
		}

		private void ShowConfirm(int index, object data)
		{
			genderPanel.Deactivate();
			ModalDialog.Confirm(string.Format("Create a new {0} {1}?", selectedGender.ToString().ToLower(), selectedClass.ToString().ToLower()), ShowNameDialog, OnConfirmCanceled);
		}

		private void ShowNameDialog()
		{
			ModalDialog.TextDialog(string.Format("Enter a name for the new {0} {1}:", selectedGender.ToString().ToLower(), selectedClass.ToString().ToLower()), OnNameSubmitted, OnConfirmCanceled);
		}

		private void OnConfirmCanceled()
		{
			var last = lastGender.gameObject;
			genderPanel.Activate();
			EventSystem.current.SetSelectedGameObject(last);
		}

		private void OnNameSubmitted(int index, object data)
		{
			if (Debug.isDebugBuild) Debug.Log(string.Format("Creating new {0} {1} named {2}", selectedGender.ToString().ToLower(), selectedClass.ToString().ToLower(), data));
			DnDUnit unit = new DnDUnit(data.ToString(), selectedClass, new CoreStats(1, 10, 10, 10, 10, 10, 10, 5, 10), DnDUnit.BodyType.Humanoid, selectedGender);
			unit.Evaluate();
			if (Debug.isDebugBuild)
			{
				Debug.Log(string.Format("Creating new {0} {1} named {2}", selectedGender.ToString().ToLower(), selectedClass.ToString().ToLower(), data));
				Debug.Log(DnDUnit.Describe(unit).Full);
			}
			DataManager.Party.Add(unit);
		}

		/// <summary>
		/// Draws class list and binds events.
		/// </summary>
		public override void Draw()
		{
			base.Draw();

			var classList = from DnDUnit.ClassType c in Enum.GetValues(typeof(DnDUnit.ClassType))
							where !DataManager.IsClassUnique(c)
							select c;

			ClassButton button, prev = null;
			RectTransform rectTransform;
			float offset;
			foreach (DnDUnit.ClassType @class in classList)
			{
				button = Instantiate(classButton);

				// Set parent to container
				button.transform.SetParent(classPanel.transform, false);

				// Set button's class value
				button.Class = @class;
				button.name = @class.ToString();

				// Adjust offset
				rectTransform = button.GetComponent<RectTransform>();
				offset = rectTransform.rect.height * classPanel.Buttons.Count;
				rectTransform.offsetMax -= new Vector2(0, offset);
				rectTransform.offsetMin -= new Vector2(0, offset);

				// Add button to list
				classPanel.Buttons.Add(button);

				// Show the button
				button.gameObject.SetActive(true);

				// Set button navigation
				var nav = button.Selectable.navigation;
				nav.mode = Navigation.Mode.Explicit;
				if (prev != null) {
					nav.selectOnUp = prev.Selectable;
					var prevNav = prev.Selectable.navigation;
					prevNav.selectOnDown = button.Selectable;
					prev.Selectable.navigation = prevNav;
				}
				button.Selectable.navigation = nav;
				prev = button;
			}

			// Bind events
			classPanel.Selected += SetSelectedClass;
			classPanel.Clicked += ActivateGender;
			classPanel.Draw();

			genderPanel.Selected += SetSelectedGender;
			genderPanel.Clicked += ShowConfirm;
			genderPanel.Draw();

			// Automatically show first class in list
			if (classPanel.Buttons.Count > 0)
				SetSelectedClass(0);
		}

		/// <summary>
		/// Clears class list.
		/// </summary>
		public override void Clear()
		{
			base.Clear();

			// Clear window events
			classPanel.Clear();
			genderPanel.Clear();

			// Clear existing buttons
			classPanel.ClearButtons();

			lastClass = null;
			lastGender = null;
		}

		/// <summary>
		/// Selects the first class.
		/// </summary>
		public override void Activate()
		{
			classPanel.Activate();
		}

		/// <summary>
		/// Cancels out of the unit creation menu.
		/// </summary>
		public override void Deactivate()
		{
			classPanel.Deactivate();
		}

		#region Unity events
		protected override void Awake()
		{
			base.Awake();
			classPanel.Canceled += OnCanceled;
			genderPanel.Canceled += OnGenderCanceled;
		}

		protected override void Start()
		{
			base.Start();
			classButton.gameObject.SetActive(false);
		}
		#endregion
	}
}