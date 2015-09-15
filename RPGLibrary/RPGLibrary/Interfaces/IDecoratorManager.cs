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
		void RemoveAt(int index);

		IList<U> GetSubsetOfType<U>() where U : class;
	}
}
