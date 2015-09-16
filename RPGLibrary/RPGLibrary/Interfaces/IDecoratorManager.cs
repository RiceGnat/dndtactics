using System.Collections.Generic;

namespace RPGLibrary
{
	public interface IDecoratorManager<T> : IEnumerable<IDecorator<T>>, IDecorator<T>
	{
		int Count { get; }
		T Result { get; }

		void Add(IDecorator<T> decorator);
		void Add(params IDecorator<T>[] decorators);
		void Remove(IDecorator<T> decorator);
		IDecorator<T> RemoveAt(int index);

		IDecorator<T> this[int index] { get; }

		IList<Tsub> GetSubsetOfType<Tsub>() where Tsub : class;
	}
}
