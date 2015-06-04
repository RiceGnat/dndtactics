using UnityEngine;
using System.Collections;

namespace Universal.UI
{
	public class ConfirmDialog : Selector
	{
		public EventButton YesButton
		{
			get { return Buttons[1]; }
			set { Buttons[1] = value; }
		}

		public EventButton NoButton
		{
			get { return Buttons[0]; }
			set { Buttons[0] = value; }
		}
	}
}
