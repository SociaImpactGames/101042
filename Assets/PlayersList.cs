using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayersList : MonoBehaviour {
	public List<Player> players;

	public void AddPlayer(Player player){
		player.transform.SetParent(transform, false);
		players.Add (player);
	}

	public void ResetScores(){
		foreach (var player in players) {
			player.SetScore (0);
		}
	}
}
