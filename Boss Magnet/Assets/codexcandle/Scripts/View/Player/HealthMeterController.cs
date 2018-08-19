using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthMeterController:MonoBehaviour
{
	private Transform[] units;

	private int activeHealthUnits;

	public void ShowView(bool value)
	{
		gameObject.SetActive(value);
	}

	public void SufferDamage()
	{
		if(activeHealthUnits > 0)
		{
			SetMeterLevel(activeHealthUnits - 1);
		}
	}

	void Start()
	{
		units = GetComponentsInChildren<Transform>();

		/*
		hack to sanitize! 
		(ignoring "root" gameObject gathered in above "units" init)
		*/
		activeHealthUnits = units.Length - 1;
	}

	private void SetMeterLevel(int showCount)
	{
		if(units == null)
			return;

		/* 
		again, hack to sanitize! 
		(ignoring "root" gameObject gathered in above "units" init)
		*/
		int count = units.Length;
		if(showCount < 1 || showCount > count)
			return;

		for(int i = 0; i < count; i++)
		{
			bool makeActive = (i <= showCount);

			units[i].gameObject.SetActive(makeActive);
		}

		activeHealthUnits = showCount;
	}
}