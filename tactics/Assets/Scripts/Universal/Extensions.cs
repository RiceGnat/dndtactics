using UnityEngine;
using UnityEngine.Events;

namespace Universal
{
	public static class Extensions
	{
		public static void Raise(this UnityAction myEvent)
		{
			if (myEvent != null) myEvent();
		}

		public static void SetParent(this Component component, Component parent)
		{
			component.transform.SetParent(parent.transform, false);
		}
	}
}