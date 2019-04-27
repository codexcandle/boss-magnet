using UnityEngine;

namespace Codebycandle.BossMagnet
{
	public class Powerup:MonoBehaviour
	{
		void OnTriggerEnter(Collider col)
		{
			if(col.gameObject.CompareTag(GameTag.TAG_PLAYER))
			{
				col.gameObject.GetComponent<PlayerController>().CollectPowerup();

				Destroy(gameObject);
			}

			else if(col.gameObject.CompareTag(GameTag.TAG_PLAYER_KID))
			{
				col.gameObject.GetComponentInParent<PlayerController>().CollectPowerup();

				Destroy(gameObject);
			}		
		}
	}
}