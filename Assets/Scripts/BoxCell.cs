using UnityEngine;
using System.Collections;

public class BoxCell : Cell {
	SpriteRenderer spR;
	void Awake(){
		spR = GetComponent<SpriteRenderer> ();
	}

	void Start(){
		spR.color = BoxColor.RandomColor ();
	}
}
