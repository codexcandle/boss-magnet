using UnityEngine;

namespace Codebycandle.BossMagnet
{
	public class GameStart:MonoBehaviour
	{
		public void HandleClick()
		{
			StartGame();
		}

		private void StartGame()
		{
			DontDestroyOnLoad(GameState.Instance);
			
			GameState.Instance.StartState();
		}
	}
}