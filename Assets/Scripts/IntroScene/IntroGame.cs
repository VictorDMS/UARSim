using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroGame : MonoBehaviour {

    public GameObject LayerFadeInOut;
    public float FadeInOutSpeed = 0;

    // Use this for initialization
    void Start () {
        LayerFadeInOut.GetComponent<Image>().color = Color.clear;
    }
	
	public void onClickStartSimButton(){
        LoadingBehavior.ActionToPerform = LoadingBehavior.Action.StartSim;
        LoadingBehavior.Timer = 3;
        StartCoroutine(fadeInCoroutine());
	}

	public void onClickTutorialButton(){
        LoadingBehavior.ActionToPerform = LoadingBehavior.Action.Tutorials;
        LoadingBehavior.Timer = 1;
        StartCoroutine(fadeInCoroutine());
	}

	public void onClickControlsButton(){
        LoadingBehavior.ActionToPerform = LoadingBehavior.Action.Controls;
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
