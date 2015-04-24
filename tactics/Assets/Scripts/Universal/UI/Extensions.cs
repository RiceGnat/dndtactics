using UnityEngine;
using UnityEngine.UI;
using Universal;

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
		public static void Append(this RectTransform parent, RectTransform obj, float offsetY = 0, float offsetX = 0)
		{
			obj.transform.SetParent(parent.transform, false);
			obj.ShiftDown(offsetY);
			obj.ShiftRight(offsetX);

			Vector2 diffMin = Vector2.Scale(new Vector2(parent.rect.width, parent.rect.height), obj.anchorMin) + obj.offsetMin;
			Vector2 diffMax = Vector2.Scale(new Vector2(-parent.rect.width, -parent.rect.height), Vector2.one - obj.anchorMax) + obj.offsetMax;

			parent.offsetMin += new Vector2(Mathf.Min(diffMin.x, 0), Mathf.Min(diffMin.y, 0));
			parent.offsetMax += new Vector2(Mathf.Max(diffMax.x, 0), Mathf.Max(diffMax.y, 0));
		}

		/// <summary>
		/// Collapses the bottom of the RectTransform to the top.
		/// </summary>
		public static void CollapseUp(this RectTransform rectTransform)
		{
			rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, rectTransform.offsetMax.y);
		}

		/// <summary>
		/// Collapses the top of the RectTransform to the bottom.
		/// </summary>
		public static void CollapseDown(this RectTransform rectTransform)
		{
			rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, rectTransform.offsetMin.y);
		}

		/// <summary>
		/// Binds this button's navigation to its preceding button.
		/// </summary>
		/// <param name="prev">The previous button</param>
		public static void BindNavigation(this Button button, Button prev)
		{
			var nav = button.navigation;
			nav.mode = Navigation.Mode.Explicit;
			if (prev != null)
			{
				nav.selectOnUp = prev;
				var prevNav = prev.navigation;
				prevNav.selectOnDown = button;
				prev.navigation = prevNav;
			}
			button.navigation = nav;
		}
	}
}