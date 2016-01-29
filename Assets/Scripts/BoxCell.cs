using UnityEngine;
using System.Collections;

public class BoxCell : Cell {
	public bool HasColor { get; protected set;}
	SpriteRenderer spR;
	DropZone dropZone;

	void Awake(){
		spR = GetComponent<SpriteRenderer> ();
		dropZone = GetComponent<DropZone> ();
	}

	public void SetColor(Color color){
		spR.color = color;
		HasColor = true;
		dropZone.Done = true;
	}

	public void Reset(float delay){
		Invoke ("ResetColor", delay + 0.1f);
		HasColor = false;
		dropZone.Done = false;
	}

	void ResetColor(){
		spR.color = Color.white;
	}
}
