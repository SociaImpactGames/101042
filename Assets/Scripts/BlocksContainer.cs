using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlocksContainer : MonoBehaviour {
	public Transform[] Positions;
	public GameObject[] Prefaps;

	public List<BlockGroup> blocks { get; protected set;}

	void Awake () {
		blocks = new List<BlockGroup> ();
	}
	
	public void PopulateBlocks(){
		foreach (var block in blocks)
			Destroy (block.gameObject);
		blocks.Clear ();

		for (int i = 0; i < Positions.Length; i++) {
			blocks.Add ((Instantiate (Prefaps [Random.Range (0, Prefaps.Length)], Positions [i].position, Quaternion.identity) as GameObject).GetComponent<BlockGroup> ());
		}

		foreach (var block in blocks)
			block.GetComponent<Draggable> ().onDragEnded += CheckForDrag;
	}

	public delegate bool OnDragEnded(Draggable self);
	public OnDragEnded onDragEnded;

	bool CheckForDrag(Draggable drag, DropZone dropedOn){
		if (onDragEnded != null)
			return onDragEnded (drag);

		return false;
	}
}
