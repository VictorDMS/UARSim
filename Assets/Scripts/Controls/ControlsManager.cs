using UnityEngine;
using UnityEngine.UI;

public class ControlsManager : MonoBehaviour {
    public GameObject LayerFadeInOut;
    public float FadeInOutSpeed = 0;

    // Use this for initialization
    void Start () {
        LayerFadeInOut.GetComponent<Image>().color = Color.clear;
    }
	
    public void onClickGoBack(){
        LoadingBehavior.ActionToPerform = LoadingBehavior.Action.Intro;
        LoadingBehavior.Timer = 1;
        StartCoroutine(Fade.fadeInCoroutine(LayerFadeInOut, FadeInOutSpeed, loadLevel));
    }

    void loadLevel(){
        GameManager.loadLoading();
    }
}