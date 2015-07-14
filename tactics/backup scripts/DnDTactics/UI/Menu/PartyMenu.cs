using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using DnDEngine;
using Universal;
using Universal.UI;
using DnDTactics.Data;

namespace DnDTactics.UI
{
	/// <summary>
	/// Sets up UI for party management.
	/// </summary>
	public class PartyMenu : Selector
	{
		#region Inspector fields
		[SerializeField]
		private ScrollRect partyScrollRect;
		[SerializeField]
		private UnitCard unitCard;
		#endregion

		private UnitDetails DetailsPage { get { return UIManager.UnitDetailsPage; } }
		private UnitCard LastUnit { get { return LastSelected.GetComponent<UnitCard>(); } }
		private RectTransform PartyContainer { get { return partyScrollRect.content; } }

		private void BindDetails(int index, object data)
		{
			Deactivate();
			DetailsPage.Bind(LastUnit.Unit);

			DetailsPage.Delegates.Add(EventKey.Cancel, CloseDetails);
			DetailsPage.Delegates.Add(EventKey.BumperL, BindPrev);
			DetailsPage.Delegates.Add(EventKey.BumperR, BindNext);

			// UnitDetails page automatically calls Activate()
			DetailsPage.Show();
		}

		private void BindNext()
		{
			if (LastSelected.ID + 1 < Buttons.Count)
			{
				LastSelected = Buttons[LastSelected.ID + 1];
				DetailsPage.Bind(LastUnit.Unit);
				DetailsPage.Refresh();
			}
		}

		private void BindPrev()
		{
			if (LastSelected.ID > 0)
			{
				LastSelected = Buttons[LastSelected.ID - 1];
				DetailsPage.Bind(LastUnit.Unit);
				DetailsPage.Refresh();
			}
		}

		private void CloseDetails()
		{
			// Clean up UnitDetails page
			DetailsPage.Delegates.Remove(EventKey.Cancel, CloseDetails);
			DetailsPage.Delegates.Remove(EventKey.BumperL, BindPrev);
			DetailsPage.Delegates.Remove(EventKey.BumperR, BindNext);
			DetailsPage.Hide();

			var last = LastUnit.gameObject;
			Activate();
			EventSystem.current.SetSelectedGameObject(last);
		}

		/// <summary>
		/// Draws and binds events for list of unit cards.
		/// </summary>
		public override void Draw()
		{
			base.Draw();

			// Create unit cards for party
			UnitCard card;
			EventButton button, prev = null;
			RectTransform rectTransform;
			float offset = 0;
			foreach (var unit in DataManager.Party)
			{
				card = Instantiate<UnitCard>(unitCard);

				// Append card to container
				rectTransform = card.GetComponent<RectTransform>();
				PartyContainer.Append(rectTransform, offset);
				offset += rectTransform.rect.height + 20;

				// Bind card to unit
				card.Bind(unit);
				card.name = unit.ToString();

				// Show the card
				card.Show();

				// Add card to list
				button = card.GetComponent<EventButton>();
				Buttons.Add(button);

				// Set button navigation
				button.Base.BindNavigation(prev != null ? prev.Base : null);
				prev = button;
			}

			// Add space for gutter in scroll region
			PartyContainer.offsetMin += new Vector2(0, unitCard.GetComponent<RectTransform>().offsetMax.y);

			BindButtonEvents();
		}

		/// <summary>
		/// Clears list of unit cards.
		/// </summary>
		public override void Clear()
		{
			base.Clear();

			// Clear existing unit cards
			ClearButtons();

			// Reset scroll height
			PartyContainer.offsetMin = Vector2.Scale(PartyContainer.offsetMin, new Vector2(1, 0));
		}

		#region Unity events
		protected override void Awake()
		{
			base.Awake();

			// Bind clicked handler to open details page
			Clicked += BindDetails;
		}

		protected override void Start()
		{
			base.Start();

			unitCard.gameObject.SetActive(false);
		}
		#endregion
	}
}