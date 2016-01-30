using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GamePlayMaster : MonoBehaviour {
	public BoxGrid grid;
	public BlocksContainer blocksContainer;


	void Start () {
		grid.PopulateGrid ();
		blocksContainer.onDragEnded += CheckForDrag;

		grid.OnColumsAndRowsCleared += delegate(int count) {
			if (OnColumsAndRowsCleared != null)
				OnColumsAndRowsCleared (count);
		};
	}

	bool CheckForDrag(Draggable drag){
		BlockGroup group = drag.GetComponent<BlockGroup> ();

		if (group == null)
			return false;

		foreach(DropZone d in group.GetAnyBlock().draggable.dropZones){
			if(CanDropHere(group, d)){
				DropHere (group, d);

				blocksContainer.blocks.Remove (group);
				Destroy (drag.gameObject);

				DoneDragging ();
				return true;
			}
		}
		return false;
	}

	bool ValidateBlocks(){
		foreach (var block in blocksContainer.blocks) {
			if (HasPlace (block))
				return true;
		}
		return false;
	}

	bool HasPlace(BlockGroup group){
		for (int x = 0; x < grid.GetMap ().size.x; x++) {
			for (int y = 0; y < grid.GetMap ().size.y; y++) {
				if(CanDropHere(group, (BoxCell) grid.GetCell(new Coord(x,y))))
					return true;
			}
		}

		return false;
	}

	bool CanDropHere(BlockGroup group, DropZone dropenOn){
		BoxCell cellDropenOn = dropenOn.GetComponent<BoxCell>();
		return CanDropHere (group, cellDropenOn);
	}

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

	void DoneDragging(){
		grid.ClearSolvedRowsAndColumns ();
		if (blocksContainer.blocks.Count == 0)
			blocksContainer.PopulateBlocks ();
		else {
			if (ValidateBlocks () == false) {
				Master.Instance.NoPlayAvailable ();
			}
		}
	}

	public void Reset(){
		grid.ClearAll ();
		blocksContainer.PopulateBlocks ();
	}

	public System.Action<int> OnColumsAndRowsCleared;
}
