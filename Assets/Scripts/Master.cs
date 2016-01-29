using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Master : MonoBehaviour {
	public CellsGrid grid;

	public Transform[] Positions;
	public GameObject[] Prefaps;

	List<BlockGroup> blocks;

	void Start () {
		grid.PopulateGrid ();

		blocks = new List<BlockGroup> ();
		PopulateBlocks ();
	}

	void PopulateBlocks(){
		for (int i = 0; i < Positions.Length; i++) {
			blocks.Add ((Instantiate (Prefaps [Random.Range (0, Prefaps.Length)], Positions [i].position, Quaternion.identity) as GameObject).GetComponent<BlockGroup> ());
		}

		foreach (var block in blocks)
			block.GetComponent<Draggable> ().onDragEnded += CheckForDrag;
	}

	bool CheckForDrag(Draggable drag, DropZone dropedOn){
		BlockGroup group = drag.GetComponent<BlockGroup> ();

		if (group == null)
			return false;

		foreach(DropZone d in group.GetAnyBlock().draggable.dropZones){
			if(CanDropHere(group, d)){
				DropHere (group, d);

				blocks.Remove (group);
				Destroy (drag.gameObject);

				DoneDragging ();
				return true;
			}
		}
		return false;
	}

	bool CanDropHere(BlockGroup group, DropZone dropenOn){
		BoxCell cellDropenOn = dropenOn.GetComponent<BoxCell>();
		foreach (var position in group.positions) {
			if ((grid.GetCell (cellDropenOn.coord + position) as BoxCell).HasColor)
				return false;
		}
		return true;
	}

	void DropHere(BlockGroup group, DropZone dropenOn){
		BoxCell cellDropenOn = dropenOn.GetComponent<BoxCell>();
		foreach (var position in group.positions) {
			(grid.GetCell (cellDropenOn.coord + position) as BoxCell).SetColor (group.color);
		}
	}

	#region GamePlayedCheck
	void DoneDragging(){
		ClearSolvedRowsAndColumns ();
		if (blocks.Count == 0)
			PopulateBlocks ();
	}

	void ClearSolvedRowsAndColumns(){
		List<int> ColumnsToClear = new List<int> ();
		List<int> RowsToClear = new List<int> ();

		for (int x = 0; x < grid.GetMap ().size.x; x++) {
			bool full = true;

			for (int y = 0; y < grid.GetMap ().size.y; y++) {
				if((grid.Cells[x,y] as BoxCell).HasColor == false){
					full = false;
					break;
				}
			}
			if(full){
				ColumnsToClear.Add (x);
			}
		}

		for (int y = 0; y < grid.GetMap ().size.y; y++) {
			bool full = true;

			for (int x = 0; x < grid.GetMap ().size.x; x++) {
				if((grid.Cells[x,y] as BoxCell).HasColor == false){
					full = false;
					break;
				}
			}
			if(full){
				RowsToClear.Add (y);
			}
		}

		foreach (int y in RowsToClear)
			ClearRow (y);

		foreach (int x in ColumnsToClear)
			ClearColumn (x);
	}

	void ClearRow(int y){
		StartCoroutine (ClearRowCoroutine (y));

	}

	void ClearColumn(int x){
		StartCoroutine (ClearColCoroutine (x));
	}

	IEnumerator ClearRowCoroutine(int y){
		for (int x = 0; x < grid.GetMap ().size.x; x++) {
			yield return new WaitForSeconds(0.05f);
			(grid.Cells [x, y] as BoxCell).Reset ();
		}
	}

	IEnumerator ClearColCoroutine(int x){
		for (int y = 0; y < grid.GetMap ().size.y; y++) {
			yield return new WaitForSeconds(0.05f);
			(grid.Cells [x, y] as BoxCell).Reset ();
		}
	}
	#endregion GamePlayedCheck
}
