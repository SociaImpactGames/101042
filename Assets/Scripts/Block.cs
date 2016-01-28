using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {
	public Color color { get; protected set;}

	SpriteRenderer spR;
	Draggable draggable;
	void Awake(){
		spR = GetComponent<SpriteRenderer> ();
		draggable = GetComponent<Draggable> ();
	}

	void Start(){
		color = BoxColor.RandomColor ();
		spR.color = color;
		transform.localScale = new Vector3 (0.5f,0.5f,0.5f);


		draggable.onDragStarted += (self) => {
			transform.localScale = new Vector3 (1, 1, 1);
		};	

		draggable.onReset += (self)  => {
			transform.localScale = new Vector3 (0.5f,0.5f,0.5f);
		};
	}
}
