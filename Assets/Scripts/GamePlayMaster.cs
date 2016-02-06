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

	public void EnableDragging(bool _enable){
		blocksContainer.EnableDragging (_enable);
	}

	bool CheckForDrag(Draggable drag){
		BlockGroup group = drag.GetComponent<BlockGroup> ();

		if (group == null)
			return false;

		foreach(DropZone d in group.GetAnyBlock().draggable.dropZones){
			if(CanDropHere(group, d)){
				if (OnSuccessfulDrag != null) {
					OnSuccessfulDrag (group, d);
				} else {
					DropHere (group, d);
				}

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
	public void DropHere(BlockGroup group, DropZone dropenOn, bool WithAnimation = false){
		BoxCell cellDropenOn = dropenOn.GetComponent<BoxCell>();
		foreach (var position in group.positions) {
			(grid.GetCell (cellDropenOn.coord + position) as BoxCell).SetColor (group.color);
		}

		blocksContainer.blocks.Remove (group);
		Destroy (group.gameObject);

		grid.ClearSolvedRowsAndColumns ();
	}

	void DoneDragging(){
		if (blocksContainer.blocks.Count == 0){
			if (OnNoBlocksOnGround != null) {
				OnNoBlocksOnGround ();
			} else {
				blocksContainer.PopulateRandomBlocks ();
			}
		}
		else {
			if (ValidateBlocks () == false) {
				if(OnBlocksOnGroundWithNoDropMatch != null){
					OnBlocksOnGroundWithNoDropMatch ();
				}
			}
		}
	}

	public void ClearBlocks(){
		grid.ClearAll ();
	}

	public Coord CoordForDropZone(DropZone dropZone){
		return dropZone.GetComponent<BoxCell> ().coord;
	}

	public DropZone DropZoneForCoord(Coord coord){
		return grid.GetCell (coord).GetComponent<DropZone> ();
	}

	public System.Action<int> OnColumsAndRowsCleared;
	public System.Action<BlockGroup, DropZone> OnSuccessfulDrag;
	public System.Action OnNoBlocksOnGround;
	public System.Action OnBlocksOnGroundWithNoDropMatch;
}
