using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

namespace Universal.UI
{
	[RequireComponent(typeof(RectTransform))]
	public class UIElement : MonoBehaviour, IDrawable, IHideable, IFocusable, IGenericData
	{
		#region Inspector fields
		//[SerializeField]
		//private bool initiallyVisible = true;
		[SerializeField]
		private CommandLabelSet commandLabels;
		#endregion

		#region Fields
		private RectTransform rectTrans;
		private List<UIElement> childElements = new List<UIElement>();
		private UnityActionSet delegates = new UnityActionSet();
		#endregion

		#region Properties
		/// <summary>
		/// The RectTransform attached to this UIElement's game object
		/// </summary>
		public RectTransform rectTransform
		{
			get
			{
				if (rectTrans == null) rectTrans = GetComponent<RectTransform>();
				return rectTrans;
			}
		}

		public ICollection<UIElement> ChildElements
		{
			get
			{
				return childElements;
			}
		}

		public bool IsVisible
		{
			get
			{
				return gameObject.activeSelf;
			}
		}

		public bool IsFocused { get; private set; }

		public CommandLabelSet CommandLabels { get { return commandLabels; } }

		public UnityActionSet CommandDelegates { get { return delegates; } }

		public object Data { get; set; }
		#endregion

		#region Events
		public event UnityAction OnDraw;
		public event UnityAction OnClear;
		public event UnityAction OnShow;
		public event UnityAction OnHide;
		#endregion

		#region Methods
		public virtual void Draw()
		{
			#if DEBUG
			if (Debug.isDebugBuild) Debug.Log("Drawing " + name);
			#endif

			OnDraw.Raise();
			
			// Search for UIElements in immediate children and add to ChildElements
			foreach (Transform child in transform)
			{
				UIElement element = child.GetComponent<UIElement>();

				if (element != null && element.IsVisible)
				{
					ChildElements.Add(element);
				}
			}

			// Separate loop to cover manually added children
			foreach (UIElement child in ChildElements)
			{
				child.Draw();
			}
		}

		public virtual void Clear()
		{
			#if DEBUG
			if (Debug.isDebugBuild) Debug.Log("Clearing " + name);
			#endif

			OnClear.Raise();

			foreach (UIElement child in ChildElements)
			{
				child.Clear();
			}

			ChildElements.Clear();
		}

		public virtual void Refresh()
		{
			Clear();
			Draw();
		}

		public virtual void Show()
		{
			#if DEBUG
			if (Debug.isDebugBuild) Debug.Log("Showing " + name);
			#endif

			gameObject.SetActive(true);
			OnShow.Raise();
		}

		public virtual void Hide()
		{
			#if DEBUG
			if (Debug.isDebugBuild) Debug.Log("Hiding " + name);
			#endif

			OnHide.Raise();
			gameObject.SetActive(false);
		}

		public virtual bool SetVisibility(bool visible)
		{
			if (visible) Show();
			else Hide();
			return IsVisible;
		}

		public virtual void Focus()
		{
			#if DEBUG
			if (Debug.isDebugBuild) Debug.Log("Focusing " + name);
			#endif

			CommandManager.BlurCurrent();
			IsFocused = true;
			CommandManager.CurrentFocused = this;
		}

		public virtual void Blur()
		{
			#if DEBUG
			if (Debug.isDebugBuild) Debug.Log("Blurring " + name);
			#endif

			IsFocused = false;
			if (this.Equals(CommandManager.CurrentFocused)) CommandManager.CurrentFocused = null;
		}

		public virtual T GetData<T>()
		{
			return (T)Data;
		}
		#endregion

		#region Unity events
		protected virtual void Awake()
		{
		}

		protected virtual void Start()
		{
		}

		protected virtual void Update()
		{
		}
		#endregion
	}
}