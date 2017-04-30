using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsManager : MonoBehaviour {
    public static LevelsManager instance = null;

    public const short MaxPoints_L1 = 5;
    public const short MaxPoints_L2 = 5;
    public const short MaxPoints_L3 = 5;
    public const short MaxPoints_L4 = 5;

    public static short Points_L1 = MaxPoints_L1;
    public static short Points_L2 = MaxPoints_L2;
    public static short Points_L3 = MaxPoints_L3;
    public static short Points_L4 = MaxPoints_L4;

    public static bool LoadConfig = false;
    public static bool LoadScore = false;
    public static bool LoadLevel = false;

    public enum Levels { Start, L1, L2, L3, L4, End };
    private static Levels CurrentLevel = Levels.Start;

    void Awake () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    
    public static bool foundGoal(){
        bool FinishedLevel = false;
        switch (CurrentLevel){
            case Levels.L1:
                --Points_L1;
                if (Points_L1 == 0){
                    FinishedLevel = true;
                    Points_L1 = MaxPoints_L1;
                }
                break;
            case Levels.L2:
                --Points_L2;
                if (Points_L2 == 0){
                    FinishedLevel = true;
                    Points_L2 = MaxPoints_L2;
                }
                break;
            case Levels.L3:
                --Points_L3;
                if (Points_L3 == 0){
                    FinishedLevel = true;
                    Points_L3 = MaxPoints_L3;
                }
                break;
            case Levels.L4:
                --Points_L4;
                if (Points_L4 == 0){
                    FinishedLevel = true;
                    Points_L4 = MaxPoints_L4;
                }
                break;
        }
        return FinishedLevel;
    }

    public static void loadLevel(){
        switch (CurrentLevel)
        {
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
            case Levels.L4:
                CurrentLevel = Levels.End;
                break;
        }
        LoadScore = true;
    }

    public static Levels getCurrentLevel(){
        return CurrentLevel;
    }

    private static void loadLevel1(){
        LoadConfig = true;
        CurrentLevel = Levels.L1;
        LoadLevel = true;
    }
    private static void loadLevel2(){
        LoadConfig = true;
        CurrentLevel = Levels.L2;
        LoadLevel = true;
    }
    private static void loadLevel3(){
        LoadConfig = true;
        CurrentLevel = Levels.L3;
        LoadLevel = true;
    }
    private static void loadLevel4(){
        LoadConfig = true;
        CurrentLevel = Levels.L4;
        LoadLevel = true;
    }
}
