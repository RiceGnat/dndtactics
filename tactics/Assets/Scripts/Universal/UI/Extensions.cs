using UnityEngine;
using UnityEngine.UI;

namespace Universal.UI
{
	public static class Extensions
	{
		/// <summary>
		/// Shifts the RectTransform down by a specified distance.
		/// </summary>
		/// <param name="offset">Distance to shift</param>
		public static void ShiftDown(this RectTransform rectTransform, float offset)
		{
			rectTransform.offsetMax -= new Vector2(0, offset);
			rectTransform.offsetMin -= new Vector2(0, offset);
		}

		/// <summary>
		/// Shifts the RectTransform right by a specified distance.
		/// </summary>
		/// <param name="offset">Distance to shift</param>
		public static void ShiftRight(this RectTransform rectTransform, float offset)
		{
			rectTransform.offsetMax += new Vector2(offset, 0);
			rectTransform.offsetMin += new Vector2(offset, 0);
		}

		/// <summary>
		/// Adds a child object to the RectTransform, extending the bounds if needed.
		/// </summary>
		/// <param name="obj">Child object</param>
		/// <param name="offsetY">Vertical offset</param>
		/// <param name="offsetX">Horizontal offset</param>
		public static void Append(this RectTransform rectTransform, RectTransform obj, float offsetY = 0, float offsetX = 0)
		{
			obj.transform.SetParent(rectTransform.transform, false);
			obj.ShiftDown(offsetY);
			obj.ShiftRight(offsetX);
			rectTransform.offsetMin = new Vector2(Mathf.Min(rectTransform.offsetMin.x, obj.offsetMin.x), Mathf.Min(rectTransform.offsetMin.y, obj.offsetMin.y));
			rectTransform.offsetMax = new Vector2(Mathf.Max(rectTransform.offsetMax.x, obj.offsetMax.x), Mathf.Max(rectTransform.offsetMax.y, obj.offsetMax.y));
		}

		/// <summary>
		/// Collapses the RectTransform to height 0.
		/// </summary>
		public static void Collapse(this RectTransform rectTransform)
		{
			rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, rectTransform.offsetMax.y);
		}

		/// <summary>
		/// Binds this button's navigation to its preceding button.
		/// </summary>
		/// <param name="prev">The previous button</param>
		public static void BindNavigation(this Button button, Button prev)
		{
			var nav = button.navigation;
			nav.mode = Navigation.Mode.Explicit;
			nav.selectOnUp = prev;
			var prevNav = prev.navigation;
			prevNav.selectOnDown = button;
			prev.navigation = prevNav;
			button.navigation = nav;
		}
	}
}