using UnityEngine;

namespace DnDTactics.UI
{
	public class Startup : MonoBehaviour
	{
		void Start()
		{
			UIManager.UnitDetailsPage.gameObject.SetActive(false);
			UIManager.GameMenu.Activate();
		}
	}
}
