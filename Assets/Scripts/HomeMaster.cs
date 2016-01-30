using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class HomeMaster : MonoBehaviour {
	public void StartOnePlayerGame(){
		SceneManager.LoadScene ("Game_1Player");
	}
}
