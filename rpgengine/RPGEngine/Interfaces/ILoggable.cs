namespace RPGEngine
{
    /// <summary>
    /// Defines properties that get long and short print-friendly descriptions of an object.
    /// </summary>
    public interface ILoggable
    {
        /// <summary>
        /// Gets the short description of this object.
        /// </summary>
        string Summary { get; }

        /// <summary>
        /// Gets the full description of this object.
        /// </summary>
        string Full { get; }
    }
}
