using UnityEngine;
using System.Collections;

namespace Universal
{
	public sealed class EventKey
	{
		public string Name { get; private set; }

		public EventKey(string name)
		{
			Name = name;
		}

		public static EventKey Submit = new EventKey("Submit");
		public static EventKey Submit = new EventKey("Cancel");
		public static EventKey Submit = new EventKey("ButtonX");
		public static EventKey Submit = new EventKey("ButtonY");
		public static EventKey Submit = new EventKey("ButtonL");
		public static EventKey Submit = new EventKey("ButtonR");
	}
}