using UnityEngine;
using System.Collections;

namespace Universal
{
	public sealed class EventKey
	{
		private static int index = 0;

		public string Name { get; private set; }
		public int Index { get; private set; }

		private EventKey(string name)
		{
			Name = name;
			Index = index;
			index++;
		}

		public static EventKey Submit = new EventKey("Submit");
		public static EventKey Cancel = new EventKey("Cancel");
		public static EventKey ButtonX = new EventKey("Button X");
		public static EventKey ButtonY = new EventKey("Button Y");
		public static EventKey BumperL = new EventKey("Bumper L");
		public static EventKey BumperR = new EventKey("Bumper R");
	}
}