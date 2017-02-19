using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialCanvasBehavior : MonoBehaviour {

    public GameObject LayerFadeInOut;
    public float FadeInOutSpeed = 0;

    // Use this for initialization
    void Start () {
        LayerFadeInOut.GetComponent<Image>().color = Color.clear;
    }
	
    public void onClickGoBack(){
        LoadingBehavior.ActionToPerform = LoadingBehavior.Action.Intro;
        LoadingBehavior.Timer = 1;
        StartCoroutine(fadeInCoroutine());
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
                SceneManager.LoadScene("Loading");
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
