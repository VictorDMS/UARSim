using UnityEngine;

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
    private enum RobotState{ DRONE, VEHICLE, unknown }
    private RobotState rstate = RobotState.unknown;

    void Start () {        
        VehicleRenderTexture = vehicleCam.GetComponent<Camera>().targetTexture;
        DroneRenderTexture = droneCam.GetComponent<Camera>().targetTexture;
        LevelsManager.loadLevel();//Here is loaded Level 1.
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
            HUDCanvasWindow.GetComponent<HUDManager>().loadActualRobotName("Quadcopter");
            DroneFPSController.m_AutoWalkingState = DroneFPSController.AutoWalkingState.Disabled;
            VehicleFPSController.m_AutoWalkingState = VehicleFPSController.AutoWalkingState.Starting;
            rstate = RobotState.DRONE;

            droneCam.GetComponent<Camera>().targetTexture = null;
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
            HUDCanvasWindow.GetComponent<HUDManager>().loadActualRobotName("Light Vehicle");
            VehicleFPSController.m_AutoWalkingState = VehicleFPSController.AutoWalkingState.Disabled;
            DroneFPSController.m_AutoWalkingState = DroneFPSController.AutoWalkingState.Starting;
            rstate = RobotState.VEHICLE;

            vehicleCam.GetComponent<Camera>().targetTexture = null;
        }
        else{
            ViewVehicle.transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
            MoveVehicle.transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);

            vehicleCam.GetComponent<Camera>().targetTexture = VehicleRenderTexture;
            vehicleCam.GetComponent<Camera>().Render();
        }
    }
    public void disableRobots(){
        activateDrone(false);
        activateVehicle(false);
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
        disableRobots();
        ScoreWindow.SetActive(false);
        ConfigWindow.SetActive(true);
        ConfigWindow.GetComponent<ConfigBehavior>().launchConfigLayer();
        HUDCanvasWindow.SetActive(false);
    }
    public void backFromConfig(){
        switch (rstate){
            case RobotState.DRONE:
                enableDrone();break;
            case RobotState.VEHICLE:
                enableVehicle(); break;
            case RobotState.unknown:
            default:
                break;
        }

        HUDCanvasWindow.SetActive(true);
        if (LevelsManager.FirstTimeNewLevel == true){
            LevelsManager.FirstTimeNewLevel = false;
            Stopwatch.GetComponent<StopwatchUpdater>().startStopwatch();
            enableVehicle();
            rstate = RobotState.VEHICLE;
            VehicleFPSController.m_AutoWalkingState = VehicleFPSController.AutoWalkingState.Disabled;
            DroneFPSController.m_AutoWalkingState = DroneFPSController.AutoWalkingState.Starting;
        }
        ConfigWindow.SetActive(false);
        HUDCanvasWindow.GetComponent<HUDManager>().onClickCancelFullMap();
    }

    public void showScoreWindow(){
        disableRobots();
        ConfigWindow.SetActive(false);
        ScoreWindow.SetActive(true);
        ScoreWindow.GetComponent<ScoreBehavior>().launchScoreLayer();
        HUDCanvasWindow.SetActive(false);
    }
    public void backFromScore(){
        LevelsManager.LoadConfig = true;
    }
    public void showNewLevelWindow(){
        switch (LevelsManager.getCurrentLevel()){
            case LevelsManager.Levels.L1:
                MazeGenerator.GetComponent<MazeSpawner>().loadMaze(LevelsManager.MaxPoints_L1);
                vehicle.GetComponent<VehicleFPSController>().resetPosition();
                drone.GetComponent<DroneFPSController>().resetPosition();
                break;
            case LevelsManager.Levels.L2:
                MazeGenerator.GetComponent<MazeSpawner>().loadMaze(LevelsManager.MaxPoints_L2);
                vehicle.GetComponent<VehicleFPSController>().resetPosition();
                drone.GetComponent<DroneFPSController>().resetPosition();
                break;                                             
            case LevelsManager.Levels.L3:                          
                MazeGenerator.GetComponent<MazeSpawner>().loadMaze(LevelsManager.MaxPoints_L3);
                vehicle.GetComponent<VehicleFPSController>().resetPosition();
                drone.GetComponent<DroneFPSController>().resetPosition();
                break;                                             
            case LevelsManager.Levels.L4:                          
                MazeGenerator.GetComponent<MazeSpawner>().loadMaze(LevelsManager.MaxPoints_L4);
                vehicle.GetComponent<VehicleFPSController>().resetPosition();
                drone.GetComponent<DroneFPSController>().resetPosition();
                break;
            case LevelsManager.Levels.End:
            case LevelsManager.Levels.Start:
            default:
                break;
        }
    }
    public void loadActiveRobot(){
        disableRobots();
        if (rstate == RobotState.DRONE){
            enableDrone();
        }else if (rstate == RobotState.VEHICLE){
            enableVehicle();
        }
    }
    public void changeActiveRobot(){
        switch (rstate){
            case RobotState.DRONE:
                enableVehicle();
                break;
            case RobotState.VEHICLE:
                enableDrone();
                break;
            default:
                Debug.Log("Unknown Robot state.");
                rstate = RobotState.unknown;
                break;
        }
    }
}
