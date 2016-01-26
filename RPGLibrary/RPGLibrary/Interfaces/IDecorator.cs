using System.Collections.Generic;

namespace RPGLibrary
{
	/// <summary>
	/// Exposes properties and methods for the object decorators.
	/// </summary>
	/// <typeparam name="T">The type of the object to be decorated.</typeparam>
	/// <remarks>
	/// Note that for the decorator design pattern to work properly, the decorator must also inherit from <typeparamref name="T"/>.
	/// </remarks>
	public interface IDecorator<T>
	{
		/// <summary>
		/// Gets the object being decorated.
		/// </summary>
		T Target { get; }

		/// <summary>
		/// Binds the decorator to an object.
		/// </summary>
		/// <param name="target">The object to be decorated.</param>
		void Bind(T target);

		/// <summary>
		/// Gets a list of the decorator's children. Used for compound decorators.
		/// </summary>
		IList<IDecorator<T>> Children { get; }
	}
}
