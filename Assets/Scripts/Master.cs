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
	public static Master Instance;

	void Awake(){
		Instance = this;
		gamePlayMaster = GetComponent<GamePlayMaster> ();
		gamePlayMaster.OnColumsAndRowsCleared += delegate(int count) {
			AddScore(count);
		};
	}

	void Start () {
		gamePlayMaster.blocksContainer.PopulateBlocks ();
	}

	public void Reset(){
		Score = 0;
		gamePlayMaster.Reset ();
	}

	public void AddScore(int add){
		Score += add;
	}

	public void NoPlayAvailable(){
		Invoke ("Reset", 1);
	}
}
