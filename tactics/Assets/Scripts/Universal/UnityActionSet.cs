using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

namespace Universal
{
	public sealed class UnityActionSet
	{
		private Dictionary<EventKey, UnityAction> events = new Dictionary<EventKey, UnityAction>();

		public void Add(EventKey key, UnityAction handler)
		{
			UnityAction q;
			events.TryGetValue(key, out q);
			events[key] = (UnityAction)Delegate.Combine(q, handler);
		}

		public void Remove(EventKey key, UnityAction handler)
		{
			UnityAction q;
			if (events.TryGetValue(key, out q))
			{
				q = (UnityAction)Delegate.Remove(q, handler);

				if (q != null) events[key] = q;
				else events.Remove(key);
			}
		}

		public void Raise(EventKey key)
		{
			UnityAction q;
			events.TryGetValue(key, out q);

			if (q != null) q();
		}
	}
}
