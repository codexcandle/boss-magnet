using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Codebycandle.BossMagnet
{
	[RequireComponent(typeof(ObjectPooler), typeof(AudioSource))]
	public class LevelLogic:MonoBehaviour
	{
        #region VARS (camera)
        [SerializeField]
		private CameraController cameraController;
        #endregion

        #region VARS (player)
        private PlayerController playerController;		
		private Transform player;
        #endregion

        #region VARS (boss)
        [SerializeField]
		private PromptController promptController;			
		[SerializeField]
		private Transform bossCameraPosition;
		private bool bossBusy;
        #endregion

        #region VARS (enemy)
        [SerializeField]
		private GameObject enemyRoot;
        #endregion

        #region VARS (mini-map)		
        [SerializeField]
		private MinimapViewController minimapViewController;
        #endregion

        #region VARS (audio)
        [SerializeField]
		private MusicPlayerController musicPlayerController;
        private AudioSource audioSource;
        public AudioClip wonClip;
        public AudioClip lostClip;
        #endregion

        #region VARS (debris)
        [SerializeField]
		private int debrisCount = 5;
		[SerializeField]
		private int targetDebrisReductionFactor = 10;
		private int targetDebrisCount;		
		[SerializeField]
		private float debrisHeight = 6F;
		[SerializeField]
		private int posScale;						
		private Debris.Kind _targetDebrisKind;
		public Debris.Kind targetDebrisKind
		{
			get
			{
				return _targetDebrisKind;
			}
		}
        #endregion

        #region VARS (powerup)
        [SerializeField]		
		private PowerupUIController powerupUIController;
        #endregion

        #region VARS (scoreboard)	
        [SerializeField]
		private ScoreboardController scoreboardController;
        #endregion

        void Start()
		{
			// get refs
			player = GameObject.FindWithTag(GameTag.TAG_PLAYER).transform;		
			playerController = player.GetComponent<PlayerController>();
			audioSource = GetComponent<AudioSource>();

			// generate debris
 			// ----------------------------------------------------------
			// ... ensure pos val
			if(debrisCount <= 0)
				debrisCount = 1;

			int maxTargetDebrisCount = debrisCount / targetDebrisReductionFactor;
			if(maxTargetDebrisCount < 1)
			{
				maxTargetDebrisCount = 1;
			}
			targetDebrisCount = Random.Range(1, maxTargetDebrisCount);
			_targetDebrisKind = GetRandomDebrisKind();
 			// ----------------------------------------------------------			

			// begin fun!
			StartCoroutine(GameLoop());
		}

        #region METHODS (game)
        private IEnumerator GameLoop()
        {
            // pre-init
            // *******************************************
            // disable everything
            enemyRoot.SetActive(false);
            powerupUIController.ShowView(false);
            scoreboardController.ShowView(false);
            minimapViewController.ShowView(false);
            playerController.ShowHealthMeter(false);

            // make boxes
            CreateDebris(debrisCount, targetDebrisCount, _targetDebrisKind);

            yield return new WaitForSeconds(2);

            // focus camera on boss
            // *******************************************
            cameraController.ZoomTo(bossCameraPosition);

            yield return new WaitForSeconds(2);

            // set game text
            // *******************************************
            string targetName = _targetDebrisKind.ToString().ToUpper();
            List<string> introText = GameText.Instance.GetIntroText(targetDebrisCount, targetName);
            SetPromptText(introText[0]);

            yield return new WaitForSeconds(2);

            // update game text
            // *******************************************
            SetPromptText(introText[1]);

            yield return new WaitForSeconds(2);

            // reveal ui #1
            // *******************************************
            scoreboardController.ShowView(true);
            minimapViewController.ShowView(true);

            SetPromptText(introText[2]);

            UpdateScore(0, targetDebrisCount, targetName);

            yield return new WaitForSeconds(3);

            // reveal ui #2 (& update game text #2)
            // *******************************************
            SetPromptText(introText[3]);

            playerController.ShowHealthMeter(true);

            yield return new WaitForSeconds(3);

            // update game text #3 (& zoom camera to player)
            // *******************************************
            SetPromptText();

            cameraController.FocusOn(player.gameObject, 4);

            yield return new WaitForSeconds(5);

            // enable enemies / player
            // *******************************************
            enemyRoot.SetActive(true);

            playerController.EnableMovement(true);
        }

        IEnumerator HandleWin()
        {
            playerController.EnableMovement(false);

            scoreboardController.ShowView(false);

            enemyRoot.SetActive(false);

            musicPlayerController.PlayMusic(false);

            SetPromptText(GameText.FOUND_ALL);

            PlaySound(wonClip);

            yield return new WaitForSeconds(3);

            cameraController.ZoomTo(bossCameraPosition);

            SetPromptText(GameText.GAME_WIN);

            yield return new WaitForSeconds(5);

            EndGame();
        }

        IEnumerator EndGameFromEnemy()
        {
            playerController.EnableMovement(false);

            playerController.ShowHealthMeter(false);

            scoreboardController.ShowView(false);

            minimapViewController.ShowView(false);

            musicPlayerController.PlayMusic(false);

            SetPromptText(GameText.GAME_LOST_ENEMY);

            PlaySound(lostClip);

            yield return new WaitForSeconds(4);

            EndGame();
        }

        IEnumerator EndGameFromBoundsExceeded()
        {
            playerController.EnableMovement(false);

            playerController.ShowHealthMeter(false);

            scoreboardController.ShowView(false);

            minimapViewController.ShowView(false);

            musicPlayerController.PlayMusic(false);

            SetPromptText(GameText.GAME_LOST_BOUNDS);

            PlaySound(lostClip);

            yield return new WaitForSeconds(4);

            EndGame();
        }

        private void EndGame()
        {
            GameState.Instance.EndState();
        }
        #endregion

        #region METHODS (boss)
        public void HandleBossInteraction()
		{
			if(bossBusy)
			{
				return;
			}
		
			PlayerController pc = player.GetComponent<PlayerController>();
		
			int carryScore = pc.CheckDebrisForScore(targetDebrisKind);

			if(carryScore <= 0)
			{
				ShowBossResponse(GameText.FOUND_NONE);
			}
			else
			{
				int newScore = GameState.Instance.score + carryScore;
				int maxScore = targetDebrisCount;

				if(newScore >= maxScore)
				{
					StartCoroutine(HandleWin());
				}
				else
				{
					// TODO - move to GameText class!
					string dTypeText = targetDebrisKind.ToString().ToUpper();
					string responseText = "You found ";
					if(carryScore == 1)
					{
						responseText += "a " + dTypeText + " box! ";
					}
					else
					{
						responseText += carryScore + " " + dTypeText + " boxes! ";
					}
					int pointsLeft = maxScore - newScore;					
					responseText += "Only " + pointsLeft + " more needed!";

					ShowBossResponse(responseText);

					UpdateScore(newScore, maxScore);
				}
			}

			pc.DissolveKids();

			StartCoroutine(DelayBossAvailability());
		}

		IEnumerator DelayBossAvailability()
		{
			bossBusy = true;

			yield return new WaitForSeconds(1);

			bossBusy = false;
		}

		private void ShowBossResponse(string text, int secs = 3)
		{
			StartCoroutine(FlashMessage(text, secs));			
		}
        #endregion

        #region METHODS (prompt)
        private void SetPromptText(string value = "")
		{
			promptController.SetText(value);
		}

		IEnumerator FlashMessage(string text, int duration)
		{
			SetPromptText(text);

			yield return new WaitForSeconds(duration);

			SetPromptText();
		}
        #endregion

        #region METHODS (scoreboard)	
        private void UpdateScore(int score, int max, string kindText = "")
		{
			if(score < 0 || score > max)
				return;

			scoreboardController.SetCountText(score.ToString() + " / " + max.ToString());

			GameState.Instance.score = score;

			if(kindText != "")
			{
				scoreboardController.SetColorText(kindText);
			}
		}
        #endregion

        #region METHODS (debris)	
        private Debris.Kind GetRandomDebrisKind()
        {
            string[] kinds = System.Enum.GetNames(typeof(Debris.Kind));

            return (Debris.Kind)Random.Range(0, kinds.Length);
        }

        private void CreateDebris(int totalCount, 
									int specialDebrisCount = -1, 
									Debris.Kind specialDebrisKind = Debris.Kind.Blue, 
									string parentName = "debris")
		{
			// init pool
			ObjectPooler.current.Init(totalCount, specialDebrisCount, specialDebrisKind);

			// create parent
			GameObject newParent = new GameObject(parentName);
			newParent.transform.parent = gameObject.transform;

			for(int i = 0; i < totalCount; i++)
			{
				GameObject obj = ObjectPooler.current.GetPooledObject();

				obj.transform.position = new Vector3(Random.Range(-posScale, posScale), 
													debrisHeight, 
													Random.Range(-posScale, posScale));

				obj.transform.parent = newParent.transform;

				obj.SetActive(true);
			}
		}
        #endregion

        #region METHODS (powerup)
        public void HandlePowerup()
		{
			StartCoroutine(ShowPowerupFound());
		}

		IEnumerator ShowPowerupFound()
		{
			powerupUIController.ShowView(true);

			powerupUIController.HandlePowerupGained(GameText.FOUND_POWERUP);

			yield return new WaitForSeconds(3);

			powerupUIController.ShowView(false);
		}
        #endregion

        #region METHODS (health)
        // HEALTH -----------------------------------		
        public void HandlePlayerHPChange(int newHP)
		{
            if(newHP <= 0)
			{
				StartCoroutine(EndGameFromEnemy());
			}
			else
			{
				GameState.Instance.hp = newHP;
			}
		}
        #endregion

        #region METHODS (bounds)
        public void HandleBoundsExceeded()
        {
            StartCoroutine(EndGameFromBoundsExceeded());
        }
        #endregion

        #region METHODS (audio)
        // TODO - move to SoundManager!
        // SOUND ------------------------------------				
        private void PlaySound(AudioClip clip)
        {
            audioSource.clip = clip;

            audioSource.Play();
        }
        #endregion
    }
}