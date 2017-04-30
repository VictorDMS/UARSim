using UnityEngine;

public class LevelController : MonoBehaviour {

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
    public enum RobotState{ DRONE, VEHICLE, unknown }
    static public RobotState rstate = RobotState.unknown;

    void Start () {        
        VehicleRenderTexture = vehicleCam.GetComponent<Camera>().targetTexture;
        DroneRenderTexture = droneCam.GetComponent<Camera>().targetTexture;
        LevelsManager.loadLevel();
    }
    void Update(){
        if (GlobalInformation.ExitConfigMenu){
            GlobalInformation.ExitConfigMenu = false;
            backFromConfig();
        }

        if(LevelsManager.LoadConfig){
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
            ViewVehicle.transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
            MoveVehicle.transform.localScale = new Vector3(0.0001f,0.0001f,0.0001f);
            MoveDrone.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else{
            ViewVehicle.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            MoveVehicle.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            MoveDrone.transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
        }

        if (!enable)
        {
            droneCam.GetComponent<Camera>().targetTexture = DroneRenderTexture;
            droneCam.GetComponent<Camera>().Render();
        }
        else
            droneCam.GetComponent<Camera>().targetTexture = null;
    }
    private void activateVehicle(bool enable){
        if (enable){
            ViewVehicle.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            MoveVehicle.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            MoveDrone.transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
        }else{
            ViewVehicle.transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
            MoveVehicle.transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
            MoveDrone.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

        if (!enable){
            vehicleCam.GetComponent<Camera>().targetTexture = VehicleRenderTexture;
            vehicleCam.GetComponent<Camera>().Render();
        }
        else
            vehicleCam.GetComponent<Camera>().targetTexture = null;
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
        ConfigWindow.SetActive(true);
        ConfigWindow.GetComponent<ConfigBehavior>().launchConfigLayer();
        HUDCanvasWindow.SetActive(false);
    }
    public void backFromConfig(){
        switch (rstate)
        {
            case RobotState.DRONE:
                enableDrone();break;
            case RobotState.VEHICLE:
                enableVehicle(); break;
            case RobotState.unknown:
            default:
                break;
        }
        if(ConfigWindow.GetComponent<ConfigBehavior>().FirstTimeShown == true){
            ConfigWindow.GetComponent<ConfigBehavior>().FirstTimeShown = false;
            Stopwatch.GetComponent<StopwatchUpdater>().startStopwatch();
            enableVehicle();
            rstate = RobotState.VEHICLE;
            HUDCanvasWindow.GetComponent<HUDManager>().loadActualRobotName("Light Vehicle");
            VehicleFPSController.m_AutoWalkingState = VehicleFPSController.AutoWalkingState.Disabled;
            DroneFPSController.m_AutoWalkingState = DroneFPSController.AutoWalkingState.Starting;
        }
        ConfigWindow.SetActive(false);
        HUDCanvasWindow.SetActive(true);
        HUDCanvasWindow.GetComponent<HUDManager>().onClickCancelFullMap();
    }

    public void showScoreWindow(){
        disableRobots();
        ScoreWindow.SetActive(true);
        ScoreWindow.GetComponent<ScoreBehavior>().launchScoreLayer();
        HUDCanvasWindow.SetActive(false);
    }
    public void backFromScore(){
        LevelsManager.loadLevel();
    }

    public void showNewLevelWindow(){
        switch (LevelsManager.getCurrentLevel()){
            case LevelsManager.Levels.L1:
                MazeGenerator.GetComponent<MazeSpawner>().loadMaze(LevelsManager.MaxPoints_L1);
                break;
            case LevelsManager.Levels.L2:
                MazeGenerator.GetComponent<MazeSpawner>().loadMaze(LevelsManager.MaxPoints_L2);
                break;                                             
            case LevelsManager.Levels.L3:                          
                MazeGenerator.GetComponent<MazeSpawner>().loadMaze(LevelsManager.MaxPoints_L3);
                break;                                             
            case LevelsManager.Levels.L4:                          
                MazeGenerator.GetComponent<MazeSpawner>().loadMaze(LevelsManager.MaxPoints_L4);
                break;
            case LevelsManager.Levels.End:
            case LevelsManager.Levels.Start:
            default:
                break;
        }
    }
}
