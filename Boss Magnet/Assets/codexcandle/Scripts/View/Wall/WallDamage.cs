using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDamage:MonoBehaviour
{
	[SerializeField]
	private Material damageMaterial1;

	[SerializeField]
	private Material damageMaterial2;	

	private int hp = 3;

	private Renderer rend;

	void Awake()
	{
		rend = GetComponent<Renderer>();
	}

	public void OnCollisionEnter(Collision col)
	{
		DamageWall();
	}

	private void DamageWall()
	{
		hp--;

		if(hp > 0)
		{
			rend.material = (hp == 2) ? damageMaterial1 : damageMaterial2;
		}
		else
		{
			DestroyWall();
		}
	}

	private void DestroyWall()
	{
		gameObject.SetActive(false);
				
		// TODO - trigger explosion!
	}
}