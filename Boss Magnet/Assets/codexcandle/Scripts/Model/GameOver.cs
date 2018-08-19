using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Codebycandle.NCSoftDemo
{
	public class GameOver:MonoBehaviour
	{
		public void HandleClick()
		{
			GameState.Instance.StartState();
		}
	}
}