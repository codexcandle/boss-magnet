using UnityEngine;
 
namespace Codebycandle.BossMagnet
{
    public class Sticky:MonoBehaviour
    {
        public bool isSticky;
        private bool isStuck;
        
        void OnTriggerEnter(Collider other)
        {
            // if collided with ROOT player object, attach to player
            if((other.gameObject.CompareTag(GameTag.TAG_PLAYER)) 
                && (isSticky == true) 
                && (isStuck == false))
            {
                // TODO - refactor chain lookup!
                string otherTag = other.gameObject.tag;
                other.gameObject.GetComponent<PlayerController>().AttachThing(transform);

                isStuck = true;          

                return;
            }

            // if collided with CHILD player object, AND "megaMagnetMode = enabled", attach to player!
            else if((other.gameObject.CompareTag(GameTag.TAG_PLAYER_KID)) 
                        && (isSticky == true) 
                        && (isStuck == false))
            {
                // TODO - refactor chain lookup!
                if (other.gameObject.GetComponentInParent<PlayerController>().megaMagnetActive)
                {
                    other.gameObject.GetComponentInParent<PlayerController>().AttachThing(transform);

                    isStuck = true;      
                }

                return;    
            }
        }
    }
}