using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {
	//Variables for map
	[SerializeField] private GameObject miniMap, fullMap, cancelFullMapButton;
    [SerializeField] private GameObject ActualRobotName;
    [SerializeField] private GameObject LevelControllerObj;

	public GameObject LayerFadeInOut;
	public float FadeInOutSpeed = 0;
    
	void Start () {	
        fullMap.SetActive(false);
		cancelFullMapButton.SetActive(false);
	}

    public void onClickChangeRobot() {//Change state
        LevelControllerObj.GetComponent<LevelController>().disableRobots(true);
        StartCoroutine(Fade.fadeInCoroutine(LayerFadeInOut, FadeInOutSpeed, changeRobotControl));
    }
    
    public void onClickConfigRobot(){
        StartCoroutine(Fade.fadeInCoroutine(LayerFadeInOut, FadeInOutSpeed, showConfigWindow));
    }
    
    void showConfigWindow(){
        LayerFadeInOut.GetComponent<Image>().color = Color.clear;
        LayerFadeInOut.SetActive(false);
        LevelControllerObj.GetComponent<LevelController>().showConfigWindow();
    }

    void changeRobotControl(){
        Debug.Log("Click on change Robot State");
        LevelControllerObj.GetComponent<LevelController>().changeActiveRobot();
        StartCoroutine(Fade.fadeOutCoroutine(LayerFadeInOut, FadeInOutSpeed));
    }
	public void onClickMiniMap() {//We display the map with full size
		Debug.Log("Click on Mini Map");
		miniMap.SetActive(false);
		fullMap.SetActive(true);
		cancelFullMapButton.SetActive(true);
        LevelControllerObj.GetComponent<LevelController>().disableRobots(false);
	}

	public void onClickCancelFullMap() {//We display the map with full size
		Debug.Log("Click on Cancel Full Map");
		miniMap.SetActive(true);
		fullMap.SetActive(false);
		cancelFullMapButton.SetActive(false);
        LevelControllerObj.GetComponent<LevelController>().loadActiveRobot();
	}
    public void loadActualRobotName(string RobotName) {
        ActualRobotName.GetComponent<Text>().text = RobotName;
    }
}
