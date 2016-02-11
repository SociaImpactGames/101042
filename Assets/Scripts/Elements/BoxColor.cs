using UnityEngine;
using System.Collections;

public class BoxColor {
	public static readonly Color Green = new Color (39f/255, 174f/255, 96f/255, 1.0f);
	public static readonly Color Orange = new Color (230f/255, 126f/255, 34f/255, 1.0f);
	public static readonly Color Blue = new Color (41f/255, 128f/255, 185f/255, 1.0f);

	static Color[] Colors = new Color[]{Green, Orange, Blue};

	public static Color RandomColor(){
		return Colors[Random.Range(0, Colors.Length)];
	}
}
