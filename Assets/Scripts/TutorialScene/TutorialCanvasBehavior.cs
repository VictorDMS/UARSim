using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialCanvasBehavior : MonoBehaviour
{
    [SerializeField]private int SpeedTransition = 1;
    [SerializeField]private GameObject LayerFadeInOut;
    [SerializeField]private GameObject TutorialCanvas, ControlsCanvas, RobotConfigCanvas, TipsCanvas;
    public float FadeInOutSpeed = 0;

    // Use this for initialization
    void Start () {
        TutorialCanvas.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        ControlsCanvas.transform.localPosition = new Vector3(1920.0f, 0.0f, 0.0f);
        RobotConfigCanvas.transform.localPosition = new Vector3(1920.0f, 0.0f, 0.0f);
        TipsCanvas.transform.localPosition = new Vector3(1920.0f, 0.0f, 0.0f);
    }
	
    public void onClickNextFromTutorial(){
        StartCoroutine(moveTutorialAndControls());
    }
    public void onClickNextFromControls(){
        StartCoroutine(moveControlsAndRobotConfig());
    }
    public void onClickNextFromRobotConfig(){
        StartCoroutine(moveRobotConfigAndTips());
    }
    public void onClickFinishFromTips(){
        LoadingBehavior.ActionToPerform = LoadingBehavior.Action.Intro;
        LoadingBehavior.Timer = 1;
        StartCoroutine(Fade.fadeInCoroutine(LayerFadeInOut, FadeInOutSpeed, loadLevel));
    }

    public IEnumerator moveTutorialAndControls(){
        const int WATCHDOG_LIM = 5000;
        int Watchdog = 0;
        float TotalIncrease = 0.0f, TotalDecrese = 1920.0f;
        while (Watchdog < WATCHDOG_LIM){
            TotalIncrease -= SpeedTransition;
            TotalDecrese -= SpeedTransition;
            TutorialCanvas.transform.localPosition = new Vector3(TotalIncrease, 0.0f, 0.0f);
            ControlsCanvas.transform.localPosition = new Vector3(TotalDecrese, 0.0f, 0.0f);
            ++Watchdog;
            if(TotalDecrese <= 0.0f || TotalIncrease <= -1920.0f){
                TutorialCanvas.transform.localPosition = new Vector3(-1920.0f, 0.0f, 0.0f);
                ControlsCanvas.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                break;
            }
            yield return null;
        }
    }

    public IEnumerator moveControlsAndRobotConfig()
    {
        const int WATCHDOG_LIM = 5000;
        int Watchdog = 0;
        float TotalIncrease = 0.0f, TotalDecrese = 1920.0f;
        while (Watchdog < WATCHDOG_LIM)
        {
            TotalIncrease -= SpeedTransition;
            TotalDecrese -= SpeedTransition;
            ControlsCanvas.transform.localPosition = new Vector3(TotalIncrease, 0.0f, 0.0f);
            RobotConfigCanvas.transform.localPosition = new Vector3(TotalDecrese, 0.0f, 0.0f);
            ++Watchdog;
            if (TotalDecrese <= 0.0f || TotalIncrease <= -1920.0f)
            {
                ControlsCanvas.transform.localPosition = new Vector3(-1920.0f, 0.0f, 0.0f);
                RobotConfigCanvas.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                break;
            }
            yield return null;
        }
    }

    public IEnumerator moveRobotConfigAndTips()
    {
        const int WATCHDOG_LIM = 5000;
        int Watchdog = 0;
        float TotalIncrease = 0.0f, TotalDecrese = 1920.0f;
        while (Watchdog < WATCHDOG_LIM)
        {
            TotalIncrease -= SpeedTransition;
            TotalDecrese -= SpeedTransition;
            RobotConfigCanvas.transform.localPosition = new Vector3(TotalIncrease, 0.0f, 0.0f);
            TipsCanvas.transform.localPosition = new Vector3(TotalDecrese, 0.0f, 0.0f);
            ++Watchdog;
            if (TotalDecrese <= 0.0f || TotalIncrease <= -1920.0f)
            {
                RobotConfigCanvas.transform.localPosition = new Vector3(-1920.0f, 0.0f, 0.0f);
                TipsCanvas.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                break;
            }
            yield return null;
        }
    }

    void loadLevel(){
        GameManager.loadLoading();
    }
}
