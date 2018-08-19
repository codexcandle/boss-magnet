using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Codebycandle.NCSoftDemo
{
	public class GameText:MonoBehaviour
	{
		private static GameText instance;

		public const string GAME_WIN			= "Great Job, Magnet. You Win!";
		public const string GAME_LOST_BOUNDS 	= "...LOST IN SPACE!";
		public const string GAME_LOST_ENEMY 	= "...THEY GOT YOU!";
		public const string FOUND_POWERUP 		= "MAGNET POWER UPGRADED!";		
		public const string FOUND_NONE 			= "No matches. Keep looking!";
		public const string FOUND_ALL			= "You found them all!";


		public static GameText Instance
		{
			get
			{
				if(instance == null)
				{
					instance = new GameObject("GameText").AddComponent<GameText>();
				}

				return instance;
			}
		}

		public void OnApplicationQuit()
		{
			instance = null;
		}

		public List<string> GetIntroText(int targetCount, string targetName)
		{
			List<string> introText = new List<string>();
			introText.Add("Welcome, Junior Magnet!");
			introText.Add("There's so much work to do!");

			// ...
			string txt = "Bring me ";
			if(targetCount == 1)
			{
				txt += "a " + targetName + " box!";
			}
			else
			{
				txt += targetCount + " " + targetName + " boxes!";
			}
			introText.Add(txt);

			introText.Add("Oh, and beware intruders!");	

			return introText;
		}
	}
}