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
        LevelControllerObj.GetComponent<LevelController>().disableRobots();
        StartCoroutine(Fade.fadeInCoroutine(LayerFadeInOut, FadeInOutSpeed, changeRobotControl));
    }
    
    public void onClickConfigRobot(){
        StartCoroutine(Fade.fadeInCoroutine(LayerFadeInOut, FadeInOutSpeed, showConfigWindow));
    }
    
    void showConfigWindow(){
        LayerFadeInOut.SetActive(false);
        LevelControllerObj.GetComponent<LevelController>().showConfigWindow();
    }

    void changeRobotControl(){
        Debug.Log("Click on change Robot State");
        switch (LevelController.rstate){
            case LevelController.RobotState.DRONE:
                LevelControllerObj.GetComponent<LevelController>().enableVehicle();
                ActualRobotName.GetComponent<Text>().text = "Light Vehicle";
                LevelController.rstate = LevelController.RobotState.VEHICLE;
                VehicleFPSController.m_AutoWalkingState = VehicleFPSController.AutoWalkingState.Disabled;
                DroneFPSController.m_AutoWalkingState = DroneFPSController.AutoWalkingState.Starting;
                break;
            case LevelController.RobotState.VEHICLE:
                LevelControllerObj.GetComponent<LevelController>().enableDrone();
                ActualRobotName.GetComponent<Text>().text = "Quadcopter";
                LevelController.rstate = LevelController.RobotState.DRONE;
                DroneFPSController.m_AutoWalkingState = DroneFPSController.AutoWalkingState.Disabled;
                VehicleFPSController.m_AutoWalkingState = VehicleFPSController.AutoWalkingState.Starting;
                break;
            default:
                Debug.Log("Unknown Robot state.");
                LevelController.rstate = LevelController.RobotState.unknown;
                break;
        }
        StartCoroutine(Fade.fadeOutCoroutine(LayerFadeInOut, FadeInOutSpeed));
    }
	public void onClickMiniMap() {//We display the map with full size
		Debug.Log("Click on Mini Map");
		miniMap.SetActive(false);
		fullMap.SetActive(true);
		cancelFullMapButton.SetActive(true);
        LevelControllerObj.GetComponent<LevelController>().disableRobots();
	}

	public void onClickCancelFullMap() {//We display the map with full size
		Debug.Log("Click on Cancel Full Map");
		miniMap.SetActive(true);
		fullMap.SetActive(false);
		cancelFullMapButton.SetActive(false);
        LevelControllerObj.GetComponent<LevelController>().disableRobots();
        if (LevelController.rstate == LevelController.RobotState.DRONE) {
            LevelControllerObj.GetComponent<LevelController>().enableDrone();
		} else if (LevelController.rstate == LevelController.RobotState.VEHICLE) {
            LevelControllerObj.GetComponent<LevelController>().enableVehicle();
		}
	}
    public void loadActualRobotName(string RobotName) {
        ActualRobotName.GetComponent<Text>().text = RobotName;
    }
}
