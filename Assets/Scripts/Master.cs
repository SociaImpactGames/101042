using UnityEngine;
using System.Collections;

public class Master : MonoBehaviour {
	public CellsGrid grid;

	void Start () {
		grid.PopulateGrid ();
	}
}
