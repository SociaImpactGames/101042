using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateRoomNumber : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Text> ().text = PhotonNetwork.room.name;
	}
}
