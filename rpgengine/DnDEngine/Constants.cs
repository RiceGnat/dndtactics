namespace DnDEngine
{
	/// <summary>
	/// Contains various strings and values for convenience.
	/// </summary>
    public static class Constants
    {
		/// <summary>
		/// Use with ToString() function to display +/- sign for modifiers.
		/// </summary>
        public static string ModifierFormat { get { return "+#;-#;+0"; } }

		/// <summary>
		/// Use with ToString() function to display +/- sign for modifiers and a dash for 0.
		/// </summary>
		public static string EquipmentModifierFormat { get { return "+#;-#;–"; } }

		/// <summary>
		/// Gets the appropriate pronoun for a given gender.
		/// </summary>
		/// <param name="gender">Gender enum</param>
		/// <returns>The pronoun corresponding to the given gender. His, her, or its, no special snowflakes here.</returns>
        public static string PossessivePronoun(DnDUnit.GenderType gender)
        {
            return gender == DnDUnit.GenderType.Male ? "his" : gender == DnDUnit.GenderType.Female ? "her" : "its";
        }
    }
}
