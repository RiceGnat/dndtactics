namespace RPGEngine
{
    /// <summary>
    /// Represents a catalogable item.
    /// </summary>
    public interface ICatalogable
    {
        /// <summary>
        /// Gets the internal ID for this item.
        /// </summary>
        uint ID { get; }
        /// <summary>
        /// Gets the name of this item.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Gets the description of this item.
        /// </summary>
        string Description { get; }
    }
}
