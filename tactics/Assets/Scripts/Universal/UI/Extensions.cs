using UnityEngine;
using UnityEngine.UI;

namespace Universal.UI
{
	public static class Extensions
	{
		/// <summary>
		/// Calls the event delegates. Automatically checks for null.
		/// </summary>
		/// <param name="myEvent">Event to raise</param>
		public static void Raise(this EventButtonHandler myEvent, EventButton sender)
		{
			if (myEvent != null) myEvent(sender);
		}

	}
}