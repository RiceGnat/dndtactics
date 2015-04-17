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
		public Button Base
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

		/// <summary>
		/// Gets or set the button's text.
		/// </summary>
		public string Text
		{
			get { return Base.GetComponent<Text>().text; }
			set
			{
				Base.GetComponent<Text>().text = value;
				Base.name = value;
			}
		}

		/// <summary>
		/// Gets or sets the data for the button.
		/// </summary>
		public object Data { get; set; }

		/// <summary>
		/// Gets the button's data field as a specific type.
		/// </summary>
		/// <typeparam name="T">Type to cast the data to</typeparam>
		/// <returns>Data cast as T</returns>
		public T GetData<T>()
		{
			return (T)Data;
		}

		void ISelectHandler.OnSelect(BaseEventData eventData)
		{
			//if (Debug.isDebugBuild) Debug.Log(name + " selected");
			if (Select != null) Select(Base, eventData);
		}

		void IDeselectHandler.OnDeselect(BaseEventData eventData)
		{
			//if (Debug.isDebugBuild) Debug.Log(name + " deselected");
			if (Deselect != null) Deselect(Base, eventData);
		}

		void ISubmitHandler.OnSubmit(BaseEventData eventData)
		{
			//if (Debug.isDebugBuild) Debug.Log(name + " submitted");
			if (Submit != null) Submit(Base, eventData);
		}

		void ICancelHandler.OnCancel(BaseEventData eventData)
		{
			//if (Debug.isDebugBuild) Debug.Log(name + " canceled");
			if (Cancel != null) Cancel(Base, eventData);
		}
	}
}