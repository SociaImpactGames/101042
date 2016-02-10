using UnityEngine;
using System.Collections;

public class TwoPlayersSoundController : SoundController {
	public TwoPlayersMaster master;

	public AudioClip TimerWarning;
	public AudioClip GameOver;

	protected override void Awake ()
	{
		base.Awake ();
		master.OnTimerTick += delegate(int countDown) {
			if( countDown <= 5 && countDown > 0 )
				PlayClip(TimerWarning, true);
		};

		master.OnGameOver += delegate() {
			PlayClip(GameOver, true);
		};
	}
}
