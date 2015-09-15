using UnityEngine;
using Universal;
using Universal.UI;
using DnDTactics.Data;

namespace DnDTactics.UI.Menu
{
	public class PartyMenu : UIElement
	{
		#region Inspector fields
		[SerializeField]
		private Repeater unitRepeater;
		[SerializeField]
		private UnitDetails unitDetails;
		#endregion

		#region Methods
		public override void Draw()
		{
			unitRepeater.Count = DataManager.Party.Count;
			ChildElements.Add(unitRepeater);

			base.Draw();
		}

		// Buttons will be destroyed in the Clear() call

		public override void Focus()
		{
			unitRepeater.Focus();
		}

		private void OpenUnitDetails(EventButton sender)
		{

		}

		private void CloseUnitDetails()
		{

		}

		private void BindUnitCard(UIElement element, int index)
		{
			element.Data = DataManager.Party[index];
			element.name = DataManager.Party[index].Name;

			EventButton button = element.GetComponent<EventButton>();
			
			button.ID = index;
			button.OnClick += OpenUnitDetails;
		}
		#endregion

		#region Unity events
		protected override void Awake()
		{
			base.Awake();
			unitRepeater.OnDataBind += BindUnitCard;

			unitRepeater.CommandDelegates.Add(EventKey.Cancel, () =>
			{
				CommandDelegates.Raise(EventKey.Cancel);
			});
		}
		#endregion
	}
}