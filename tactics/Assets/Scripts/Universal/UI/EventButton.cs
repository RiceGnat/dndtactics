using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

namespace Universal.UI
{
	/// <summary>
	/// Pairs with a UnityEngine.UI.Button to provide events with sender data.
	/// </summary>
	[Serializable]
	[RequireComponent(typeof(Button))]
	public class EventButton : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler, ICancelHandler
	{
		#region Events
		public event SelectableEventHandler Select;
		public event SelectableEventHandler Deselect;
		public event SelectableEventHandler Submit;
		public event SelectableEventHandler Cancel;
		#endregion

		private Button selectable;
		/// <summary>
		/// Gets the UnityEngine.UI.Button Component that this object is paired with.
		/// </summary>
		public Button Selectable
		{
			get
			{
				if (selectable == null) selectable = GetComponent<Button>();
				return selectable;
			}
		}

		/// <summary>
		/// Gets or sets the ID of this button.
		/// </summary>
		public int ID { get; set; }

		public void SetText(string text)
		{
			Selectable.GetComponent<Text>().text = text;
		}

		void ISelectHandler.OnSelect(BaseEventData eventData)
		{
			//if (Debug.isDebugBuild) Debug.Log(name + " selected");
			if (Select != null) Select(Selectable, eventData);
		}

		void IDeselectHandler.OnDeselect(BaseEventData eventData)
		{
			//if (Debug.isDebugBuild) Debug.Log(name + " deselected");
			if (Deselect != null) Deselect(Selectable, eventData);
		}

		void ISubmitHandler.OnSubmit(BaseEventData eventData)
		{
			//if (Debug.isDebugBuild) Debug.Log(name + " submitted");
			if (Submit != null) Submit(Selectable, eventData);
		}

		void ICancelHandler.OnCancel(BaseEventData eventData)
		{
			//if (Debug.isDebugBuild) Debug.Log(name + " canceled");
			if (Cancel != null) Cancel(Selectable, eventData);
		}
	}
}