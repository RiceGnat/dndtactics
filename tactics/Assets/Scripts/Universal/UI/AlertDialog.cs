using UnityEngine;
using System.Collections;

namespace Universal.UI
{
	public class AlertDialog : Window
	{
		public EventButton OkButton
		{
			get { return Buttons[0]; }
			set { Buttons[0] = value; }
		}
	}
}
