using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Master : MonoBehaviour {
	GamePlayMaster gamePlayMaster;

	void Awake(){
		gamePlayMaster = GetComponent<GamePlayMaster> ();
	}

	void Start () {
		Reset ();
	}

	public void Reset(){
		gamePlayMaster.Reset ();
	}
}
