using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoxGrid : CellsGrid {
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

		foreach (int y in RowsToClear)
			ClearRow (y);

		foreach (int x in ColumnsToClear)
			ClearColumn (x);
	}

	void ClearRow(int y){
		for (int x = 0; x < GetMap ().size.x; x++) {
			(Cells [x, y] as BoxCell).Reset (x * 0.05f);
		}
	}

	void ClearColumn(int x){
		for (int y = 0; y < GetMap ().size.y; y++) {
			(Cells [x, y] as BoxCell).Reset (y * 0.05f);
		}
	}

	public void ClearAll(){
		for (int x = 0; x < GetMap ().size.x; x++) {
			ClearColumn (x);
		}
	}
}
