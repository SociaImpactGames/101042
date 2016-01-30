using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class HomeMaster : MonoBehaviour {
	RoomNetworkMaster roomNetwork;

	void Awake(){
		roomNetwork = GetComponent<RoomNetworkMaster> ();
	}

	public void StartOnePlayerGame(){
		SceneManager.LoadScene ("Game_1Player");
	}

	public void StartTwoPlayersGame(){
		roomNetwork.OnJoinedRoomEvent += GoToGame;
		roomNetwork.Connect ();
	}
		
	void GoToGame(){
		roomNetwork.StartScene ();
	}
}
