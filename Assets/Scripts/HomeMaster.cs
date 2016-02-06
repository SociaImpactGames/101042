using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class HomeMaster : MonoBehaviour {
	public GameObject OneVSTwoUIPanel;
	public GameObject JoinCreateRoomUIPanel;
	public GameObject JoinRoomUIPanel;


	RoomNetworkMaster roomNetwork;
	public static HomeMaster Instance;

	void Awake(){
		Instance = this;
		ShowHomePanel ();
		roomNetwork = GetComponent<RoomNetworkMaster> ();
	}

	public void StartOnePlayerGame(){
		SceneManager.LoadScene ("Game_1Player");
	}

	public void StartTwoPlayersGame(string RoomName = ""){
		roomNetwork.OnJoinedRoomEvent -= GoToGame;
		roomNetwork.OnJoinedRoomEvent += GoToGame;
		if (RoomName == "") {
			roomNetwork.OnConnectedEvent = delegate() {
				roomNetwork.CreateRandomRoom ();
			};
		} else {
			roomNetwork.OnConnectedEvent = delegate() {
				roomNetwork.JoinRoom (RoomName);
			};
		}
		roomNetwork.ConnectToMaster ();
	}
		
	void GoToGame(){
		roomNetwork.StartScene ();
	}

	public void ShowHomePanel(){
		ShowOnly (OneVSTwoUIPanel);
	}

	public void ShowJoinCreateRoomPanel(){
		ShowOnly (JoinCreateRoomUIPanel);
	}

	public void ShowJoinRoomPanel(){
		ShowOnly (JoinRoomUIPanel);
	}

	void ShowOnly(GameObject panel){
		OneVSTwoUIPanel.SetActive (false);
		JoinCreateRoomUIPanel.SetActive (false);
		JoinRoomUIPanel.SetActive (false);

		panel.SetActive (true);
	}
}
