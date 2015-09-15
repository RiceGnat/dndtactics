using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RPGLibrary
{
	[Serializable]
	public class DecoratorManager<T> : IDecoratorManager<T>
		where T : class
	{
		private List<IDecorator<T>> master = new List<IDecorator<T>>();
		[NonSerialized]
		private T target;

		public T Target
		{
			get
			{
				return target;
			}
		}

		public int Count
		{
			get
			{
				return master.Count;
			}
		}

		public T Result
		{
			get
			{
				return (Count == 0) ? Target : master[Count - 1] as T;
			}
		}

		public void Add(IDecorator<T> decorator)
		{
			if (!(decorator is T))
			{
				throw new ArgumentException(String.Format("Decorator should share the {0} interface with the target", typeof(T)));
			}
			decorator.Bind(Result);
			master.Add(decorator);
		}

		public void Add(params IDecorator<T>[] decorators)
		{
			foreach (IDecorator<T> decorator in decorators)
			{
				Add(decorator);
			}
		}

		public void Remove(IDecorator<T> decorator)
		{
			RemoveAt(master.IndexOf(decorator));
		}

		public void RemoveAt(int index)
		{
			if (index > -1)
			{
				if (Count > 1)
				{
					if (index == 0)
					{
						master[index + 1].Bind(Target);
					}
					else
					{
						master[index + 1].Bind(master[index - 1] as T);
					}
				}

				master.RemoveAt(index);
			}
		}

		public IList<IDecorator<T>> Children
		{
			get
			{
				List<IDecorator<T>> list = new List<IDecorator<T>>();

				foreach (IDecorator<T> decorator in master)
				{
					list.Add(decorator);
					if (decorator.Children != null) list.AddRange(decorator.Children);
				}

				return list;
			}
		}

		public IList<U> GetSubsetOfType<U>()
			where U : class
		{
			IList<U> list = new List<U>();

			foreach (IDecorator<T> decorator in Children)
			{
				if (decorator is U)
					list.Add(decorator as U);
			}

			return list;
		}

		public void Bind(T target)
		{
			this.target = target;
			if (Count > 0) master[0].Bind(Target);
		}

		[OnDeserialized]
		private void RebindDecorators(StreamingContext context)
		{
			for (int i = 1; i < Count; i++)
			{
				master[i].Bind(master[i - 1] as T);
			}
		}

		#region IEnumerable
		IEnumerator<IDecorator<T>> IEnumerable<IDecorator<T>>.GetEnumerator()
		{
			return master.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return master.GetEnumerator();
		}
		#endregion

		public DecoratorManager() { }

		public DecoratorManager(T target)
		{
			Bind(target);
		}
	}
}
