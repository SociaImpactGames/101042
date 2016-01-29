using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {
	SpriteRenderer spR;
	BlockGroup group;
	public Draggable draggable { get; protected set;}

	void Awake(){
		spR = GetComponent<SpriteRenderer> ();
		draggable = GetComponent<Draggable> ();
	}

	public void SetGroup(BlockGroup _group){
		group = _group;
		spR.color = _group.color;
	}

	public Color GetColor(){
		return  group.color;
	}
}
