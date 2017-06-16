using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsManager : MonoBehaviour {

    public const short MaxPoints_L1 = 1;
    public const short MaxPoints_L2 = 2;
    public const short MaxPoints_L3 = 3;
    public const short MaxPoints_L4 = 5;

    public static short Points_L1 = MaxPoints_L1;
    public static short Points_L2 = MaxPoints_L2;
    public static short Points_L3 = MaxPoints_L3;
    public static short Points_L4 = MaxPoints_L4;

    public static bool LoadConfig = false;
    public static bool LoadScore = false;
    public static bool LoadLevel = false;

    public static bool ExitConfigMenu = false;
    public static bool ExitScoreMenu = false;
    public static bool FirstTimeNewLevel = true;

    public static bool ShowFirstTimeControlImageVehicle = true;
    public static bool ShowFirstTimeControlImageDrone = true;
    
    public enum Levels { Start = 10, L1 = 1, L2 = 2, L3 = 3, L4 = 4, End = 11, unknown = -1 };
    private static Levels CurrentLevel = Levels.Start;
        
    public static bool foundGoal(){
        bool FinishedLevel = false;
        int RemainingGoalsForLog = 0;
        switch (CurrentLevel){
            case Levels.L1:
                --Points_L1;
                RemainingGoalsForLog = Points_L1;
                if (Points_L1 == 0){
                    FinishedLevel = true;
                    Points_L1 = MaxPoints_L1;
                }
                break;
            case Levels.L2:
                --Points_L2;
                RemainingGoalsForLog = Points_L2;
                if (Points_L2 == 0){
                    FinishedLevel = true;
                    Points_L2 = MaxPoints_L2;
                }
                break;
            case Levels.L3:
                --Points_L3;
                RemainingGoalsForLog = Points_L3;
                if (Points_L3 == 0){
                    FinishedLevel = true;
                    Points_L3 = MaxPoints_L3;
                }
                break;
            case Levels.L4:
                --Points_L4;
                RemainingGoalsForLog = Points_L4;
                if (Points_L4 == 0){
                    FinishedLevel = true;
                    Points_L4 = MaxPoints_L4;
                }
                break;
        }
        EventsDBModel.logEvent(EventsTypesDB.GameEvent, SubEventsTypesDB.GetGoal, "Remaining: " + RemainingGoalsForLog);
        return FinishedLevel;
    }
    public static void loadNewLevel(){
        switch (CurrentLevel){
            case Levels.Start:
                loadLevel1();
                break;
            case Levels.L1:
                loadLevel2();
                break;
            case Levels.L2:
                loadLevel3();
                break;
            case Levels.L3:
                loadLevel4();
                break;
            case Levels.L4://Here we load the final score. There is no new level.
                CurrentLevel = Levels.End;
                LoadScore = true;
                break;
        }
    }
    public static void endLevel(){
        LoadScore = true;
    }
    public static Levels getCurrentLevel(){
        return CurrentLevel;
    }
    private static void loadLevel1(){
        CurrentLevel = Levels.L1;
        LoadLevel = true;
        LoadConfig = true;
        FirstTimeNewLevel = true;
    }
    private static void loadLevel2(){
        CurrentLevel = Levels.L2;
        LoadLevel = true;
        LoadConfig = true;
        FirstTimeNewLevel = true;
    }
    private static void loadLevel3(){
        CurrentLevel = Levels.L3;
        LoadLevel = true;
        LoadConfig = true;
        FirstTimeNewLevel = true;
    }
    private static void loadLevel4(){
        CurrentLevel = Levels.L4;
        LoadLevel = true;
        LoadConfig = true;
        FirstTimeNewLevel = true;
    }

    public static void GameOver(){
        LoadConfig = false;
        LoadScore = false;
        LoadLevel = false;
        ExitConfigMenu = false;
        ExitScoreMenu = false;
        FirstTimeNewLevel = true;
        ShowFirstTimeControlImageVehicle = true;
        ShowFirstTimeControlImageDrone = true;
        CurrentLevel = Levels.Start;
        GameManager.PlayerName = "";
        GameManager.loadIntro();
    }
}
