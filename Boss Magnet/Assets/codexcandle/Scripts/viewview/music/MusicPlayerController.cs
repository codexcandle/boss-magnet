using UnityEngine;

namespace Codebycandle.BossMagnet
{
	public class MusicPlayerController:MonoBehaviour
	{
		private AudioSource audioSource;

		public void PlayMusic(bool play)
		{
			if(play)
			{
				audioSource.enabled = true;
				
				audioSource.Play();
			}
			else
			{
				audioSource.Stop();

				audioSource.enabled = true;
			}
		}	

		void Awake()
		{
			audioSource = GetComponent<AudioSource>();
		}

		void Start()
		{
			PlayMusic(true);
		}

		void OnApplicationQuit()
		{
			PlayMusic(false);			
		}
	}
}