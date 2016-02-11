using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoxGrid : CellsGrid {
	public override void Awake(){
		base.Awake();
		InstantiateCallBack += delegate(Cell obj) {
			BoxCell cell = obj as BoxCell;
			cell.OnResetColor += delegate() {
				if (OnCellCleared != null)
					OnCellCleared (cell.coord);
			};
		};
	}

	public void ClearSolvedRowsAndColumns(){
		List<int> ColumnsToClear = new List<int> ();
		List<int> RowsToClear = new List<int> ();

		for (int x = 0; x < GetMap ().size.x; x++) {
			bool full = true;

			for (int y = 0; y < GetMap ().size.y; y++) {
				if((Cells[x,y] as BoxCell).HasColor == false){
					full = false;
					break;
				}
			}
			if(full){
				ColumnsToClear.Add (x);
			}
		}

		for (int y = 0; y < GetMap ().size.y; y++) {
			bool full = true;

			for (int x = 0; x < GetMap ().size.x; x++) {
				if((Cells[x,y] as BoxCell).HasColor == false){
					full = false;
					break;
				}
			}
			if(full){
				RowsToClear.Add (y);
			}
		}

		if (OnColumsAndRowsCleared != null)
			OnColumsAndRowsCleared (ColumnsToClear.Count, RowsToClear.Count);

		foreach (int y in RowsToClear)
			ClearRow (y);

		foreach (int x in ColumnsToClear)
			ClearColumn (x);
	}

	void ClearRow(int y){
		for (int x = 0; x < GetMap ().size.x; x++) {
			(Cells [x, y] as BoxCell).ResetWithDelay(0.1f + x * 0.05f);
		}
	}

	void ClearColumn(int x){
		for (int y = 0; y < GetMap ().size.y; y++) {
			(Cells [x, y] as BoxCell).ResetWithDelay(0.1f + y * 0.05f);
		}
	}

	public void ClearAll(){
		for (int x = 0; x < GetMap ().size.x; x++) {
			ClearColumn (x);
		}
	}

	public System.Action<int, int> OnColumsAndRowsCleared;
	public System.Action<Coord> OnCellCleared;
}
