using UnityEngine;
using System.Collections;
 
namespace Codebycandle.NCSoftDemo
{
    public class Sticky:MonoBehaviour
    {
        public bool isSticky;
        private bool isStuck;
        
        void OnTriggerEnter(Collider other)
        {
            if((other.gameObject.CompareTag(GameTag.TAG_PLAYER)) 
                && (isSticky == true) 
                && (isStuck == false))
            {  
                string otherTag = other.gameObject.tag;

                // TODO - refactor chain lookup!
                other.gameObject.GetComponent<PlayerController>().AttachThing(transform);

                isStuck = true;          

                return;
            }

            else if((other.gameObject.CompareTag(GameTag.TAG_PLAYER_KID)) 
                        && (isSticky == true) 
                        && (isStuck == false))
            {
                if(other.gameObject.GetComponentInParent<PlayerController>().megaMagnetActive)
                {
                    // TODO - refactor chain lookup!
                    other.gameObject.GetComponentInParent<PlayerController>().AttachThing(transform);

                    isStuck = true;      
                }

                return;    
            }
        }
    }
}