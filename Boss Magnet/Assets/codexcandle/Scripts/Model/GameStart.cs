using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Codebycandle.NCSoftDemo
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