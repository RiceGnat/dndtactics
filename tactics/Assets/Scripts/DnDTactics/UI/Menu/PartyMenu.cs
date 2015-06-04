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
		[SerializeField]
		private UnitDetails detailsPage;
		#endregion

		private int lastIndex;

		private UnitCard lastSelected { get { return Buttons[lastIndex].GetComponent<UnitCard>(); } }
		private RectTransform partyContainer { get { return partyScrollRect.content; } }

		private void BindDetails(int index, object data)
		{
			lastIndex = index;
			Deactivate();
			detailsPage.Bind(lastSelected.Unit);

			detailsPage.Delegates.Add(EventKey.Cancel, CloseDetails);
			detailsPage.Delegates.Add(EventKey.BumperL, BindPrev);
			detailsPage.Delegates.Add(EventKey.BumperR, BindNext);
			//detailsPage.Canceled += CloseDetails;
			//detailsPage.BumperL += BindPrev;
			//detailsPage.BumperR += BindNext;

			// UnitDetails page automatically calls Activate()
			detailsPage.Show();
		}

		private void BindNext()
		{
			if (lastIndex + 1 < Buttons.Count)
			{
				lastIndex++;
				detailsPage.Bind(lastSelected.Unit);
				detailsPage.Refresh();
			}
		}

		private void BindPrev()
		{
			if (lastIndex > 0)
			{
				lastIndex--;
				detailsPage.Bind(lastSelected.Unit);
				detailsPage.Refresh();
			}
		}

		private void CloseDetails()
		{
			// Clean up UnitDetails page
			detailsPage.Delegates.Remove(EventKey.Cancel, CloseDetails);
			detailsPage.Delegates.Remove(EventKey.BumperL, BindPrev);
			detailsPage.Delegates.Remove(EventKey.BumperR, BindNext);
			detailsPage.Hide();

			Activate();
			EventSystem.current.SetSelectedGameObject(lastSelected.gameObject);
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
				partyContainer.Append(rectTransform, offset);
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
			partyContainer.offsetMin += new Vector2(0, unitCard.GetComponent<RectTransform>().offsetMax.y);

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
			partyContainer.offsetMin = Vector2.Scale(partyContainer.offsetMin, new Vector2(1, 0));
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