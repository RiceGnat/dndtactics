using System.Collections.Generic;

namespace RPGLibrary
{
	/// <summary>
	/// Exposes methods to manage a set of <see cref="IDecorator{T}"/>. Also functions as a <see cref="IDecorator{T}"/>.
	/// </summary>
	/// <typeparam name="T">The type of the object to be decorated.</typeparam>
	public interface IDecoratorManager<T> : IEnumerable<IDecorator<T>>, IDecorator<T>
	{
		/// <summary>
		/// Gets the number of child decorators.
		/// </summary>
		int Count { get; }

		/// <summary>
		/// Gets the outermost child decorator.
		/// </summary>
		T Result { get; }

		/// <summary>
		/// Adds a child decorator.
		/// </summary>
		/// <param name="decorator">The new decorator to be added.</param>
		void Add(IDecorator<T> decorator);

		/// <summary>
		/// Adds child decorators.
		/// </summary>
		/// <param name="decorators">The new decorators to be added.</param>
		void Add(params IDecorator<T>[] decorators);

		/// <summary>
		/// Removes a decorator from the children.
		/// </summary>
		/// <param name="decorator">The decorator to be removed.</param>
		void Remove(IDecorator<T> decorator);

		/// <summary>
		/// Removes a decorator at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the decorator to remove.</param>
		/// <returns>The <see cref="IDecorator{T}"/> that was removed.</returns>
		IDecorator<T> RemoveAt(int index);

		/// <summary>
		/// Gets the decorator at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the decorator to remove.</param>
		/// <returns>The <see cref="IDecorator{T}"/> at the specified index.</returns>
		IDecorator<T> this[int index] { get; }

		/// <summary>
		/// Gets the child decorators that match the type specified by <typeparamref name="Tsub"/>.
		/// </summary>
		/// <typeparam name="Tsub">The class type used to filter the children.</typeparam>
		/// <returns>The subset of child decorators of type <typeparamref name="Tsub"/>.</returns>
		IList<Tsub> GetSubset<Tsub>() where Tsub : class;
	}
}
