using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Master : MonoBehaviour {
	public Text scoreText;
	int score;
	int Score {
		get { return score; }
		set { score = value; scoreText.text = Score + "";}
	}

	GamePlayMaster gamePlayMaster;

	void Awake(){
		gamePlayMaster = GetComponent<GamePlayMaster> ();
		gamePlayMaster.OnColumsAndRowsCleared += delegate(int ColCount, int RowCount) {
			AddScore(ScoreManager.GetTotalScore(ColCount, RowCount));
		};
	}

	void Start () {
		NewBlockSet ();

		gamePlayMaster.OnSuccessfulDrag += delegate(BlockGroup g, DropZone d) {
			gamePlayMaster.DropHere(g, d, false);
		};

		gamePlayMaster.OnNoBlocksOnGround += delegate {
			NewBlockSet();
		};

		gamePlayMaster.OnBlocksOnGroundWithNoDropMatch += delegate {
			NoPlayAvailable ();
		};
	}

	void NewBlockSet(){
		gamePlayMaster.blocksContainer.PopulateRandomBlocks ();
		gamePlayMaster.EnableDragging (true);
	}

	public void Reset(){
		Score = 0;
		gamePlayMaster.ClearBlocks ();
		gamePlayMaster.blocksContainer.PopulateRandomBlocks ();
	}

	public void AddScore(int add){
		Score += add;
	}

	public void NoPlayAvailable(){
		Invoke ("Reset", 1);
	}

	public void Home(){
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Home");
	}
}
