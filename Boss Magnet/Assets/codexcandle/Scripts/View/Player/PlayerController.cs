using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Codebycandle.NCSoftDemo
{
	[RequireComponent(typeof(HealthMeterController))]
    [RequireComponent(typeof(AudioSource))]
    public class PlayerController:MonoBehaviour
    {
        // TODO - implement abstract class interface!
        // TODO - elim ref w/ aciton delegates!
        [SerializeField] 
        public LevelLogic levelLogic;

		// HEALTH -----------------------------------
        public Image damageImage;                                   
        public float flashSpeed = 5f;                               
        public Color flashColour = new Color(1f, 0f, 0f, 0.1f);           
		[SerializeField]		
		private HealthMeterController healthMeterController;
        private bool isDead;
        private bool damaged;
        private int maxHP;
        private int curHP; 

 		// SOUND ------------------------------------
        private AudioSource audioSource;
        public AudioClip stuckClip;
        public AudioClip ouchClip;

 		// MOVEMENT ---------------------------------   
        private bool interactive;
        public float speed;

  		// COLLISION --------------------------------
        public Transform debrisTrunk;
        public bool megaMagnetActive;
        [SerializeField]
        private Rigidbody playerModelRB;
        [SerializeField]
        private ParticleSystem damagePS;
        [SerializeField]
        private ParticleSystem powerupPS;        
        private bool busyDelaying;

 		// MOVEMENT ---------------------------------         
        void FixedUpdate()
        {
            if(interactive)
            {
                float moveHorizontal = Input.GetAxis("Horizontal");
                float moveVertical = Input.GetAxis("Vertical");

                Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

                playerModelRB.AddForce(movement * speed);
            }
        }

        public void EnableMovement(bool value)
        {
            interactive = value;
        }

 		// HEALTH -----------------------------------
        void Start()
        {
            curHP = maxHP = 100;
        }

        void Update()
        {
            if(damaged)
            {
                // flash "damage" color
                damageImage.color = flashColour;
            }
            else
            {
                // transition color back to clear
                damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
            }

            damaged = false;
        }         
              
        public void TakeDamage(int amount)
        {
            if(interactive == false)
                return;

            damaged = true;

            curHP -= amount;

            healthMeterController.SufferDamage();

            PlaySound(ouchClip);

            ExpelKids();

            if(curHP == 0)
            {
                interactive = false;
            }

            levelLogic.HandlePlayerHPChange(curHP);            
        }

        public void ShowHealthMeter(bool show)
        {
            healthMeterController.ShowView(show);
        }

		// SCORE ------------------------------------        
        public int CheckDebrisForScore(Debris.Kind targetDebrisKind)
        {
            int matchCount = 0;
            foreach(Transform trans in debrisTrunk)
            {
                if(trans.gameObject.activeSelf == false)
                    break;

                if(trans.gameObject.CompareTag(GameTag.TAG_PLAYER_KID))
                {
                    Debris d = trans.gameObject.GetComponent<Debris>();
                    if(d)
                    {
                        if(d.kind == targetDebrisKind)
                        {
                            matchCount++;
                        }
                    }
                }
            }

            return matchCount;
        }

 		// SOUND ------------------------------------
        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlayAttachSound()
        {
            PlaySound(stuckClip);
        }

		// TODO - move to SoundManager!        
        private void PlaySound(AudioClip clip)
        {
            audioSource.clip = clip;
            
            audioSource.Play ();
        }   

  		// POWERUP ----------------------------------    
        public void CollectPowerup()
        {
            levelLogic.HandlePowerup();

            powerupPS.Play();

            megaMagnetActive = true;
        }

  		// COLLISION --------------------------------
        void OnCollisionEnter(Collision info)
        {
            if(interactive == false)
                return;

            switch(info.gameObject.tag)
            {
                case GameTag.TAG_DEBRIS:
                    if(!megaMagnetActive)
                        return;  
                    break;
                case GameTag.TAG_PLAYER_KID:
                    if(!megaMagnetActive)
                        return;  
                    break;
                case GameTag.TAG_BOSS:
                    levelLogic.HandleBossInteraction();
                    return;
                case GameTag.TAG_BOUNDS:
                    levelLogic.HandleBoundsExceeded();
                    return;
                default:
                    return;
            }

            if(busyDelaying)
            {
                return;
            }

            bool hitMainCollider = false;
        
            // only attach if hit main sphere
            foreach(ContactPoint contact in info.contacts)
            {
                Collider col = GetComponent<Collider>();
                if (contact.thisCollider == col)
                {
                    hitMainCollider = true;
                    
                    break;
                }
            }
       
            if(hitMainCollider)
            {
                AttachFromCollision(info);
            }
        }          

        public void AttachThing(Transform trans)
        {
            trans.parent = debrisTrunk.transform;
            
            trans.gameObject.tag = GameTag.TAG_PLAYER_KID;

            SetLayerForMinimapKids(trans, false);

            Destroy(trans.GetComponent<Rigidbody>());

            PlayAttachSound();        
        }

        public void AttachFromCollision(Collision col)
        {
            if(!megaMagnetActive)
                return;    

            Destroy(col.rigidbody);

            col.transform.parent = debrisTrunk;

            col.transform.tag = GameTag.TAG_PLAYER_KID;

            SetLayerForMinimapKids(col.transform, false);

            PlayAttachSound();
        }

        public void ExpelKids()
        {
            damagePS.Play();

            playerModelRB.useGravity = false;
            playerModelRB.isKinematic = true;
            playerModelRB.detectCollisions = false;

            List<Transform> playerKids = new List<Transform>();

            foreach(Transform trans in debrisTrunk)
            {
                if(trans.CompareTag(GameTag.TAG_PLAYER_KID))
                {
                    playerKids.Add(trans);             
                }
            }

            debrisTrunk.DetachChildren();

            // now, let's loop through "playerKids", & apply repulsion force!
            foreach(Transform trans2 in playerKids)
            {
                trans2.tag = GameTag.TAG_DEBRIS;

                // TODO - replace int literal w/ constant!
                SetLayerForMinimapKids(trans2, true);

                trans2.gameObject.AddComponent<Rigidbody>();
                
                Rigidbody rb = trans2.gameObject.GetComponent<Rigidbody>();
                rb.mass = 0.1F;
                rb.AddForce(Vector3.up * 100.0F, ForceMode.Acceleration);   

                trans2.parent = null; 
            }

            playerModelRB.useGravity = true;

            playerModelRB.isKinematic = false;
            playerModelRB.detectCollisions = true;

            if(busyDelaying == false)
            {
                StartCoroutine(DelayActivateMagnet());
            }
        }

        public void DissolveKids()
        {
            if(debrisTrunk != null)
            {
                foreach(Transform trans in debrisTrunk)
                {
                    // TODO - confirm which proves better for perf...
                    Destroy(trans.gameObject);
                    // trans.gameObject.SetActive(false);
                }
            }
        }

        private void SetLayerForMinimapKids(Transform t, bool show)
        {
            foreach(Transform trans in t)
            {
                if(trans.gameObject.CompareTag(GameTag.TAG_MINIMAP))
                {
                    // TODO - replace literals!
                    // layer 8 = Minimap
                    // layer 11 = Minimap_hidden
                    trans.gameObject.layer = show ? 8 : 11; 
                }
            }
        }

        IEnumerator DelayActivateMagnet()
        {
            busyDelaying = true;

            yield return new WaitForSeconds(1.0F);

            busyDelaying = false;
        }                                     
    }
}