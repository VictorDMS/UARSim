using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
 
public class Fade{
    static public IEnumerator fadeInCoroutine(GameObject LayerFadeInOut, float FadeInOutDuration, Action CallBack)
    {
        float t = 0.0f;
        LayerFadeInOut.SetActive(true);
        Image ImageFadeInOut = LayerFadeInOut.GetComponent<Image>();
        for (;;)
        {
            ImageFadeInOut.color = Color.Lerp(ImageFadeInOut.color, Color.black, t);
            if (t < 0.95){
                t += Time.deltaTime / FadeInOutDuration;
                yield return null;
            }else{
                ImageFadeInOut.color = Color.black;
                CallBack();
                break;
            }
        }
    }

    static public IEnumerator fadeOutCoroutine(GameObject LayerFadeInOut, float FadeInOutDuration)
    {
        float t = 0.0f;
        LayerFadeInOut.SetActive(true);
        Image ImageFadeInOut = LayerFadeInOut.GetComponent<Image>();
        for (;;){
            ImageFadeInOut.color = Color.Lerp(ImageFadeInOut.color, Color.clear, t);
            if (ImageFadeInOut.color.a > 0.05f){
                t += Time.deltaTime / FadeInOutDuration;
                yield return null;
            }else{
                ImageFadeInOut.color = Color.clear;
                break;
            }
        }
        LayerFadeInOut.SetActive(false);
    }
}
