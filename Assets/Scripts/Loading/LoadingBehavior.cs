using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingBehavior : MonoBehaviour {

    [SerializeField] private Text TitleText;
    private bool IsGrowing = true; 
    public enum Action { Intro, StartSim, NameSelector, Tutorials, AboutUs, unknown };
    public static Action ActionToPerform = Action.unknown;
    public static int Timer = 0;
    public GameObject LayerFadeInOut;
    public float FadeInOutSpeed = 0;

    private const int MAX_LOADING_SIZE = 230;
    private const int MIN_LOADING_SIZE = 80;
    private const int RELATIVE_UPDATE_LOADING_SIZE = 5;

    void Start () {
        LayerFadeInOut.GetComponent<Image>().color = Color.clear;
        StartCoroutine(loadingSceneTimer());
        IsGrowing = true;
    }
    private void FixedUpdate(){
        if (IsGrowing && TitleText.fontSize < MAX_LOADING_SIZE) {
            TitleText.fontSize += RELATIVE_UPDATE_LOADING_SIZE;
        }
        else if (IsGrowing && TitleText.fontSize >= MAX_LOADING_SIZE) {
            IsGrowing = false;
        }else if(!IsGrowing && TitleText.fontSize > MIN_LOADING_SIZE){
            TitleText.fontSize -= RELATIVE_UPDATE_LOADING_SIZE;
        }else if(!IsGrowing && TitleText.fontSize <= MIN_LOADING_SIZE){
            IsGrowing = true;
        }
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
                if(GameManager.PlayerName.Length > 0 || Debug.isDebugBuild){
                    if(GameManager.PlayerName.Length == 0){
                        GameManager.PlayerName = "TestUser";
                    }
                    GameManager.loadLevel();
                }else{
                    GameManager.loadNameSelector();
                }
                break;
            case Action.NameSelector:
                GameManager.loadLevel();
                break;
            case Action.Tutorials:
                GameManager.loadTutorials();
                break;
            case Action.AboutUs:
                GameManager.loadAboutUs();
                break;
            case Action.unknown:
            default:
                break;
        }
    }
}
