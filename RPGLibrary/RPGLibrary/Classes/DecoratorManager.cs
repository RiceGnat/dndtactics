using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RPGLibrary
{
	/// <summary>
	/// Manages child decorators. Can be used as a compound decorator.
	/// </summary>
	/// <typeparam name="T">The type of the object to be decorated.</typeparam>
	[Serializable]
	public class DecoratorManager<T> : IDecoratorManager<T>
		where T : class
	{
		private List<IDecorator<T>> master = new List<IDecorator<T>>();

		[NonSerialized]
		private T target;

		public T Target
		{
			get { return target; }
		}

		public int Count
		{
			get { return master.Count; }
		}

		public T Result
		{
			get { return (Count == 0) ? Target : master[Count - 1] as T; }
		}

		public virtual void Add(IDecorator<T> decorator)
		{
			if (!(decorator is T))
			{
				throw new ArgumentException(String.Format("Decorator should share the {0} interface with the target.", typeof(T)));
			}

			if (master.Contains(decorator))
			{
				throw new ArgumentException(String.Format("Cannot add the same decorator more than once."));
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

		public virtual IDecorator<T> RemoveAt(int index)
		{
			IDecorator<T> removed = null;

			if (index > -1)
			{
				removed = master[index];

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
				removed.Bind(null);
			}

			return removed;
		}

		public IDecorator<T> this[int index]
		{
			get
			{
				return master[index];
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

		public IList<Tsub> GetSubset<Tsub>()
			where Tsub : class
		{
			IList<Tsub> list = new List<Tsub>();

			foreach (IDecorator<T> decorator in Children)
			{
				if (decorator is Tsub)
					list.Add(decorator as Tsub);
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
