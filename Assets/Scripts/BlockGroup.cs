using UnityEngine;
using System.Collections;

public class BlockGroup : MonoBehaviour {
	public Coord[] positions;
	public Color color { get; protected set;}

	public GameObject BlockPrefap;

	Draggable draggable;

	public Block[] blocks { get; protected set;}

	void Awake(){
		draggable = GetComponent<Draggable> ();
		color = BoxColor.RandomColor ();
		blocks = new Block[positions.Length];
	}

	void Start(){
		int maxX = 0;
		int maxY = 0;

		for (int i = 0; i < positions.Length; i++) {
			Coord position = positions [i];
			GameObject go = Instantiate (BlockPrefap) as GameObject;
			go.transform.parent = transform;
			go.transform.localPosition = new Vector3(position.x, position.y, 1);
			blocks [i] = go.GetComponent<Block> ();
			blocks [i].SetGroup (this);

			if (position.x > maxX)
				maxX = position.x;

			if (position.y > maxY)
				maxY = position.y;
		}

		transform.localScale = new Vector3 (0.5f,0.5f,0.5f);

		draggable.onDragStarted += (self) => {
			transform.localScale = new Vector3 (1, 1, 1);
		};	

		draggable.onReset += (self)  => {
			transform.localScale = new Vector3 (0.5f,0.5f,0.5f);
		};


		GetComponent<BoxCollider2D> ().offset = new Vector2 (maxX * 0.5f, maxY * 0.5f);
		maxX++;
		maxY++;
		GetComponent<BoxCollider2D> ().size = Vector2.one * 4;//new Vector2 (maxX, maxY);
	}

	public Block GetAnyBlock(){
		if (blocks.Length > 0)
			return blocks [0];
		return null;
	}

	public void EnableDragging(bool _enable){
		draggable.IsDraggable = _enable;
	}
}
