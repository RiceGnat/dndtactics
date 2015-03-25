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
	}
}