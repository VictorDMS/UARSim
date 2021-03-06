﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StopwatchUpdater : MonoBehaviour {

	private float startTime;
    private int FPSIterator = 0;
    private bool ClockStarted = false;
    private float TimeElapsedInitLevel = 0.0f;

    private const float PUNISHMENT_TIME_L3 = 5.0f;
    private const float PUNISHMENT_TIME_L4 = 10.0f;
    private const float REDTIME_DURATION = 5.0f;

    [SerializeField] private Text timerText;
    [SerializeField] private int FPSFrecuency = 10;
    
    public void startStopwatch(){
        startTime = Time.time;
        TimeElapsedInitLevel = 0.0f;
        ClockStarted = true;
    }
    
    public float getTimeElapsedInitLevel(){
        return TimeElapsedInitLevel;
    }

	void Update () {
        if (ClockStarted){
            if (FPSIterator >= FPSFrecuency){
                float t = Time.time - startTime;
                int nHours = ((int)t / (60 * 60));
                int nMinutes = ((int)t / 60);
                float nSeconds = (t % 60);
                string sHours = nHours > 9 ? nHours.ToString() : "0" + nHours.ToString();
                string sMinutes = nMinutes > 9 ? nMinutes.ToString() : "0" + nMinutes.ToString();
                string sSeconds = nSeconds >= 10.0f ? nSeconds.ToString("f2") : "0" + nSeconds.ToString("f2");
                timerText.text = sHours + ":" + sMinutes + ":" + sSeconds;
                FPSIterator = 0;
                TimeElapsedInitLevel = t;
            }
            else{
                FPSIterator++;
            }
        }
	}

    public void punishForBreakWall(){//How to add time? reducing startTime
        float PunishmentValue = 0.0f;
        if(LevelsManager.getCurrentLevel() == LevelsManager.Levels.L3){
            PunishmentValue = PUNISHMENT_TIME_L3;
            startTime -= PunishmentValue;
        }else if(LevelsManager.getCurrentLevel() == LevelsManager.Levels.L4){
            PunishmentValue = PUNISHMENT_TIME_L4;
            startTime -= PunishmentValue;
        }
        EventsDBModel.logEvent(EventsTypesDB.GameEvent, SubEventsTypesDB.WallDestroyed, "Punishment: " + PunishmentValue.ToString());
        StartCoroutine(turnOnAndOffRedColour());
    }

    private IEnumerator turnOnAndOffRedColour(){
        timerText.color = Color.red;
        yield return new WaitForSeconds(REDTIME_DURATION);
        timerText.color = Color.black;
    }
}
