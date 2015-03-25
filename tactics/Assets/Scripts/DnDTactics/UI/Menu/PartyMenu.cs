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
		private CanvasRenderer scrollContent;
		[SerializeField]
		private UnitCard unitCard;
		[SerializeField]
		private UnitDetails detailsPage;
		#endregion

		private UnitCard lastSelected;

		private void BindDetails(int index, object data)
		{
			lastSelected = partyPanel.Buttons[index].GetComponent<UnitCard>();
			Deactivate();
			detailsPage.Bind(lastSelected.Unit);
			detailsPage.Canceled += CloseDetails;

			// UnitDetails page automatically calls Activate()
			detailsPage.Show();
		}

		private void CloseDetails()
		{
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

			// Add space for gutter in scroll region
			scrollContent.GetComponent<RectTransform>().offsetMin += new Vector2(0, unitCard.GetComponent<RectTransform>().offsetMax.y);

			// Create unit cards for party
			UnitCard card;
			EventButton button;
			RectTransform rectTransform;
			float offset;
			foreach (var unit in DataManager.Party)
			{
				card = Instantiate(unitCard).GetComponent<UnitCard>();

				// Set parent to container
				card.transform.SetParent(scrollContent.transform, false);

				// Adjust card offset and container height
				rectTransform = card.GetComponent<RectTransform>();
				offset = rectTransform.rect.height + 20;
				rectTransform.ShiftDown(offset * partyPanel.Buttons.Count);
				scrollContent.GetComponent<RectTransform>().offsetMin -= new Vector2(0, offset);

				// Bind card to unit
				card.Bind(unit);
				card.name = unit.ToString();

				// Show the card
				card.Show();

				// Add card to list
				button = card.GetComponent<EventButton>();
				partyPanel.Buttons.Add(button);
			}

			// Bind event handler for populating details panel on select
			//partyPanel.Selected += BindDetails;
			partyPanel.Clicked += BindDetails;
			//partyPanel.Canceled += OnCanceled;
			partyPanel.Draw();

			// Automatically fill detail panel with first unit's information
			//if (partyPanel.Buttons.Count > 0)
			//	BindDetails(0);
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
			foreach (var card in partyPanel.Buttons)
			{
				Destroy(card.gameObject);
			}
			partyPanel.Buttons.Clear();

			// Reset scroll height
			scrollContent.GetComponent<RectTransform>().offsetMin = Vector2.Scale(scrollContent.GetComponent<RectTransform>().offsetMin, new Vector2(1, 0));
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
			if (scrollContent == null) scrollContent = this.GetComponent<CanvasRenderer>();

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