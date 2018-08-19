using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardController:MonoBehaviour
{
	[SerializeField]
	private Text colorText;

	[SerializeField]
	private Text countText;

	public void ShowView(bool value)
	{
		gameObject.SetActive(value);
	}

	public void SetColorText(string value = "")
	{
		colorText.text = (value != null) ? value.ToUpper() : "";
	}

	public void SetCountText(string value = "")
	{
		countText.text = value;
	}	

	void Start()
	{
		colorText.text = "";
	}
}