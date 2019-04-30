using UnityEngine;
using UnityEngine.SceneManagement;

namespace Codebycandle.BossMagnet
{
	public class GameState:MonoBehaviour
	{
		private static GameState instance;

		private const string SCENE_PLAY = "01_play";
		private const string SCENE_OVER = "02_over";

		private int _maxHP;
		public int maxHP
		{
			get
			{
				return _maxHP;
			}
		}	

		private int _hp;
		public int hp
		{
			get
			{
				return _hp;
			}
			set
			{
				_hp = value;
			}
		}		

		private int _score;
		public int score
		{
			get
			{
				return _score;
			}
			set
			{
				_score = value;
			}
		}

		public static GameState Instance
		{
			get
			{
				if(instance == null)
				{
					instance = new GameObject("GameState").AddComponent<GameState>();
				}

				return instance;
			}
		}

		public void OnApplicationQuit()
		{
			instance = null;
		}

		public void StartState()
		{
			// init
			_hp = _maxHP = 100;
			score = 0;

			// load
			SceneManager.LoadScene(SCENE_PLAY, LoadSceneMode.Single);	
		}

		public void EndState()
		{
			SceneManager.LoadScene(SCENE_OVER, LoadSceneMode.Single);
		}
	}
}