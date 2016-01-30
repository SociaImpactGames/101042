using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashMaster : MonoBehaviour {
	void Start () {
		Invoke ("Home", 1.5f);
	}
	
	void Home(){
		SceneManager.LoadScene ("Home");
	}
}
