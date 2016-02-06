using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : Photon.PunBehaviour {
	public Text ScoreText;
	int score;

	void Awake(){
		GameObject.FindObjectOfType<PlayersList> ().AddPlayer(this);
	}

	public void AddScore(int score){
		SetScore (photonView.owner.GetScore () + score);
	}

	public void SetScore(int score){
		photonView.RPC ("SetScore_RPC", PhotonTargets.All, score);
	}

	[PunRPC]
	public void SetScore_RPC(int _score,PhotonMessageInfo info){
		if (score == _score || info.sender != photonView.owner)
			return;

		score = _score;
		photonView.owner.SetScore (_score);
		ScoreText.text = score + "";
	}

	public override void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps) {
		PhotonPlayer player = playerAndUpdatedProps[0] as PhotonPlayer;
		if(player == photonView.owner)
			AddScore (0); // FIXME Logic Refactor !!
	}
}
