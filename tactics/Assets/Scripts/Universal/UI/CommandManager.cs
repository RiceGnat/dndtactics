using UnityEngine;
using System;

namespace Universal.UI
{
	public sealed class CommandManager : SingletonMonoBehaviour
	{
		#region Singleton
		private static SingletonMonoBehaviour instance;

		protected override SingletonMonoBehaviour Instance
		{
			get { return instance; }
			set { instance = value; }
		}
		#endregion

		private static EventKey[] capturedInputs = new EventKey[] {
            EventKey.Submit,
            EventKey.ButtonX,
            EventKey.ButtonY,
            EventKey.BumperL,
            EventKey.BumperR,
            EventKey.Cancel
        };

        public static EventKey[] CapturedInputs
        {
            get
            {
                return capturedInputs;
            }
        }

		public static IFocusable CurrentFocused { get; set; }

        public static void BlurCurrent()
        {
            if (CurrentFocused != null) CurrentFocused.Blur();
        }

        private void Update()
        {
            if (CurrentFocused != null)
            {
                // Check captured inputs
                foreach (EventKey key in capturedInputs)
                {
                    if (Input.GetButtonDown(key.Name))
                    {
                        if (Debug.isDebugBuild) Debug.Log(String.Format("{0} event on {1}", key.Name, name));
                        CurrentFocused.CommandDelegates.Raise(key);

                        // Reset inputs for the rest of this frame to prevent cascading 
                        Input.ResetInputAxes();
                    }
                }
            }
        }
	}
}