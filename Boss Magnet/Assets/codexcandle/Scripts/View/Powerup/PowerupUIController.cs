using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class PowerupUIController:MonoBehaviour
{
	private AudioSource audioSource;	

	private Text tf;

	public void ShowView(bool value)
	{
		gameObject.SetActive(value);
	}

	public void HandlePowerupGained(string message)
	{
		audioSource.Play();

		tf.text = message;
	}

	void Awake()
	{
		tf = GetComponentInChildren<Text>();

		audioSource = GetComponent<AudioSource>();
	}
}