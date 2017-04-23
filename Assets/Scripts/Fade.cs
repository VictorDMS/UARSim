using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Fade{
    static public IEnumerator fadeInCoroutine(GameObject LayerFadeInOut, float FadeInOutSpeed, Action CallBack)
    {
        LayerFadeInOut.SetActive(true);
        Image ImageFadeInOut = LayerFadeInOut.GetComponent<Image>();
        for (;;)
        {
            ImageFadeInOut.color = Color.Lerp(ImageFadeInOut.color, Color.black, (FadeInOutSpeed / 2) * Time.deltaTime);
            if (ImageFadeInOut.color.a >= 0.95f)
            {
                ImageFadeInOut.color = Color.black;
                CallBack();
                break;
            }
            else
            {
                yield return null;
            }
        }
    }

    static public IEnumerator fadeOutCoroutine(GameObject LayerFadeInOut, float FadeInOutSpeed)
    {
        LayerFadeInOut.SetActive(true);
        Image ImageFadeInOut = LayerFadeInOut.GetComponent<Image>();
        for (;;)
        {
            ImageFadeInOut.color = Color.Lerp(ImageFadeInOut.color, Color.clear, (FadeInOutSpeed / 2) * Time.deltaTime);
            if (ImageFadeInOut.color.a <= 0.05f)
            {
                ImageFadeInOut.color = Color.clear;
                break;
            }
            else
            {
                yield return null;
            }
        }
        LayerFadeInOut.SetActive(false);
    }
}
