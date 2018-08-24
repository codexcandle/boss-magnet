using UnityEngine;
 
namespace Codebycandle.BossMagnet
{
    public class Sticky:MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            GameObject go = other.gameObject;
            if (go.CompareTag(GameTag.TAG_PLAYER) || go.CompareTag(GameTag.TAG_PLAYER_KID))
            {
                go.GetComponentInParent<PlayerController>().RequestAttachThing(go.tag, transform);
            }
        }
    }
}