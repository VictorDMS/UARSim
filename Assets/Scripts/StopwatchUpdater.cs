﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StopwatchUpdater : MonoBehaviour {

	private float startTime;
	private float elapsedTime;
	public Text timerText;

	void Start () {
		startTime = Time.time;
	}

	void Update () {
		float t = Time.time - startTime;

		int nHours = ((int)t / (60 * 60));
		int nMinutes = ((int)t / 60);
		float nSeconds = (t % 60);

		string sHours 	= nHours > 9 ? nHours.ToString () : "0" + nHours.ToString ();
		string sMinutes = nMinutes > 9 ? nMinutes.ToString () : "0" + nMinutes.ToString ();
		string sSeconds = nSeconds >= 10.0f ? nSeconds.ToString ("f2") : "0" + nSeconds.ToString ("f2"); 

		timerText.text = sHours + ":" + sMinutes + ":" + sSeconds;
	}
}
