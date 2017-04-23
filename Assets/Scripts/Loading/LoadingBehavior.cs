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
                LevelsLoader.loadIntro();
                break;
            case Action.StartSim:
                LevelsLoader.loadLevel1();
                break;
            case Action.Tutorials:
                LevelsLoader.loadTutorials();
                break;
            case Action.Controls:
                LevelsLoader.loadControls();
                break;
            case Action.unknown:
            default:
                break;
        }
    }
}
