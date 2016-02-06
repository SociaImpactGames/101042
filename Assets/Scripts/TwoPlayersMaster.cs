using UnityEngine;
using System.Collections;

public class TwoPlayersMaster : Photon.PunBehaviour {
	public PlayersList PlayerList;
	public GameObject StartBtn;

	GamePlayMaster gamePlayMaster;

	void Awake(){
		gamePlayMaster = GetComponent<GamePlayMaster> ();

		gamePlayMaster.OnColumsAndRowsCleared += AddScoreForTheCurrentPlayingPlayer;

		gamePlayMaster.OnNoBlocksOnGround += delegate {
			NewBlockSet ();
		};

		gamePlayMaster.OnBlocksOnGroundWithNoDropMatch += NoPlayAvailable;

		gamePlayMaster.OnSuccessfulDrag += DropBlocksOnDropZone;
	}

	void Start(){
		//FIXME : Create Players
	}
		
	public void StartGame(){
		if(PhotonNetwork.playerList.Length != 2)
			return;
		
		HideStartGameBtn ();
		PlayerList.ResetScores();
		ClearBlocksForAllPlayers ();

		NewBlockSet ();
	}

	void NewBlockSet(){
		int[] indices = gamePlayMaster.blocksContainer.GetRandomBlockIndices ();
		photonView.RPC("PopulateBricksWithPrefaps_RPC", PhotonTargets.All, indices);
	}

	void AddScoreForTheCurrentPlayingPlayer(int count){
		// FIXME
	}

	[PunRPC]
	public void PopulateBricksWithPrefaps_RPC(int[] indices, PhotonMessageInfo info){
		gamePlayMaster.blocksContainer.PopulateBlocks (indices);

		if (info.sender == PhotonNetwork.player) {
			gamePlayMaster.EnableDragging (false);
		} else {
			gamePlayMaster.EnableDragging (true);
		}
	}

	public void NoPlayAvailable(){
		Invoke ("GameOver", 1);
	}

	void GameOver(){
		// FIXME
		// Show Score Panel
	}

	public void ClearBlocksForAllPlayers(){
		photonView.RPC ("ClearBlocks_RPC", PhotonTargets.All);
	}

	[PunRPC]
	public void ClearBlocks_RPC(){
		gamePlayMaster.ClearBlocks ();
	}

	void HideStartGameBtn(){
		photonView.RPC ("HideStartGameBtn_RPC", PhotonTargets.All);
	}

	[PunRPC]
	void HideStartGameBtn_RPC(){
		StartBtn.SetActive (false);
	}

	void DropBlocksOnDropZone(BlockGroup g, DropZone d){
		int gNumber = gamePlayMaster.blocksContainer.GetBlockGroupID(g);
		Coord cellCoord = gamePlayMaster.CoordForDropZone(d);
		photonView.RPC ("DropBlocksOnDropZone_RPC", PhotonTargets.All, gNumber, cellCoord.x, cellCoord.y);
	}

	[PunRPC]
	void DropBlocksOnDropZone_RPC(int gNumber, int dX, int dY, PhotonMessageInfo info){
		BlockGroup g = gamePlayMaster.blocksContainer.GetBlockGroupByID(gNumber);
		DropZone d = gamePlayMaster.DropZoneForCoord (new Coord(dX, dY));
		gamePlayMaster.DropHere (g, d, info.sender != PhotonNetwork.player);
	}
}
