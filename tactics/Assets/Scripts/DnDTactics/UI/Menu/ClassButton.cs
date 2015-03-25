using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DnDEngine;

namespace DnDTactics.UI
{
	/// <summary>
	/// Contains a DnDUnit.ClassType value.
	/// </summary>
	public class ClassButton : Universal.UI.EventButton
	{
		#region Inspector fields
		[SerializeField]
		private DnDUnit.ClassType @class;
		[SerializeField]
		private DnDUnit.GenderType gender;
		#endregion

		/// <summary>
		/// Gets or sets the DnDUnit.ClassType value for this button.
		/// </summary>
		public DnDUnit.ClassType Class
		{
			get { return @class; }
			set
			{
				@class = value;
				GetComponent<Text>().text = DataManager.GetClassDisplayName(value);
			}
		}

		/// <summary>
		/// Gets or sets the DnDUnit.GenderType value for this button.
		/// </summary>
		public DnDUnit.GenderType Gender
		{
			get { return gender; }
			set { gender = value; }
		}
	}
}
