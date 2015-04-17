using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using DnDEngine;
using DnDEngine.Items;
using Universal.UI;

namespace DnDTactics.UI
{
	public class UnitDetails : UnitCard
	{
		#region Inspector fields
		[SerializeField]
		private UnitEquipmentWindow equipmentPanel;
		[SerializeField]
		private UnitInventoryWindow inventoryPanel;
		[SerializeField]
		private ItemPopout popout;
		#endregion

		private RectTransform equipmentContainer { get { return equipmentPanel.GetComponent<ScrollRect>().content; } }

		private EquipIndex lastActiveEquipSlot;
		
		private void EquipItem(int index, object data)
		{
			

			inventoryPanel.Clicked -= EquipItem;
		}

		public override void Draw()
		{
			base.Draw();

			// Draw equipment panel
			equipmentPanel.Data = Unit;
			equipmentPanel.Draw();

			
			// Draw inventory panel
			inventoryPanel.Data = Unit;
			inventoryPanel.Draw();

			// Activate input handlers
			Activate();
		}

		public override void Clear()
		{
			base.Clear();

			// Deactivate input handlers
			Deactivate();

			// Clear equipment panel
			equipmentPanel.Clear();
			inventoryPanel.Clear();
		}

		public override void Activate()
		{
			base.Activate();

			popout.Hide();

			EventSystem.current.SetSelectedGameObject(null);
		}

		#region Unity events
		protected override void Awake()
		{
			base.Awake();

			// Bind UIPanel events (these will not be cleared)
			Submitted += () =>
			{
				Deactivate();
				// Activate equipment if A button is pressed
				equipmentPanel.Activate();
			};

			equipmentPanel.Selected += (int index) =>
			{
				EquipIndex target = equipmentPanel.Buttons[index].Data as EquipIndex;
				if (target.Equip == null) popout.Hide();
				else if (!popout.gameObject.activeSelf) popout.Show();
				popout.Data = target.Equip;
				popout.Refresh();
			};

			equipmentPanel.Clicked += (int index, object data) =>
			{
				EquipIndex target = data as EquipIndex; 
				if (target.Equip == null)
				{
					// Activate inventory screen to select equipment
					lastActiveEquipSlot = data as EquipIndex;
					Deactivate();
					inventoryPanel.Activate();
					inventoryPanel.Clicked += EquipItem;
				}
			};

			equipmentPanel.Canceled += () =>
			{
				equipmentPanel.Deactivate();
				Activate();
			};

			equipmentPanel.ButtonX += () =>
			{
				EventButton current = EventSystem.current.currentSelectedGameObject.GetComponent<EventButton>();
				int index = current.ID;
				if (equipmentPanel.Buttons.Contains(current))
				{
					// Unequip and move to inventory
					Equipment equip = (current.Data as EquipIndex).Equip;
					if (equip != null)
					{
						var slot = equip.Slot;
						var list = equipmentPanel.UnitEquipment[slot];
						inventoryPanel.UnitInventory.All.Add(equip);
						list[list.IndexOf(equip)] = null;
						Unit.Evaluate();
						Refresh();
						Deactivate();
						EventSystem.current.SetSelectedGameObject(equipmentPanel.Buttons[index].gameObject);
					}
				}
			};

			equipmentPanel.BumperR += () =>
			{
				equipmentPanel.Deactivate();
				inventoryPanel.Activate();
			};

			inventoryPanel.Canceled += () =>
			{
				inventoryPanel.Deactivate();
				Activate();
			};

			inventoryPanel.ButtonX += () =>
			{
				EventButton current = EventSystem.current.currentSelectedGameObject.GetComponent<EventButton>();
				int index = current.ID;
				if (inventoryPanel.Buttons.Contains(current) && current.Data != null)
				{
					if (inventoryPanel.IsCaravan)
					{
						// Move item from caravan to inventory
						inventoryPanel.UnitInventory.All.Add(DataManager.Caravan[index]);
						DataManager.Caravan.RemoveAt(index);
					}
					else
					{
						// Move item from inventory to caravan
						DataManager.Caravan.Add(inventoryPanel.UnitInventory.All[index]);
						inventoryPanel.UnitInventory.All.RemoveAt(index);
					}

					inventoryPanel.Refresh();
					EventSystem.current.SetSelectedGameObject(inventoryPanel.Buttons[index].gameObject);
				}
			};

			inventoryPanel.BumperL += () =>
			{
				inventoryPanel.Deactivate();
				equipmentPanel.Activate();
			};
		}

		protected override void Update()
		{
			base.Update();

			if (IsActivated)
			{

			}
		}
		#endregion
	}
}