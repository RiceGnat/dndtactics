namespace Universal.UI
{
	/// <summary>
	/// Represents a UI element with its own draw call.
	/// </summary>
    public interface IDrawable
    {
		/// <summary>
		/// Draws the contents of this element.
		/// </summary>
        void Draw();

		/// <summary>
		/// Clears the drawn contents of this element.
		/// </summary>
        void Clear();

		/// <summary>
		/// Redraws this element.
		/// </summary>
        void Refresh();
    }

	/// <summary>
	/// Represents a UI element with toggleable visibility.
	/// </summary>
    public interface IHideable
    {
		/// <summary>
		/// Gets whether this element is visible.
		/// </summary>
		bool IsVisible { get; }

		/// <summary>
		/// Shows this element.
		/// </summary>
        void Show();

		/// <summary>
		/// Hides this element.
		/// </summary>
        void Hide();

		/// <summary>
		/// Sets this element's visibility.
		/// </summary>
		/// <param name="visible">Visibility</param>
		/// <returns>The new visible state</returns>
		bool SetVisibility(bool visible);
    }

	/// <summary>
	/// Represents a UI element that can be focused for input capture.
	/// </summary>
    public interface IFocusable
    {
		/// <summary>
		/// Gets whether this element is focused.
		/// </summary>
		bool IsFocused { get; }

		/// <summary>
		/// Gets the command action labels for this element.
		/// </summary>
		CommandLabelSet CommandLabels { get; }

        /// <summary>
        /// Gets the delegate set for this element.
        /// </summary>
        UnityActionSet CommandDelegates { get; }

		/// <summary>
		/// Focuses this element.
		/// </summary>
        void Focus();

		/// <summary>
		/// Blurs this element.
		/// </summary>
        void Blur();
    }

	/// <summary>
	/// Represents a UI element that can store a generic data object.
	/// </summary>
    public interface IGenericData
    {
		/// <summary>
		/// Gets or sets this element's data object.
		/// </summary>
		object Data { get; set; }

		/// <summary>
		/// Gets this element's data object as a specific type.
		/// </summary>
		/// <typeparam name="T">Return type</typeparam>
		/// <returns>Data object as the specified type</returns>
        T GetData<T>();
    }
}