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

	bool ValidateBlocks(){
		foreach (var block in blocks) {
			if (HasPlace (block))
				return true;
		}
		return false;
	}

	//TODO: Move From Here
	bool HasPlace(BlockGroup group){
		for (int x = 0; x < grid.GetMap ().size.x; x++) {
			for (int y = 0; y < grid.GetMap ().size.y; y++) {
				if(CanDropHere(group, (BoxCell) grid.GetCell(new Coord(x,y))))
					return true;
			}
		}

		return false;
	}

	//TODO: Move From Here
	bool CanDropHere(BlockGroup group, DropZone dropenOn){
		BoxCell cellDropenOn = dropenOn.GetComponent<BoxCell>();
		return CanDropHere (group, cellDropenOn);
	}

	//TODO: Move From Here
	bool CanDropHere(BlockGroup group, BoxCell cell){
		foreach (var position in group.positions) {
			BoxCell boxCell = ( grid.GetCell (cell.coord + position) as BoxCell );

			if (boxCell == null || boxCell.HasColor)
				return false;
		}
		return true;
	}

	// Refactor
	void DropHere(BlockGroup group, DropZone dropenOn){
		BoxCell cellDropenOn = dropenOn.GetComponent<BoxCell>();
		foreach (var position in group.positions) {
			(grid.GetCell (cellDropenOn.coord + position) as BoxCell).SetColor (group.color);
		}
	}

	//TODO: Move it from here to Grid !
	#region GamePlayedCheck
	void DoneDragging(){
		ClearSolvedRowsAndColumns ();
		if (blocks.Count == 0)
			PopulateBlocks ();
		else {
			if (ValidateBlocks () == false) {
				Debug.Log ("Game Over");
			}
		}
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
		for (int x = 0; x < grid.GetMap ().size.x; x++) {
			(grid.Cells [x, y] as BoxCell).Reset (x * 0.05f);
		}
	}

	void ClearColumn(int x){
		for (int y = 0; y < grid.GetMap ().size.y; y++) {
			(grid.Cells [x, y] as BoxCell).Reset (y * 0.05f);
		}
	}
	#endregion GamePlayedCheck
}
