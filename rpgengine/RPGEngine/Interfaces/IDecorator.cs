namespace RPGEngine
{
    /// <summary>
    /// Defines a method to bind a decorator to unit.
    /// </summary>
    public interface IDecorator
    {
        /// <summary>
        /// Bind this decorator to the given unit.
        /// </summary>
        /// <param name="baseUnit">Unit that this decorator will be bound to.</param>
        /// <returns>The bound decorator cast as an IUnit.</returns>
        IUnit Bind(IUnit baseUnit);
    }
}
