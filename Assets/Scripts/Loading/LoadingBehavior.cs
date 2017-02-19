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
	
    IEnumerator loadingSceneTimer()
    {
        yield return new WaitForSeconds(Timer);
        StartCoroutine(fadeInCoroutine());
    }

    void performAction()
    {
        switch (ActionToPerform)
        {
            case Action.Intro:
                SceneManager.LoadScene("Intro");
                break;
            case Action.StartSim:
                SceneManager.LoadScene("Level1_Maze");
                break;
            case Action.Tutorials:
                SceneManager.LoadScene("Tutorials");
                break;
            case Action.Controls:
                SceneManager.LoadScene("Controls");
                break;
            case Action.unknown:
            default:
                break;
        }
    }

    IEnumerator fadeInCoroutine()
    {
        LayerFadeInOut.SetActive(true);
        Image ImageFadeInOut = LayerFadeInOut.GetComponent<Image>();
        for (;;)
        {
            ImageFadeInOut.color = Color.Lerp(ImageFadeInOut.color, Color.black, (FadeInOutSpeed / 2) * Time.deltaTime);
            if (ImageFadeInOut.color.a >= 0.95f)
            {
                ImageFadeInOut.color = Color.black;
                performAction();
                break;
            }
            else
            {
                yield return null;
            }
        }
        print("Process \"fade in\" is done.");
    }
}
