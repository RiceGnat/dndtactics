using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using DnDEngine;
using Universal;
using Universal.UI;
using DnDTactics.Data;

namespace DnDTactics.UI
{
	/// <summary>
	/// Sets up UI for new unit creation.
	/// </summary>
	public class NewUnitMenu : UIPanel
	{
		#region Inspector fields
		[SerializeField]
		private Selector classPanel;
		[SerializeField]
		private EventButton classButton;
		[SerializeField]
		private Selector genderPanel;
		#endregion

		#region Properties
		private EventButton MaleButton { get { return genderPanel.Buttons[0]; } }
		private EventButton FemaleButton { get { return genderPanel.Buttons[1]; } }
		private DnDUnit.ClassType SelectedClass { get { return classPanel.LastSelected.GetData<DnDUnit.ClassType>(); } }
		private DnDUnit.GenderType SelectedGender { get { return genderPanel.LastSelected.GetData<DnDUnit.GenderType>(); } }
		#endregion

		#region Methods
		private void SetSelectedClass(int index)
		{
			// Will load unit artwork here
			MaleButton.GetComponentInChildren<Text>().text = SelectedClass + " placeholder\nMale";
			FemaleButton.GetComponentInChildren<Text>().text = SelectedClass + " placeholder\nFemale";
		}

		private void ActivateGender(int index, object data)
		{
			Deactivate();
			genderPanel.Activate();
		}

		private void SetSelectedGender(int index)
		{

		}	

		private void OnGenderCanceled()
		{
			genderPanel.Deactivate();
			var last = classPanel.LastSelected.gameObject;
			Activate();
			EventSystem.current.SetSelectedGameObject(last);
		}

		private void ShowConfirm(int index, object data)
		{
			genderPanel.Deactivate();
			ModalDialog.Confirm(string.Format("Create a new {0} {1}?", SelectedGender.ToString().ToLower(), SelectedClass.ToString().ToLower()), ShowNameDialog, OnConfirmCanceled);
		}

		private void ShowNameDialog()
		{
			ModalDialog.TextDialog(string.Format("Enter a name for the new {0} {1}:", SelectedGender.ToString().ToLower(), SelectedClass.ToString().ToLower()), OnNameSubmitted, OnConfirmCanceled);
		}

		private void OnConfirmCanceled()
		{
			var last = genderPanel.LastSelected.gameObject;
			genderPanel.Activate();
			EventSystem.current.SetSelectedGameObject(last);
		}

		private void OnNameSubmitted(int index, object data)
		{
			DnDUnit unit = new DnDUnit(data.ToString(), SelectedClass, new CoreStats(1, 10, 10, 10, 10, 10, 10, 5, 10), DnDUnit.BodyType.Humanoid, SelectedGender);
			unit.Evaluate();
			if (Debug.isDebugBuild)
			{
				Debug.Log(string.Format("Creating new {0} {1} named {2}", SelectedGender.ToString().ToLower(), SelectedClass.ToString().ToLower(), data));
				Debug.Log(DnDUnit.Describe(unit).Full);
			}
			DataManager.Party.Add(unit);

			Activate();
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

			EventButton button, prev = null;
			RectTransform rectTransform;
			float offset = 0;
			foreach (DnDUnit.ClassType @class in classList)
			{
				button = Instantiate(classButton);

				// Set button's class value
				button.Data = @class;
				button.Text = DataManager.GetClassDisplayName(@class);

				// Append button to container
				rectTransform = button.GetComponent<RectTransform>();
				classPanel.GetComponent<RectTransform>().Append(rectTransform, offset);
				offset += rectTransform.rect.height;

				// Add button to list
				classPanel.Buttons.Add(button);

				// Show the button
				button.gameObject.SetActive(true);

				// Set button navigation
				button.Base.BindNavigation(prev != null ? prev.Base : null);
				prev = button;
			}

			// Bind button events
			classPanel.BindButtonEvents();
		}

		/// <summary>
		/// Clears class list.
		/// </summary>
		public override void Clear()
		{
			base.Clear();

			// Clear window data
			classPanel.Clear();
			genderPanel.Clear();

			// Clear existing buttons
			classPanel.ClearButtons();
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
		#endregion

		#region Unity events
		protected override void Awake()
		{
			base.Awake();

			// Assign gender data to buttons
			MaleButton.Data = DnDUnit.GenderType.Male;
			FemaleButton.Data = DnDUnit.GenderType.Female;

			// Bind control events
			classPanel.Selected += SetSelectedClass;
			classPanel.Clicked += ActivateGender;

			genderPanel.Selected += SetSelectedGender;
			genderPanel.Clicked += ShowConfirm;
			genderPanel.BindButtonEvents();

			classPanel.Delegates.Add(EventKey.Cancel, OnCanceled);
			genderPanel.Delegates.Add(EventKey.Cancel, OnGenderCanceled);
		}

		protected override void Start()
		{
			base.Start();
			classButton.gameObject.SetActive(false);
		}
		#endregion
	}
}