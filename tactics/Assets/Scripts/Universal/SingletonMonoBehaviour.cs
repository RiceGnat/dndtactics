using UnityEngine;
using System.Collections;

namespace Universal
{
	public abstract class SingletonMonoBehaviour : MonoBehaviour
	{
		protected abstract SingletonMonoBehaviour Instance { get; set; }

		protected virtual void Init()
		{

		}

		private void Awake()
		{
			if (Instance != null)
			{
				if (Debug.isDebugBuild) Debug.LogWarning("Only one instance of type " + this.GetType().ToString() + " allowed. Destroying " + name);
				Destroy(gameObject);
			}
			else
			{
				Instance = this;
				Init();
			}
		}
	}
}