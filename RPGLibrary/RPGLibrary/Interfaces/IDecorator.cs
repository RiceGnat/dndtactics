using System.Collections.Generic;

namespace RPGLibrary
{
	public interface IDecorator<T>
	{
		T Target { get; }
		void Bind(T target);

		IList<IDecorator<T>> Children { get; }
	}
}
