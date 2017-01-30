using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntroGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void onClickStartSimButton(){
		SceneManager.LoadScene ("Level1_Maze");
	}

	public void onClickTutorialButton(){
	}

	public void onClickControlsButton(){
	}
}
