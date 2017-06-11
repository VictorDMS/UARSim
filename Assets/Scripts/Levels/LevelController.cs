using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour {

    [SerializeField]private GameObject drone;
    [SerializeField]private GameObject vehicle;
    [SerializeField]private GameObject Stopwatch;
    [SerializeField]private GameObject ConfigWindow;
    [SerializeField]private GameObject ScoreWindow;
    [SerializeField]private GameObject MazeGenerator;
    [SerializeField]private GameObject HUDCanvasWindow;
    [SerializeField]private Camera droneCam;
    [SerializeField]private Camera vehicleCam;
    
    [SerializeField]private GameObject ViewVehicle;
    [SerializeField]private GameObject MoveVehicle;
    [SerializeField]private GameObject MoveDrone;

    private RenderTexture DroneRenderTexture;
    private RenderTexture VehicleRenderTexture;

    void Start () {        
        VehicleRenderTexture = vehicleCam.GetComponent<Camera>().targetTexture;
        DroneRenderTexture = droneCam.GetComponent<Camera>().targetTexture;
        LevelsManager.loadNewLevel();//Here is loaded Level 1.
        ScoreWindow.SetActive(false);
    }
    void Update(){
        if (LevelsManager.ExitConfigMenu){
            LevelsManager.ExitConfigMenu = false;
            backFromConfig();
        }
        if (LevelsManager.ExitScoreMenu){
            LevelsManager.ExitScoreMenu = false;
            backFromScore();
        }

        if (LevelsManager.LoadConfig){
            showConfigWindow();
            LevelsManager.LoadConfig = false;
        }
        else if (LevelsManager.LoadScore){
            showScoreWindow();
            LevelsManager.LoadScore = false;
        }
        else if (LevelsManager.LoadLevel){
            showNewLevelWindow();
            LevelsManager.LoadLevel = false;
        }
    }

    private void activateDrone(bool enable){
        if (enable){
            MoveDrone.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            if (drone.GetComponent<DroneFPSController>().m_WalkSpeed == DroneFPSController.Speed.NORMAL_SPEED){
                HUDCanvasWindow.GetComponent<HUDManager>().loadActualRobotName("Normal Drone");
            }else if (drone.GetComponent<DroneFPSController>().m_WalkSpeed == DroneFPSController.Speed.SLOW_SPEED){
                HUDCanvasWindow.GetComponent<HUDManager>().loadActualRobotName("Slow Drone");
            }else if (drone.GetComponent<DroneFPSController>().m_WalkSpeed == DroneFPSController.Speed.FAST_SPEED){
                HUDCanvasWindow.GetComponent<HUDManager>().loadActualRobotName("Fast Drone");
            }
            
            DroneFPSController.m_AutoWalkingState = DroneFPSController.AutoWalkingState.Disabled;
            VehicleFPSController.m_AutoWalkingState = VehicleFPSController.AutoWalkingState.Starting;
            ConfigBehavior.AutoConfiguration = ConfigBehavior.RobotTypes.LIGHT;

            droneCam.GetComponent<Camera>().targetTexture = null;
            if (LevelsManager.ShowFirstTimeControlImageDrone){
                StartCoroutine(Timer.launchActionWithDelay(5, removeDroneControlImage));
                LevelsManager.ShowFirstTimeControlImageDrone = false;
            }
        }
        else{
            MoveDrone.transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);

            droneCam.GetComponent<Camera>().targetTexture = DroneRenderTexture;
            droneCam.GetComponent<Camera>().Render();
        }
    }
    private void activateVehicle(bool enable){
        if (enable){
            ViewVehicle.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            MoveVehicle.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            if (vehicle.GetComponent<VehicleFPSController>().m_WalkSpeed == VehicleFPSController.Speed.LIGHT_SPEED) {
                HUDCanvasWindow.GetComponent<HUDManager>().loadActualRobotName("Light Vehicle");
            }else if (vehicle.GetComponent<VehicleFPSController>().m_WalkSpeed == VehicleFPSController.Speed.HEAVY_SPEED) {
                HUDCanvasWindow.GetComponent<HUDManager>().loadActualRobotName("Heavy Vehicle");
            } else if (vehicle.GetComponent<VehicleFPSController>().m_WalkSpeed == VehicleFPSController.Speed.ULTRA_SPEED) {
                HUDCanvasWindow.GetComponent<HUDManager>().loadActualRobotName("Ultra Light Vehicle");
            }
            VehicleFPSController.m_AutoWalkingState = VehicleFPSController.AutoWalkingState.Disabled;
            DroneFPSController.m_AutoWalkingState = DroneFPSController.AutoWalkingState.Starting;
            ConfigBehavior.AutoConfiguration = ConfigBehavior.RobotTypes.DRONE;

            vehicleCam.GetComponent<Camera>().targetTexture = null;

            if (LevelsManager.ShowFirstTimeControlImageVehicle){
                StartCoroutine(Timer.launchActionWithDelay(5, removeVehicleControlImage));
                LevelsManager.ShowFirstTimeControlImageVehicle = false;
            }
        }else{
            ViewVehicle.transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
            MoveVehicle.transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);

            vehicleCam.GetComponent<Camera>().targetTexture = VehicleRenderTexture;
            vehicleCam.GetComponent<Camera>().Render();
        }
    }
    public void disableRobots(bool Both){
        if (Both){
            activateDrone(false);
            activateVehicle(false);
        }
        else{
            switch (ConfigBehavior.AutoConfiguration){
                case ConfigBehavior.RobotTypes.DRONE:
                    activateVehicle(false);
                    break;
                case ConfigBehavior.RobotTypes.LIGHT:
                case ConfigBehavior.RobotTypes.HEAVY:
                case ConfigBehavior.RobotTypes.ULTRA:
                    activateDrone(false);
                    break;
            }
        }        
    }
    public void enableDrone(){
        activateVehicle(false);
        activateDrone(true);
    }
    public void enableVehicle() {
        activateDrone(false);
        activateVehicle(true);
    }

    public void showConfigWindow(){
        disableRobots(true);
        ScoreWindow.SetActive(false);
        ConfigWindow.SetActive(true);
        ConfigWindow.GetComponent<ConfigBehavior>().launchConfigLayer();
        HUDCanvasWindow.SetActive(false);
    }
    public void backFromConfig(){
        HUDCanvasWindow.SetActive(true);

        if (LevelsManager.FirstTimeNewLevel == true){
            LevelsManager.FirstTimeNewLevel = false;
            Stopwatch.GetComponent<StopwatchUpdater>().startStopwatch();
        }

        loadActiveRobot();
        ConfigWindow.SetActive(false);
        HUDCanvasWindow.GetComponent<HUDManager>().onClickCancelFullMap();
    }

    public void showScoreWindow(){
        disableRobots(true);
        ConfigWindow.SetActive(false);
        ScoreWindow.SetActive(true);
        ScoreWindow.GetComponent<ScoreBehavior>().launchScoreLayer();
        HUDCanvasWindow.SetActive(false);
    }
    public void backFromScore(){
        if ((LevelsManager.getCurrentLevel() == LevelsManager.Levels.End))
            LevelsManager.GameOver(); //End of the game
        else
            LevelsManager.loadNewLevel();
    }
    public void showNewLevelWindow(){
        vehicle.GetComponent<VehicleFPSController>().resetPosition();
        drone.GetComponent<DroneFPSController>().resetPosition();
        switch (LevelsManager.getCurrentLevel()){
            case LevelsManager.Levels.L1:
                MazeGenerator.GetComponent<MazeSpawner>().loadMaze(LevelsManager.MaxPoints_L1);
                vehicle.GetComponent<VehicleFPSController>().loadLevel1Params();
                drone.GetComponent<DroneFPSController>().loadLevel1Params();
                break;
            case LevelsManager.Levels.L2:
                MazeGenerator.GetComponent<MazeSpawner>().loadMaze(LevelsManager.MaxPoints_L2);
                vehicle.GetComponent<VehicleFPSController>().loadLevel2Params();
                drone.GetComponent<DroneFPSController>().loadLevel2Params();
                break;                                             
            case LevelsManager.Levels.L3:                          
                MazeGenerator.GetComponent<MazeSpawner>().loadMaze(LevelsManager.MaxPoints_L3);
                vehicle.GetComponent<VehicleFPSController>().loadLevel3Params();
                drone.GetComponent<DroneFPSController>().loadLevel3Params();
                break;                                             
            case LevelsManager.Levels.L4:                          
                MazeGenerator.GetComponent<MazeSpawner>().loadMaze(LevelsManager.MaxPoints_L4);
                vehicle.GetComponent<VehicleFPSController>().loadLevel4Params();
                drone.GetComponent<DroneFPSController>().loadLevel4Params();
                break;
            case LevelsManager.Levels.End:
            case LevelsManager.Levels.Start:
            default:
                break;
        }
    }
    public void loadActiveRobot(){
        switch (ConfigBehavior.AutoConfiguration){
            case ConfigBehavior.RobotTypes.DRONE:
                enableVehicle(); break;
            case ConfigBehavior.RobotTypes.LIGHT:
            case ConfigBehavior.RobotTypes.HEAVY:
            case ConfigBehavior.RobotTypes.ULTRA:
                enableDrone(); break;
            case ConfigBehavior.RobotTypes.UNKNOWN:
            default:
                break;
        }
    }
    public void changeActiveRobot(){
        switch (ConfigBehavior.AutoConfiguration){
            case ConfigBehavior.RobotTypes.DRONE:
                enableDrone(); break;
            case ConfigBehavior.RobotTypes.LIGHT:
            case ConfigBehavior.RobotTypes.HEAVY:
            case ConfigBehavior.RobotTypes.ULTRA:
                enableVehicle(); break;
            case ConfigBehavior.RobotTypes.UNKNOWN:
            default:
                break;
        }
    }

    public void removeDroneControlImage(){
        MoveDrone.GetComponent<Image>().color = Color.clear;
    }
    public void removeVehicleControlImage(){
        MoveVehicle.GetComponent<Image>().color = Color.clear;
        ViewVehicle.GetComponent<Image>().color = Color.clear;
    }
}
