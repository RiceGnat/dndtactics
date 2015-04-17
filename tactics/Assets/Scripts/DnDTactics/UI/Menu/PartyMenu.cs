using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using DnDEngine;
using Universal.UI;

namespace DnDTactics.UI
{
	/// <summary>
	/// Sets up UI for party management.
	/// </summary>
	public class PartyMenu : UIPanel
	{
		#region Inspector fields
		[SerializeField]
		private Window partyPanel;
		[SerializeField]
		private UnitCard unitCard;
		[SerializeField]
		private UnitDetails detailsPage;
		#endregion

		private int lastIndex;

		private UnitCard lastSelected { get { return partyPanel.Buttons[lastIndex].GetComponent<UnitCard>(); } }
		private RectTransform partyContainer { get { return partyPanel.GetComponent<ScrollRect>().content; } }

		private void BindDetails(int index, object data)
		{
			lastIndex = index;
			Deactivate();
			detailsPage.Bind(lastSelected.Unit);
			detailsPage.Canceled += CloseDetails;
			detailsPage.BumperL += BindPrev;
			detailsPage.BumperR += BindNext;

			// UnitDetails page automatically calls Activate()
			detailsPage.Show();
		}

		private void BindNext()
		{
			if (lastIndex + 1 < partyPanel.Buttons.Count)
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
			detailsPage.Canceled -= CloseDetails;
			detailsPage.BumperL -= BindPrev;
			detailsPage.BumperR -= BindNext;
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

				// Set parent to container
				card.transform.SetParent(partyContainer.transform, false);

				// Adjust card offset and container height
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
				partyPanel.Buttons.Add(button);

				// Set button navigation
				button.Base.BindNavigation(prev != null ? prev.Base : null);
				prev = button;
			}

			// Add space for gutter in scroll region
			partyContainer.offsetMin += new Vector2(0, unitCard.GetComponent<RectTransform>().offsetMax.y);

			// Draw panel (binds button events)
			partyPanel.Draw();
		}

		/// <summary>
		/// Clears list of unit cards.
		/// </summary>
		public override void Clear()
		{
			base.Clear();

			// Clear window events
			partyPanel.Clear();

			// Clear existing unit cards
			partyPanel.ClearButtons();

			// Reset scroll height
			partyContainer.offsetMin = Vector2.Scale(partyContainer.offsetMin, new Vector2(1, 0));
		}

		/// <summary>
		/// Selects the first unit card.
		/// </summary>
		public override void Activate()
		{
			partyPanel.Activate();
		}

		/// <summary>
		/// Cancels out of the party management menu.
		/// </summary>
		public override void Deactivate()
		{
			partyPanel.Deactivate();
		}

		#region Unity events
		protected override void Awake()
		{
			base.Awake();

			// Bind details page handler
			partyPanel.Clicked += BindDetails;
			partyPanel.Canceled += OnCanceled;
		}

		protected override void Start()
		{
			base.Start();
			unitCard.gameObject.SetActive(false);
		}
		#endregion
	}
}