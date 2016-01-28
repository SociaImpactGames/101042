using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Master : MonoBehaviour {
	public CellsGrid grid;

	public Block[] blocks;

	void Start () {
		grid.PopulateGrid ();

		foreach (var block in blocks)
			block.GetComponent<Draggable> ().onDragEnded += CheckForDrag;
	}

	bool CheckForDrag(Draggable drag, DropZone dropedOn){
		if (dropedOn.Done == false) {
			Color c = drag.GetComponent<Block> ().color;
			dropedOn.GetComponent<BoxCell> ().SetColor (c);
			Destroy (drag.gameObject);

			DoneDragging ();
			return true;
		}
		return false;
	}

	void DoneDragging(){
		ClearSolvedRowsAndColumns ();
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
}
