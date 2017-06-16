using UnityEngine;
using UnityEngine.UI;

public class AboutUsManager : MonoBehaviour {
    public GameObject LayerFadeInOut;
    public float FadeInOutSpeed = 0;

    // Use this for initialization
    void Start () {
        LayerFadeInOut.GetComponent<Image>().color = Color.clear;
    }
	
    public void onClickGoBack(){
        EventsDBModel.logEvent(EventsTypesDB.UserEvent, SubEventsTypesDB.MenuGame, "From AboutUs To Intro");
        LoadingBehavior.ActionToPerform = LoadingBehavior.Action.Intro;
        LoadingBehavior.Timer = 1;
        StartCoroutine(Fade.fadeInCoroutine(LayerFadeInOut, FadeInOutSpeed, GameManager.loadLoading));
    }
}