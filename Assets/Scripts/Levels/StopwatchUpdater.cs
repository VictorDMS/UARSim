using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StopwatchUpdater : MonoBehaviour {

	private float startTime;
    private int FPSIterator = 0;
    private bool ClockStarted = false;

    [SerializeField] private Text timerText;
    [SerializeField] private int FPSFrecuency = 5;

	void Start () {
	}

    public void startStopwatch(){
        startTime = Time.time;
        ClockStarted = true;
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
            }else{
                FPSIterator++;
            }
        }
	}
}
