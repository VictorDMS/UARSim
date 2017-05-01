using UnityEngine;
using UnityEngine.UI;

public class ScoreBehavior : MonoBehaviour {

    [SerializeField]private GameObject BackButton;
    [SerializeField]private GameObject BackButtonText;
    [SerializeField]private GameObject LayerFadeInOut;
    public float FadeInOutSpeed = 0;

    static string ScoreLevel1P1 = "", PlayerLevel1P1 = "";
    static string ScoreLevel2P1 = "", PlayerLevel2P1 = "";
    static string ScoreLevel3P1 = "", PlayerLevel3P1 = "";
    static string ScoreLevel4P1 = "", PlayerLevel4P1 = "";

    static string ScoreLevel1P2 = "", PlayerLevel1P2 = "";
    static string ScoreLevel2P2 = "", PlayerLevel2P2 = "";
    static string ScoreLevel3P2 = "", PlayerLevel3P2 = "";
    static string ScoreLevel4P2 = "", PlayerLevel4P2 = "";

    static string ScoreLevel1P3 = "", PlayerLevel1P3 = "";
    static string ScoreLevel2P3 = "", PlayerLevel2P3 = "";
    static string ScoreLevel3P3 = "", PlayerLevel3P3 = "";
    static string ScoreLevel4P3 = "", PlayerLevel4P3 = "";

    public void launchScoreLayer()
    {
        switch (LevelsManager.getCurrentLevel())
        {
            case LevelsManager.Levels.L1:
                loadScoreLevel1();
                break;
            case LevelsManager.Levels.L2:
                loadScoreLevel2();
                break;
            case LevelsManager.Levels.L3:
                loadScoreLevel3();
                break;
            case LevelsManager.Levels.L4:
                loadScoreLevel4();
                break;
            default:
                break;
        }
        BackButtonText.GetComponent<Text>().text = "Start new level!";
        LayerFadeInOut.GetComponent<Image>().color = Color.clear;
        LayerFadeInOut.SetActive(false);
    }

    public void onClickGoBackButton(){

    }

    public static void loadScoreLevel1(){

    }
    public static void loadScoreLevel2(){

    }
    public static void loadScoreLevel3(){

    }
    public static void loadScoreLevel4(){

    }
    public static void loadScoreTotal(){

    }
}
