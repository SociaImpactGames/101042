using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TwoPlayersMaster : Photon.PunBehaviour {
	public PlayersList PlayerList;
	public GameObject StartPanel;

	GamePlayMaster gamePlayMaster;

	public Text TimerText;
	public int SessionTime = 10;
	int timer;

	void Awake(){
		gamePlayMaster = GetComponent<GamePlayMaster> ();

		gamePlayMaster.OnNoBlocksOnGround += delegate {
			NewBlockSet ();
		};

		gamePlayMaster.OnBlocksOnGroundWithNoDropMatch += NoPlayAvailable;

		gamePlayMaster.OnSuccessfulDrag += DropBlocksOnDropZone;
	}

	void Start(){
		PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity, 0);
	}
		
	public void StartGame(){
		if(PhotonNetwork.playerList.Length != 2)
			return;
		
		StartGameUI ();
		PlayerList.ResetScores();
		ClearBlocksForAllPlayers ();

		NewBlockSet ();
	}

	void NewBlockSet(){
		int[] indices = gamePlayMaster.blocksContainer.GetRandomBlockIndices ();
		photonView.RPC("PopulateBricksWithPrefaps_RPC", PhotonTargets.All, indices);
	}

	void AddScoreForTheCurrentPlayingPlayer(int ColCount, int RowCount){
		PhotonNetwork.player.AddScore (ScoreManager.GetTotalScore(ColCount, RowCount));

	}

	[PunRPC]
	public void PopulateBricksWithPrefaps_RPC(int[] indices, PhotonMessageInfo info){
		gamePlayMaster.blocksContainer.PopulateBlocks (indices);

		if (info.sender == PhotonNetwork.player) {
			gamePlayMaster.EnableDragging (false);
			gamePlayMaster.OnColumsAndRowsCleared -= AddScoreForTheCurrentPlayingPlayer;

			StopTimer ();
		} else {
			gamePlayMaster.EnableDragging (true);
			gamePlayMaster.OnColumsAndRowsCleared += AddScoreForTheCurrentPlayingPlayer;

			StartTimer ();
		}
	}

	public void NoPlayAvailable(){
		Invoke ("GameOver", 1);
	}

	void GameOver(){
		photonView.RPC ("GameOver_RPC", PhotonTargets.All);
	}

	[PunRPC]
	void GameOver_RPC(){
		if (OnGameOver != null)
			OnGameOver ();
	}

	public void ClearBlocksForAllPlayers(){
		photonView.RPC ("ClearBlocks_RPC", PhotonTargets.All);
	}

	[PunRPC]
	public void ClearBlocks_RPC(){
		gamePlayMaster.ClearBlocks ();
	}

	void StartGameUI(){
		photonView.RPC ("StartGameUI_RPC", PhotonTargets.All);
	}

	[PunRPC]
	void StartGameUI_RPC(){
		StartPanel.SetActive (false);
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

	public void QuitSession(){
		PhotonNetwork.Disconnect ();
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Home");
	}

	void StartTimer(){
		timer = SessionTime;
		TimerText.text = timer + "";
		Invoke ("TimerTick", 1);
	}

	void StopTimer(){
		CancelInvoke ("TimerTick");
		TimerText.text = "";
	}

	void TimerTick(){
		timer--;
		TimerText.text = timer + "";

		if (OnTimerTick != null)
			OnTimerTick (timer);

		if (timer == 0)
			NewBlockSet ();
		else
			Invoke ("TimerTick", 1);
	}

	public event System.Action OnGameOver;
	public event System.Action<int> OnTimerTick;
}
