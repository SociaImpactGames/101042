using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlocksContainer : MonoBehaviour {
	public Transform[] Positions;
	public GameObject[] Prefabs;

	public List<BlockGroup> blocks { get; protected set;}

	void Awake () {
		blocks = new List<BlockGroup> ();
	}

	public void EnableDragging(bool _enable){
		foreach (var group in blocks) {
			group.EnableDragging (_enable);
		}
	}
	
	public void PopulateRandomBlocks(){
		PopulateBlocks (GetRandomBlockIndices());
	}

	public int[] GetRandomBlockIndices(){
		int[] indices = new int[Positions.Length];
		for (int i = 0; i < Positions.Length; i++) {
			indices[i] = Random.Range (0, Prefabs.Length);
		}
		return indices;
	}

	public void PopulateBlocks(int[] indices){
		foreach (var block in blocks)
			Destroy (block.gameObject);
		blocks.Clear ();

		for (int i = 0; i < Mathf.Min(Positions.Length, indices.Length); i++) {
			blocks.Add ((Instantiate (Prefabs [indices[i]], Positions [i].position, Quaternion.identity) as GameObject).GetComponent<BlockGroup> ());
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

	public int GetBlockGroupID(BlockGroup g){
		return blocks.IndexOf (g);
	}

	public BlockGroup GetBlockGroupByID(int id){
		return blocks [id];
	}
}
