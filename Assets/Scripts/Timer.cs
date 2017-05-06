using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Timer : MonoBehaviour {
    static public IEnumerator launchActionWithDelay(int DelayInSeconds, Action CallBack){
        yield return new WaitForSeconds(DelayInSeconds);
        CallBack();
    }
}
