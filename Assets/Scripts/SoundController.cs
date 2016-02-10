using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {
	public AudioClip SuccessfulDrag;
	public AudioClip FailDrag;
	public AudioClip ClearCell;

	public BoxGrid grid;
	public GamePlayMaster gamePlay;

	bool isPlaying;
	protected virtual void Awake () {
		gamePlay.OnSuccessfulDrop += delegate {
			PlayClip (SuccessfulDrag, true);
		};

		grid.OnCellCleared += delegate {
			PlayClip (ClearCell);
		};

		gamePlay.OnFailDrag += delegate {
			PlayClip (FailDrag);
		};
	}
	
	protected void PlayClip (AudioClip clip, bool importantSound = false) {
		if (clip == null || ( isPlaying && importantSound == false))
			return;
	
		AudioSource.PlayClipAtPoint (clip, Camera.main.transform.position);
		isPlaying = true;
		Invoke ("Done", clip.length);
	}

	void Done(){
		isPlaying = false;
	}
}
