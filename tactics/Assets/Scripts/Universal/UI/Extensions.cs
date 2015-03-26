using UnityEngine;

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

		public static void Append(this RectTransform rectTransform, RectTransform obj, float offsetY = 0, float offsetX = 0)
		{
			obj.transform.SetParent(rectTransform.transform, false);
			obj.ShiftDown(offsetY);
			obj.ShiftRight(offsetX);
			rectTransform.offsetMin = new Vector2(Mathf.Min(rectTransform.offsetMin.x, obj.offsetMin.x), Mathf.Min(rectTransform.offsetMin.y, obj.offsetMin.y));
			rectTransform.offsetMax = new Vector2(Mathf.Max(rectTransform.offsetMax.x, obj.offsetMax.x), Mathf.Max(rectTransform.offsetMax.y, obj.offsetMax.y));
		}
	}
}