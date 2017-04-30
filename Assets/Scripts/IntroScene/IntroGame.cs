﻿    using UnityEngine;
using System.Collections;
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
        LoadingBehavior.Timer = 1;
        StartCoroutine(Fade.fadeInCoroutine(LayerFadeInOut, FadeInOutSpeed, GameManager.loadLoading));
	}

	public void onClickTutorialButton(){
        LoadingBehavior.ActionToPerform = LoadingBehavior.Action.Tutorials;
        LoadingBehavior.Timer = 1;
        StartCoroutine(Fade.fadeInCoroutine(LayerFadeInOut, FadeInOutSpeed, GameManager.loadLoading));
	}

	public void onClickControlsButton(){
        LoadingBehavior.ActionToPerform = LoadingBehavior.Action.Controls;
        LoadingBehavior.Timer = 1;
        StartCoroutine(Fade.fadeInCoroutine(LayerFadeInOut, FadeInOutSpeed, GameManager.loadLoading));
    }
}
