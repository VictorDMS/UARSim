using UnityEngine;
using System.Collections;
using System;

public class Timer {
    static public IEnumerator launchActionWithDelay(int DelayInSeconds, Action CallBack){
        yield return new WaitForSeconds(DelayInSeconds);
        CallBack();
    }
}
