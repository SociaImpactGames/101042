using UnityEngine;
using System.Collections;

public class BoxCell : Cell {
	public bool HasColor { get; protected set;}
	SpriteRenderer spR;
	DropZone dropZone;

	Color BaseColor = Color.white;

	void Awake(){
		spR = GetComponent<SpriteRenderer> ();
		dropZone = GetComponent<DropZone> ();
	}

	public void SetColor(Color color){
		spR.color = color;
		HasColor = true;
		dropZone.Done = true;
	}

	public void Reset(){
		ResetWithDelay (0);
	}

	public void ResetWithDelay(float delay){
		Invoke ("ResetColor", delay);
		HasColor = false;
		dropZone.Done = false;

		if (OnReset != null)
			OnReset ();
	}

	void ResetColor(){
		if(spR.color == BaseColor)
			return;

		if (OnResetColor != null)
			OnResetColor ();
		
		spR.color = BaseColor;
	}

	public event System.Action OnResetColor;
	public event System.Action OnReset;
}
