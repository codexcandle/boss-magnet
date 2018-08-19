using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris:MonoBehaviour
{
	public enum Kind
	{
		Blue,
		Purple,
		Red,
		Yellow,
	}
	public Kind kind;

	public static string GetRandomKind()
	{
		string[] names = System.Enum.GetNames(typeof(Kind));
		int count = names.Length;
		int randIndex = Random.Range(0, names.Length);

		return names[randIndex];
	}
}