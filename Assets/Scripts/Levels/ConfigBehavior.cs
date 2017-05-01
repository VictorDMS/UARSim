using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConfigBehavior : MonoBehaviour {
    [SerializeField]private GameObject BackButton;
    [SerializeField]private GameObject BackButtonText;
    [SerializeField]private GameObject LayerFadeInOut;
    public float FadeInOutSpeed = 0;

    [SerializeField]private GameObject QuadSpiral, QuadScan, QuadRandom;
    [SerializeField]private GameObject LightRight, LightLeft, LightRandom;
    [SerializeField]private GameObject HeavyRight, HeavyLeft, HeavyRandom;
    [SerializeField]private GameObject UltraRight, UltraLeft, UltraRandom;
    [SerializeField]private GameObject Quad, Light, Heavy, Ultra;

    public enum VehicleRobotConfiguration { RANDOM, RIGHT, LEFT, UNKNOWN }
    public enum QuadRobotConfiguration { SPIRAL, SCAN, RANDOM, UNKNOWN }

    static public QuadRobotConfiguration QuadCurrentConfig = QuadRobotConfiguration.UNKNOWN;
    static public VehicleRobotConfiguration LightCurrentConfig = VehicleRobotConfiguration.UNKNOWN;
    static public VehicleRobotConfiguration HeavyCurrentConfig = VehicleRobotConfiguration.UNKNOWN;
    static public VehicleRobotConfiguration UltraCurrentConfig = VehicleRobotConfiguration.UNKNOWN;

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
        enableObject(Quad, false);
        enableObject(Light, false);
        enableObject(Heavy, false);
        enableObject(Ultra, false);
    }
    public void onToggleQuadSpiral(bool newValue){
        if (newValue){
            QuadSpiral.GetComponent<Toggle>().isOn = true;
            QuadScan.GetComponent<Toggle>().isOn = false;
            QuadRandom.GetComponent<Toggle>().isOn = false;
        }
        updateButton();
    }
    public void onToggleQuadScan(bool newValue){
        if (newValue)
        {
            QuadSpiral.GetComponent<Toggle>().isOn = false;
            QuadScan.GetComponent<Toggle>().isOn = true;
            QuadRandom.GetComponent<Toggle>().isOn = false;
        }
        updateButton();
    }
    public void onToggleQuadRandom(bool newValue){
        if (newValue)
        {
            QuadSpiral.GetComponent<Toggle>().isOn = false;
            QuadScan.GetComponent<Toggle>().isOn = false;
            QuadRandom.GetComponent<Toggle>().isOn = true;
        }
        updateButton();
    }
    public void onToggleLightRight(bool newValue){
        if (newValue)
        {
            LightRight.GetComponent<Toggle>().isOn = true;
            LightLeft.GetComponent<Toggle>().isOn = false;
            LightRandom.GetComponent<Toggle>().isOn = false;
        }
        updateButton();
    }
    public void onToggleLightLeft(bool newValue){
        if (newValue)
        {
            LightRight.GetComponent<Toggle>().isOn = false;
            LightLeft.GetComponent<Toggle>().isOn = true;
            LightRandom.GetComponent<Toggle>().isOn = false;
        }
        updateButton();
    }
    public void onToggleLightRandom(bool newValue){
        if (newValue)
        {
            LightRight.GetComponent<Toggle>().isOn = false;
            LightLeft.GetComponent<Toggle>().isOn = false;
            LightRandom.GetComponent<Toggle>().isOn = true;
        }
        updateButton();
    }
    public void onToggleHeavyRight(bool newValue){
        if (newValue)
        {
            HeavyRight.GetComponent<Toggle>().isOn = true;
            HeavyLeft.GetComponent<Toggle>().isOn = false;
            HeavyRandom.GetComponent<Toggle>().isOn = false;
        }
        updateButton();
    }
    public void onToggleHeavyLeft(bool newValue){
        if (newValue)
        {
            HeavyRight.GetComponent<Toggle>().isOn = false;
            HeavyLeft.GetComponent<Toggle>().isOn = true;
            HeavyRandom.GetComponent<Toggle>().isOn = false;
        }
        updateButton();
    }
    public void onToggleHeavyRandom(bool newValue){
        if (newValue)
        {
            HeavyRight.GetComponent<Toggle>().isOn = false;
            HeavyLeft.GetComponent<Toggle>().isOn = false;
            HeavyRandom.GetComponent<Toggle>().isOn = true;
        }
        updateButton();
    }
    public void onToggleUltraRight(bool newValue){
        if (newValue)
        {
            UltraRight.GetComponent<Toggle>().isOn = true;
            UltraLeft.GetComponent<Toggle>().isOn = false;
            UltraRandom.GetComponent<Toggle>().isOn = false;
        }
        updateButton();
    }
    public void onToggleUltraLeft(bool newValue){
        if (newValue)
        {
            UltraRight.GetComponent<Toggle>().isOn = false;
            UltraLeft.GetComponent<Toggle>().isOn = true;
            UltraRandom.GetComponent<Toggle>().isOn = false;
        }
        updateButton();
    }
    public void onToggleUltraRandom(bool newValue){
        if (newValue)
        {
            UltraRight.GetComponent<Toggle>().isOn = false;
            UltraLeft.GetComponent<Toggle>().isOn = false;
            UltraRandom.GetComponent<Toggle>().isOn = true;
        }
        updateButton();
    }
    private void updateButton()
    {
        switch (LevelsManager.getCurrentLevel())
        {
            case LevelsManager.Levels.L1:
                if ((LightRight.GetComponent<Toggle>().isOn || LightLeft.GetComponent<Toggle>().isOn || LightRandom.GetComponent<Toggle>().isOn) &&
                    (QuadRandom.GetComponent<Toggle>().isOn || QuadScan.GetComponent<Toggle>().isOn || QuadSpiral.GetComponent<Toggle>().isOn))
                    BackButton.GetComponent<Button>().interactable = true;
                else
                    BackButton.GetComponent<Button>().interactable = false;
                break;
            case LevelsManager.Levels.L2:
                if ((UltraRight.GetComponent<Toggle>().isOn || UltraLeft.GetComponent<Toggle>().isOn || UltraRandom.GetComponent<Toggle>().isOn) &&
                    (QuadRandom.GetComponent<Toggle>().isOn || QuadScan.GetComponent<Toggle>().isOn || QuadSpiral.GetComponent<Toggle>().isOn))
                    BackButton.GetComponent<Button>().interactable = true;
                else
                    BackButton.GetComponent<Button>().interactable = false;
                break;
            case LevelsManager.Levels.L3:
                if ((HeavyRight.GetComponent<Toggle>().isOn || HeavyLeft.GetComponent<Toggle>().isOn || HeavyRandom.GetComponent<Toggle>().isOn) &&
                    (QuadRandom.GetComponent<Toggle>().isOn || QuadScan.GetComponent<Toggle>().isOn || QuadSpiral.GetComponent<Toggle>().isOn))
                    BackButton.GetComponent<Button>().interactable = true;
                else
                    BackButton.GetComponent<Button>().interactable = false;
                break;
            case LevelsManager.Levels.L4:
                if ((HeavyRight.GetComponent<Toggle>().isOn || HeavyLeft.GetComponent<Toggle>().isOn || HeavyRandom.GetComponent<Toggle>().isOn) &&
                    (QuadRandom.GetComponent<Toggle>().isOn || QuadScan.GetComponent<Toggle>().isOn || QuadSpiral.GetComponent<Toggle>().isOn) &&
                    (UltraRandom.GetComponent<Toggle>().isOn || UltraRight.GetComponent<Toggle>().isOn || UltraLeft.GetComponent<Toggle>().isOn))
                    BackButton.GetComponent<Button>().interactable = true;
                else
                    BackButton.GetComponent<Button>().interactable = false;
                break;
            default:
                break;
        }
    }
    void enableObject(GameObject obj, bool show){
        obj.SetActive(show);
    }
    public void onClickBack() {
        storeQuadConfiguration();
        storeLightConfiguration();
        storeHeavyConfiguration();
        storeUltraConfiguration();
        
        StartCoroutine(Fade.fadeInCoroutine(LayerFadeInOut, FadeInOutSpeed, backToLevel));
    }
    void backToLevel(){
        LevelsManager.ExitConfigMenu = true;
    }
    
    public void launchConfigLayer(){
        switch (LevelsManager.getCurrentLevel()){
            case LevelsManager.Levels.L1:
                loadLevel1Config();
                if (LightCurrentConfig == VehicleRobotConfiguration.LEFT) LightLeft.GetComponent<Toggle>().enabled = true;
                else if (LightCurrentConfig == VehicleRobotConfiguration.RIGHT) LightRight.GetComponent<Toggle>().enabled = true;
                else if (LightCurrentConfig == VehicleRobotConfiguration.RANDOM) LightRandom.GetComponent<Toggle>().enabled = true;
                if (QuadCurrentConfig == QuadRobotConfiguration.SPIRAL) QuadSpiral.GetComponent<Toggle>().enabled = true;
                else if (QuadCurrentConfig == QuadRobotConfiguration.SCAN) QuadScan.GetComponent<Toggle>().enabled = true;
                else if (QuadCurrentConfig == QuadRobotConfiguration.RANDOM) QuadRandom.GetComponent<Toggle>().enabled = true;
                break;
            case LevelsManager.Levels.L2:
                loadLevel2Config();
                if (UltraCurrentConfig == VehicleRobotConfiguration.LEFT) UltraLeft.GetComponent<Toggle>().enabled = true;
                else if (UltraCurrentConfig == VehicleRobotConfiguration.RIGHT) UltraRight.GetComponent<Toggle>().enabled = true;
                else if (UltraCurrentConfig == VehicleRobotConfiguration.RANDOM) UltraRandom.GetComponent<Toggle>().enabled = true;
                if (QuadCurrentConfig == QuadRobotConfiguration.SPIRAL) QuadSpiral.GetComponent<Toggle>().enabled = true;
                else if (QuadCurrentConfig == QuadRobotConfiguration.SCAN) QuadScan.GetComponent<Toggle>().enabled = true;
                else if (QuadCurrentConfig == QuadRobotConfiguration.RANDOM) QuadRandom.GetComponent<Toggle>().enabled = true;
                break;
            case LevelsManager.Levels.L3:
                loadLevel3Config();
                if (HeavyCurrentConfig == VehicleRobotConfiguration.LEFT) HeavyLeft.GetComponent<Toggle>().enabled = true;
                else if (HeavyCurrentConfig == VehicleRobotConfiguration.RIGHT) HeavyRight.GetComponent<Toggle>().enabled = true;
                else if (HeavyCurrentConfig == VehicleRobotConfiguration.RANDOM) HeavyRandom.GetComponent<Toggle>().enabled = true;
                if (QuadCurrentConfig == QuadRobotConfiguration.SPIRAL) QuadSpiral.GetComponent<Toggle>().enabled = true;
                else if (QuadCurrentConfig == QuadRobotConfiguration.SCAN) QuadScan.GetComponent<Toggle>().enabled = true;
                else if (QuadCurrentConfig == QuadRobotConfiguration.RANDOM) QuadRandom.GetComponent<Toggle>().enabled = true;
                break;
            case LevelsManager.Levels.L4:
                loadLevel4Config();
                if (HeavyCurrentConfig == VehicleRobotConfiguration.LEFT) HeavyLeft.GetComponent<Toggle>().enabled = true;
                else if (HeavyCurrentConfig == VehicleRobotConfiguration.RIGHT) HeavyRight.GetComponent<Toggle>().enabled = true;
                else if (HeavyCurrentConfig == VehicleRobotConfiguration.RANDOM) HeavyRandom.GetComponent<Toggle>().enabled = true;
                if (QuadCurrentConfig == QuadRobotConfiguration.SPIRAL) QuadSpiral.GetComponent<Toggle>().enabled = true;
                else if (QuadCurrentConfig == QuadRobotConfiguration.SCAN) QuadScan.GetComponent<Toggle>().enabled = true;
                else if (QuadCurrentConfig == QuadRobotConfiguration.RANDOM) QuadRandom.GetComponent<Toggle>().enabled = true;
                if (UltraCurrentConfig == VehicleRobotConfiguration.LEFT) UltraLeft.GetComponent<Toggle>().enabled = true;
                else if (UltraCurrentConfig == VehicleRobotConfiguration.RIGHT) UltraRight.GetComponent<Toggle>().enabled = true;
                else if (UltraCurrentConfig == VehicleRobotConfiguration.RANDOM) UltraRandom.GetComponent<Toggle>().enabled = true;
                break;
            default:
                break;
        }
        BackButtonText.GetComponent<Text>().text = "Go Back";
        LayerFadeInOut.GetComponent<Image>().color = Color.clear;
        LayerFadeInOut.SetActive(false);
    }

    public void loadLevel1Config(){
        enableObject(Quad, true);
        enableObject(Light, true);
        enableObject(Heavy, false);
        enableObject(Ultra, false);
    }
    public void loadLevel2Config(){
        enableObject(Quad, true);
        enableObject(Light, false);
        enableObject(Heavy, false);
        enableObject(Ultra, true);
    }
    public void loadLevel3Config(){
        enableObject(Quad, true);
        enableObject(Light, false);
        enableObject(Heavy, true);
        enableObject(Ultra, false);
    }
    public void loadLevel4Config(){
        enableObject(Quad, true);
        enableObject(Light, false);
        enableObject(Heavy, true);
        enableObject(Ultra, true);
    }
    void storeQuadConfiguration(){
        //QUAD
        if (QuadScan.GetComponent<Toggle>().isOn)
            QuadCurrentConfig = QuadRobotConfiguration.SCAN;
        else if (QuadRandom.GetComponent<Toggle>().isOn)
            QuadCurrentConfig = QuadRobotConfiguration.RANDOM;
        else if (QuadSpiral.GetComponent<Toggle>().isOn)
            QuadCurrentConfig = QuadRobotConfiguration.SPIRAL;
        else
            QuadCurrentConfig = QuadRobotConfiguration.UNKNOWN;
    }
    void storeLightConfiguration(){
        //LIGHT
        if (LightRight.GetComponent<Toggle>().isOn)
            LightCurrentConfig = VehicleRobotConfiguration.RIGHT;
        else if (LightLeft.GetComponent<Toggle>().isOn)
            LightCurrentConfig = VehicleRobotConfiguration.LEFT;
        else if (LightRandom.GetComponent<Toggle>().isOn)
            LightCurrentConfig = VehicleRobotConfiguration.RANDOM;
        else
            LightCurrentConfig = VehicleRobotConfiguration.UNKNOWN;
    }
    void storeHeavyConfiguration(){
        //HEAVY
        if (HeavyRight.GetComponent<Toggle>().isOn)
            HeavyCurrentConfig = VehicleRobotConfiguration.RIGHT;
        else if (HeavyLeft.GetComponent<Toggle>().isOn)
            HeavyCurrentConfig = VehicleRobotConfiguration.LEFT;
        else if (HeavyRandom.GetComponent<Toggle>().isOn)
            HeavyCurrentConfig = VehicleRobotConfiguration.RANDOM;
        else
            HeavyCurrentConfig = VehicleRobotConfiguration.UNKNOWN;
    }
    void storeUltraConfiguration(){
        //ULTRA
        if (UltraRight.GetComponent<Toggle>().isOn)
            UltraCurrentConfig = VehicleRobotConfiguration.RIGHT;
        else if (UltraLeft.GetComponent<Toggle>().isOn)
            UltraCurrentConfig = VehicleRobotConfiguration.LEFT;
        else if (UltraRandom.GetComponent<Toggle>().isOn)
            UltraCurrentConfig = VehicleRobotConfiguration.RANDOM;
        else
            UltraCurrentConfig = VehicleRobotConfiguration.UNKNOWN;
    }
}
