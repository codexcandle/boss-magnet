using UnityEngine;

namespace Codebycandle.BossMagnet
{
	public class GameOver:MonoBehaviour
	{
		public void HandleClick()
		{
			GameState.Instance.StartState();
		}
	}
}