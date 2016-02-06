using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : Photon.PunBehaviour {
	public Text ScoreText;

	void Awake(){
		GameObject.FindObjectOfType<PlayersList> ().AddPlayer(this);
	}

	public void AddScore(int score){
		SetScore (photonView.owner.GetScore () + score);
	}

	public void SetScore(int score){
		photonView.RPC ("SetScore", PhotonTargets.All, score);
	}

	[PunRPC]
	public void SetScore_RPC(int score){
		photonView.owner.SetScore (score);
		ScoreText.text = score + "";
	}

	public override void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps) {
		PhotonPlayer player = playerAndUpdatedProps[0] as PhotonPlayer;
		Hashtable props = playerAndUpdatedProps[1] as Hashtable;

		Debug.Log (props [PunPlayerScores.PlayerScoreProp]);
	}
}
