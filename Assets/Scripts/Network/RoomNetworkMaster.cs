using UnityEngine;
using System.Collections;

public class RoomNetworkMaster : Photon.PunBehaviour {
	public void Connect(){
		PhotonNetwork.automaticallySyncScene = true;
		PhotonNetwork.ConnectUsingSettings ("1");
	}

	public override void OnJoinedLobby(){
		RoomOptions ro = new RoomOptions (){isVisible = true, maxPlayers = 2};
		PhotonNetwork.JoinOrCreateRoom ("Random!", ro, TypedLobby.Default);
	}

	public override void OnJoinedRoom(){
		if (PhotonNetwork.isMasterClient &&  OnJoinedRoomEvent != null)
			OnJoinedRoomEvent ();
	}

	public void StartScene(){
		PhotonNetwork.LoadLevel ("Game_2Players");
	}

	public System.Action OnJoinedRoomEvent;
}
