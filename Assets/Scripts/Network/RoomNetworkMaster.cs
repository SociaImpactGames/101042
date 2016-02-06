using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RoomNetworkMaster : Photon.PunBehaviour {
	public Text RoomConnectionLog;

	public void ConnectToMaster(){
		if (PhotonNetwork.connected) {
			if (OnConnectedEvent != null)
				OnConnectedEvent ();
			return;
		}

		PhotonNetwork.autoJoinLobby = true;
		PhotonNetwork.automaticallySyncScene = true;
		PhotonNetwork.ConnectUsingSettings ("1");
	}

	public override void OnJoinedLobby(){
		if (OnConnectedEvent != null)
			OnConnectedEvent ();
	}

	public void CreateRandomRoom(){
		string roomName = "";
		List<string> roomNames = GetRoomsNames ();

		while(roomName == "" || roomNames.Contains(roomName)){
			roomName = Random.Range (1000,10000) + "";
		}

		RoomOptions ro = new RoomOptions (){isVisible = true, maxPlayers = 2};
		PhotonNetwork.CreateRoom (roomName, ro, TypedLobby.Default);
	}

	public void JoinRoom(string RoomName){
		Debug.Log ("1");
		RoomConnectionLog.text = "";
		PhotonNetwork.JoinRoom (RoomName);
	}

	public override void OnJoinedRoom(){
		if (PhotonNetwork.isMasterClient &&  OnJoinedRoomEvent != null)
			OnJoinedRoomEvent ();
	}

	public override void OnPhotonJoinRoomFailed(object[] codeAndMsg){
		RoomConnectionLog.text = codeAndMsg[1] as string;
	}

	public void StartScene(){
		PhotonNetwork.LoadLevel ("Game_2Players");
	}

	public System.Action OnJoinedRoomEvent;
	public System.Action OnConnectedEvent;

	public static List<string> GetRoomsNames(){
		List<string> roomNames = new List<string> ();
		foreach (var room in PhotonNetwork.GetRoomList()) {
			roomNames.Add (room.name);
		}
		return roomNames;
	}
}
