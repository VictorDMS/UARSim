using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingBehavior : MonoBehaviour {

    public enum Action { Intro, StartSim, Tutorials, Controls, unknown };
    public static Action ActionToPerform = Action.unknown;
    public static int Timer = 0;
    public GameObject LayerFadeInOut;
    public float FadeInOutSpeed = 0;

    // Use this for initialization
    void Start () {
        LayerFadeInOut.GetComponent<Image>().color = Color.clear;
        StartCoroutine(loadingSceneTimer());
	}
	
    IEnumerator loadingSceneTimer(){
        yield return new WaitForSeconds(Timer);
        StartCoroutine(Fade.fadeInCoroutine(LayerFadeInOut, FadeInOutSpeed, performAction));
    }

    void performAction()
    {
        switch (ActionToPerform)
        {
            case Action.Intro:
                GameManager.loadIntro();
                break;
            case Action.StartSim:
                GameManager.loadLevel();
                break;
            case Action.Tutorials:
                GameManager.loadTutorials();
                break;
            case Action.Controls:
                GameManager.loadControls();
                break;
            case Action.unknown:
            default:
                break;
        }
    }
}
