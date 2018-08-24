using UnityEngine;

namespace Codebycandle.BossMagnet
{
    public class EnemyAttack:MonoBehaviour
    {
        public float timeBetweenAttacks     = 0.5f;
        public int attackDamage             = 10;

        // TODO - elim ref with action delegate!
        private PlayerController playerController;
        private GameObject player;

        private Animator anim;
        private bool playerInRange;
        private float attackTimer;

        void Awake()
        {
            player = GameObject.FindGameObjectWithTag(GameTag.TAG_PLAYER);
            playerController = player.GetComponent<PlayerController>();

            anim = GetComponent<Animator>();
        }

        void Update()
        {
            attackTimer += Time.deltaTime;

            if(attackTimer >= timeBetweenAttacks && playerInRange)
            {
                Attack();
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if(other.gameObject == player)
            {
                playerInRange = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if(other.gameObject == player)
            {
                playerInRange = false;
            }
        }

        private void Attack()
        {
            attackTimer = 0f;

            playerController.TakeDamage(attackDamage);
        }
    }
}