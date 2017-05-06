using UnityEngine;
using UnityEngine.UI;

public class ScoreBehavior : MonoBehaviour {
    
    [SerializeField]private GameObject BackButton;
    [SerializeField]private GameObject BackButtonText;
    [SerializeField]private GameObject LayerFadeInOut;
    public float FadeInOutSpeed = 0;

    [SerializeField]private Text ScoreLevelP1, PlayerLevelP1;
    [SerializeField]private Text ScoreLevelP2, PlayerLevelP2;
    [SerializeField]private Text ScoreLevelP3, PlayerLevelP3;

    public void launchScoreLayer()
    {
        switch (LevelsManager.getCurrentLevel()){
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
            case LevelsManager.Levels.End:
                loadScoreTotal();
                break;
            default:
                break;
        }
        LayerFadeInOut.GetComponent<Image>().color = Color.clear;
        LayerFadeInOut.SetActive(false);
    }

    public void onClickGoBackButton(){
        StartCoroutine(Fade.fadeInCoroutine(LayerFadeInOut, FadeInOutSpeed, backToLevel));
    }

    void backToLevel(){
        LevelsManager.ExitScoreMenu = true;
    }

    public void loadScoreLevel1(){
        ScoreDBModel.getScoreFirst(ref ScoreLevelP1, ref PlayerLevelP1);
        ScoreDBModel.getScoreSecond(ref ScoreLevelP2, ref PlayerLevelP2);
        ScoreDBModel.getScoreThird(ref ScoreLevelP3, ref PlayerLevelP3);
        BackButtonText.GetComponent<Text>().text = "New level!";
    }
    public void loadScoreLevel2(){
        ScoreDBModel.getScoreFirst(ref ScoreLevelP1, ref PlayerLevelP1);
        ScoreDBModel.getScoreSecond(ref ScoreLevelP2, ref PlayerLevelP2);
        ScoreDBModel.getScoreThird(ref ScoreLevelP3, ref PlayerLevelP3);
        BackButtonText.GetComponent<Text>().text = "New level!";
    }
    public void loadScoreLevel3(){
        ScoreDBModel.getScoreFirst(ref ScoreLevelP1, ref PlayerLevelP1);
        ScoreDBModel.getScoreSecond(ref ScoreLevelP2, ref PlayerLevelP2);
        ScoreDBModel.getScoreThird(ref ScoreLevelP3, ref PlayerLevelP3);
        BackButtonText.GetComponent<Text>().text = "Last level!";
    }
    public void loadScoreLevel4(){
        ScoreDBModel.getScoreFirst(ref ScoreLevelP1, ref PlayerLevelP1);
        ScoreDBModel.getScoreSecond(ref ScoreLevelP2, ref PlayerLevelP2);
        ScoreDBModel.getScoreThird(ref ScoreLevelP3, ref PlayerLevelP3);
        BackButtonText.GetComponent<Text>().text = "Total Score";
    }
    public void loadScoreTotal(){
        ScoreDBModel.getScoreFirst(ref ScoreLevelP1, ref PlayerLevelP1);
        ScoreDBModel.getScoreSecond(ref ScoreLevelP2, ref PlayerLevelP2);
        ScoreDBModel.getScoreThird(ref ScoreLevelP3, ref PlayerLevelP3);
        BackButtonText.GetComponent<Text>().text = "Finish :)";
    }
}
