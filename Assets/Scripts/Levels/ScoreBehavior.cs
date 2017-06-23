using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBehavior : MonoBehaviour {

    [SerializeField]private GameObject Title;
    [SerializeField]private GameObject BackButton;
    [SerializeField]private GameObject BackButtonText;
    [SerializeField]private GameObject LayerFadeInOut;
    public float FadeInOutSpeed = 0;

    [SerializeField]private Text ScoreLevelP1, PlayerLevelP1;
    [SerializeField]private Text ScoreLevelP2, PlayerLevelP2;
    [SerializeField]private Text ScoreLevelP3, PlayerLevelP3;
    [SerializeField] private Text ScoreLevelActual, PlayerLevelActual;

    [SerializeField]private GameObject Stopwatch;
    ScoreDBEntity CurrentScore;
    float TotalScoreElapsedTime = 0.0f;

    private void Start(){
        LayerFadeInOut.GetComponent<Image>().color = Color.black;
        LayerFadeInOut.SetActive(true);
    }

    public void launchScoreLayer(){
        float CurrentElapsedTime = 0.0f;
        switch (LevelsManager.getCurrentLevel()){
            case LevelsManager.Levels.L1:
                CurrentElapsedTime = Stopwatch.GetComponent<StopwatchUpdater>().getTimeElapsedInitLevel();
                TotalScoreElapsedTime = CurrentElapsedTime;
                loadScoreLevel1();
                break;
            case LevelsManager.Levels.L2:
                loadScoreLevel2();
                CurrentElapsedTime = Stopwatch.GetComponent<StopwatchUpdater>().getTimeElapsedInitLevel();
                TotalScoreElapsedTime += CurrentElapsedTime;
                break;
            case LevelsManager.Levels.L3:
                loadScoreLevel3();
                CurrentElapsedTime = Stopwatch.GetComponent<StopwatchUpdater>().getTimeElapsedInitLevel();
                TotalScoreElapsedTime += CurrentElapsedTime;
                break;
            case LevelsManager.Levels.L4:
                loadScoreLevel4();
                CurrentElapsedTime = Stopwatch.GetComponent<StopwatchUpdater>().getTimeElapsedInitLevel();
                TotalScoreElapsedTime += CurrentElapsedTime;
                break;
            case LevelsManager.Levels.End:
                CurrentElapsedTime = TotalScoreElapsedTime;
                loadScoreTotal(); break;
            default:
                break;
        }
        
        var CurrentTimeStamp = System.DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
        CurrentScore = new ScoreDBEntity {
            UUID = SystemInfo.deviceUniqueIdentifier + "_"+ CurrentTimeStamp + "_"+ Random.Range(0, 1000000).ToString(),
            DeviceID = SystemInfo.deviceUniqueIdentifier,
            Level = (int)LevelsManager.getCurrentLevel(),
            TimeElapsed = CurrentElapsedTime,
            Timestamp = CurrentTimeStamp,
            RobotConfiguration = DBModel.getRobotConfigurationString(),
            DisplayName = GameManager.PlayerName,
            Email = GameManager.EmailPlayer,
            GameID = GameManager.GameID
        };
        
        ScoreLevelActual.text = CurrentScore.TimeElapsed.ToString() + " s";
        PlayerLevelActual.text = CurrentScore.DisplayName;
        ScoreDBModel.GetScoreTable(CurrentScore);
        StartCoroutine(loadScoreInGUI());
    }

    private IEnumerator loadScoreInGUI(){
        const int WATCHDOG = 20;
        for (int i = 0;;i++)
        {
            if (ScoreDBModel.ScoreRetrieved ||
                i >= WATCHDOG){
                ScoreDBModel.ScoreRetrieved = false;
                ScoreDBModel.getScoreFirst(ref ScoreLevelP1, ref PlayerLevelP1);
                ScoreDBModel.getScoreSecond(ref ScoreLevelP2, ref PlayerLevelP2);
                ScoreDBModel.getScoreThird(ref ScoreLevelP3, ref PlayerLevelP3);
                ScoreDBModel.loadCurrentScorePosition(ref PlayerLevelActual, CurrentScore);
                LayerFadeInOut.GetComponent<Image>().color = Color.clear;
                LayerFadeInOut.SetActive(false);
                break;
            }else{
                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    public void onClickGoBackButton(){
        StartCoroutine(Fade.fadeInCoroutine(LayerFadeInOut, FadeInOutSpeed, backToLevel));
    }

    void backToLevel(){
        ScoreDBModel.CreateScoreInTable(CurrentScore);
        EventsDBModel.logEvent(EventsTypesDB.ScoreEvent, SubEventsTypesDB.Score, "TimeElapsed: " + CurrentScore.Timestamp.ToString());
        EventsDBModel.CreateEventsInTable();
        LevelsManager.ExitScoreMenu = true;
    }

    public void loadScoreLevel1(){
        BackButtonText.GetComponent<Text>().text = "New Level!";
        Title.GetComponent<Text>().text = "Score Level 1";
    }
    public void loadScoreLevel2(){
        BackButtonText.GetComponent<Text>().text = "New level!";
        Title.GetComponent<Text>().text = "Score Level 2";
    }
    public void loadScoreLevel3(){
        BackButtonText.GetComponent<Text>().text = "Last level!";
        Title.GetComponent<Text>().text = "Score Level 3";
    }
    public void loadScoreLevel4(){
        BackButtonText.GetComponent<Text>().text = "Total Score";
        Title.GetComponent<Text>().text = "Score Level 4";
    }
    public void loadScoreTotal(){
        BackButtonText.GetComponent<Text>().text = "Back to Menu";
        Title.GetComponent<Text>().text = "Total Score";
    }
}
