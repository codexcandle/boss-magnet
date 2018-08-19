using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapViewController:MonoBehaviour
{
	public void ShowView(bool value)
	{
		gameObject.SetActive(value);
	}
}