using UnityEngine;
using UnityEngine.UI;

public class IntroGame : MonoBehaviour {

    [SerializeField]private GameObject LayerFadeInOut;
    [SerializeField]private float FadeInOutSpeed = 0;

    // Use this for initialization
    void Start () {
        LayerFadeInOut.GetComponent<Image>().color = Color.clear;
    }
	
	public void onClickStartSimButton(){
        EventsDBModel.logEvent(EventsTypesDB.UserEvent, SubEventsTypesDB.MenuGame, "From Intro To StartSimulation");
        LoadingBehavior.ActionToPerform = LoadingBehavior.Action.StartSim;
        LoadingBehavior.Timer = 1;
        StartCoroutine(Fade.fadeInCoroutine(LayerFadeInOut, FadeInOutSpeed, GameManager.loadLoading));
	}

	public void onClickTutorialButton(){
        EventsDBModel.logEvent(EventsTypesDB.UserEvent, SubEventsTypesDB.MenuGame, "From Intro To Tutorial");
        LoadingBehavior.ActionToPerform = LoadingBehavior.Action.Tutorials;
        LoadingBehavior.Timer = 1;
        StartCoroutine(Fade.fadeInCoroutine(LayerFadeInOut, FadeInOutSpeed, GameManager.loadLoading));
	}

	public void onClickAboutUsButton(){
        EventsDBModel.logEvent(EventsTypesDB.UserEvent, SubEventsTypesDB.MenuGame, "From Intro To AboutUs");
        LoadingBehavior.ActionToPerform = LoadingBehavior.Action.AboutUs;
        LoadingBehavior.Timer = 1;
        StartCoroutine(Fade.fadeInCoroutine(LayerFadeInOut, FadeInOutSpeed, GameManager.loadLoading));
    }
}
