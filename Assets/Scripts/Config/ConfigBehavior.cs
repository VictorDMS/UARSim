using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConfigBehavior : MonoBehaviour {

    //private GameObject[] QuadrotorMov;
    //private GameObject[] LightMov;
    //private GameObject[] HeavyMov;
    //private GameObject[] UltraMov;

    [SerializeField]private GameObject LayerFadeInOut;
    public float FadeInOutSpeed = 0;

    [SerializeField]private GameObject QuadSpiral, QuadScan, QuadRandom;
    [SerializeField]private GameObject LightRight, LightLeft, LightRandom;
    [SerializeField]private GameObject HeavyRight, HeavyLeft, HeavyRandom;
    [SerializeField]private GameObject UltraRight, UltraLeft, UltraRandom;

    // Use this for initialization
    void Start()
    {
        LayerFadeInOut.GetComponent<Image>().color = Color.clear;

        QuadSpiral.GetComponent<Toggle>().isOn = false;
        QuadScan.GetComponent<Toggle>().isOn = false;
        QuadRandom.GetComponent<Toggle>().isOn = false;
        LightRight.GetComponent<Toggle>().isOn = false;
        LightLeft.GetComponent<Toggle>().isOn = false;
        LightRandom.GetComponent<Toggle>().isOn = false;
        HeavyRight.GetComponent<Toggle>().isOn = false;
        HeavyLeft.GetComponent<Toggle>().isOn = false;
        HeavyRandom.GetComponent<Toggle>().isOn = false;
        UltraRight.GetComponent<Toggle>().isOn = false;
        UltraLeft.GetComponent<Toggle>().isOn = false;
        UltraRandom.GetComponent<Toggle>().isOn = false;
    }
    public void onToggleQuadSpiral(bool newValue){
        if (newValue){
            QuadSpiral.GetComponent<Toggle>().isOn = true;
            QuadScan.GetComponent<Toggle>().isOn = false;
            QuadRandom.GetComponent<Toggle>().isOn = false;
        }
    }
    public void onToggleQuadScan(bool newValue){
        if (newValue)
        {
            QuadSpiral.GetComponent<Toggle>().isOn = false;
            QuadScan.GetComponent<Toggle>().isOn = true;
            QuadRandom.GetComponent<Toggle>().isOn = false;
        }
       
    }
    public void onToggleQuadRandom(bool newValue){
        if (newValue)
        {
            QuadSpiral.GetComponent<Toggle>().isOn = false;
            QuadScan.GetComponent<Toggle>().isOn = false;
            QuadRandom.GetComponent<Toggle>().isOn = true;
        }
    }
    public void onToggleLightRight(bool newValue){
        if (newValue)
        {
            LightRight.GetComponent<Toggle>().isOn = true;
            LightLeft.GetComponent<Toggle>().isOn = false;
            LightRandom.GetComponent<Toggle>().isOn = false;
        }
    }
    public void onToggleLightLeft(bool newValue){
        if (newValue)
        {
            LightRight.GetComponent<Toggle>().isOn = false;
            LightLeft.GetComponent<Toggle>().isOn = true;
            LightRandom.GetComponent<Toggle>().isOn = false;
        }
    }
    public void onToggleLightRandom(bool newValue){
        if (newValue)
        {
            LightRight.GetComponent<Toggle>().isOn = false;
            LightLeft.GetComponent<Toggle>().isOn = false;
            LightRandom.GetComponent<Toggle>().isOn = true;
        }
    }
    public void onToggleHeavyRight(bool newValue){
        if (newValue)
        {
            HeavyRight.GetComponent<Toggle>().isOn = true;
            HeavyLeft.GetComponent<Toggle>().isOn = false;
            HeavyRandom.GetComponent<Toggle>().isOn = false;
        }
    }
    public void onToggleHeavyLeft(bool newValue){
        if (newValue)
        {
            HeavyRight.GetComponent<Toggle>().isOn = false;
            HeavyLeft.GetComponent<Toggle>().isOn = true;
            HeavyRandom.GetComponent<Toggle>().isOn = false;
        }
    }
    public void onToggleHeavyRandom(bool newValue){
        if (newValue)
        {
            HeavyRight.GetComponent<Toggle>().isOn = false;
            HeavyLeft.GetComponent<Toggle>().isOn = false;
            HeavyRandom.GetComponent<Toggle>().isOn = true;
        }
    }
    public void onToggleUltraRight(bool newValue){
        if (newValue)
        {
            UltraRight.GetComponent<Toggle>().isOn = true;
            UltraLeft.GetComponent<Toggle>().isOn = false;
            UltraRandom.GetComponent<Toggle>().isOn = false;
        }
    }
    public void onToggleUltraLeft(bool newValue){
        if (newValue)
        {
            UltraRight.GetComponent<Toggle>().isOn = false;
            UltraLeft.GetComponent<Toggle>().isOn = true;
            UltraRandom.GetComponent<Toggle>().isOn = false;
        }
    }
    public void onToggleUltraRandom(bool newValue){
        if (newValue)
        {
            UltraRight.GetComponent<Toggle>().isOn = false;
            UltraLeft.GetComponent<Toggle>().isOn = false;
            UltraRandom.GetComponent<Toggle>().isOn = true;
        }
    }
    public void onClickBack() {
        StartCoroutine(fadeInCoroutine());
    }
    IEnumerator fadeInCoroutine()
    {
        LayerFadeInOut.SetActive(true);
        Image ImageFadeInOut = LayerFadeInOut.GetComponent<Image>();
        for (;;)
        {
            ImageFadeInOut.color = Color.Lerp(ImageFadeInOut.color, Color.black, (FadeInOutSpeed / 2) * Time.deltaTime);
            if (ImageFadeInOut.color.a >= 0.95f)
            {
                ImageFadeInOut.color = Color.black;
                SceneManager.LoadScene("Level1_Maze");
                break;
            }
            else
            {
                yield return null;
            }
        }
        print("Process \"fade in\" is done.");
    }
}
