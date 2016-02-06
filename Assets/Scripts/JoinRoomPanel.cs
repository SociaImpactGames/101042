using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JoinRoomPanel : MonoBehaviour {
	public InputField RoomNumberField;
	string roomName = "";

	public void JoinRoom(){
		HomeMaster.Instance.StartTwoPlayersGame (roomName);
		Clear ();
	}

	public void Clear(){
		roomName = "";
		RoomNumberField.text = ""+ roomName;
	}

	public void AddChr(string c){
		if (roomName.Length < 4 && !(roomName.Length == 0 && c == "0"))
			roomName += c;

		RoomNumberField.text = "" + roomName;
	}
}
