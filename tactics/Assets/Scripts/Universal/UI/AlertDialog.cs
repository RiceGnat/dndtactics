using UnityEngine;
using System.Collections;

namespace Universal.UI
{
	public sealed class AlertDialog : Selector
	{
		#region Inspector fields
		[SerializeField]
		private EventButton okButton;
		#endregion

		public EventButton OkButton
		{
			get { return okButton; }
		}
	}
}
