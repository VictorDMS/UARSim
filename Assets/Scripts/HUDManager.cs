using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

[DynamoDBTable("VDMSTable")]
public class VDMSTable
{
    [DynamoDBHashKey]// Hash key.
    public string Description { get; set; }

    [DynamoDBHashKey]// Hash key.
    public string EventType { get; set; }

    [DynamoDBHashKey]// Hash key.
    public string LoQueQuiera { get; set; }
}

public class HUDManager : MonoBehaviour {
	[SerializeField] private GameObject drone;
	[SerializeField] private GameObject vehicle;
	[SerializeField] private GameObject droneDualC;
	[SerializeField] private GameObject vehicleDualC;
	[SerializeField] private Camera droneCam;
	[SerializeField] private Camera vehicleCam;
	//Variables for map
	[SerializeField] private GameObject miniMap, fullMap, cancelFullMapButton;
    [SerializeField] private GameObject ActualRobotName;

	private RenderTexture DroneRenderTexture;
	private RenderTexture VehicleRenderTexture;
	private RobotState rstate = RobotState.unknown;
	public GameObject LayerFadeInOut;
	public float FadeInOutSpeed = 0;

    private enum OnClickActionEnum { ChangeRobot, OpenConfig, unknown };
    private OnClickActionEnum OnClickAction = OnClickActionEnum.unknown;

	private enum RobotState{
		DRONE,
		VEHICLE,
		unknown
	}

	// Use this for initialization
	void Start () {	
		VehicleRenderTexture = vehicleCam.GetComponent<Camera> ().targetTexture;
		DroneRenderTexture = droneCam.GetComponent<Camera> ().targetTexture;

		activateViewGameObjects(drone, droneDualC, droneCam, DroneRenderTexture, false);
		activateViewGameObjects (vehicle, vehicleDualC, vehicleCam, VehicleRenderTexture, false);
		activateViewGameObjects (vehicle, vehicleDualC, vehicleCam, VehicleRenderTexture, true);
        ActualRobotName.GetComponent<Text>().text = "Light Vehicle";

        fullMap.SetActive(false);
		cancelFullMapButton.SetActive(false);
		rstate = RobotState.VEHICLE;
	}
    //void SyncSuccessCallback(object sender, SyncSuccessEvent e){
    //    // Your handler code here
    //}

    public void onClickChangeRobot() {//Change state
        activateViewGameObjects(drone, droneDualC, droneCam, DroneRenderTexture, false);
        activateViewGameObjects(vehicle, vehicleDualC, vehicleCam, VehicleRenderTexture, false);
        OnClickAction = OnClickActionEnum.ChangeRobot;
        StartCoroutine(fadeInCoroutine());

        //TEST
        UnityInitializer.AttachToGameObject(this.gameObject);
        print("Attached");
        var credentials = new CognitoAWSCredentials("xxxxxxx",
                            "eu - central - 1_QrjYxJ01d",
                            "arn:aws:cognito-idp:eu-central-1:624311491617:userpool/eu-central-1_QrjYxJ01d",
                            "arn:aws:cognito-idp:eu-central-1:624311491617:userpool/eu-central-1_QrjYxJ01d", RegionEndpoint.EUCentral1);

        //("eu-central-1_QrjYxJ01d", RegionEndpoint.EUCentral1);
        print("Credentials");
        AmazonDynamoDBClient client = new AmazonDynamoDBClient(credentials);
        print("Client");
        DynamoDBContext Context = new DynamoDBContext(client);

        VDMSTable testLog = new VDMSTable
        {
            Description = "Good luck que se traza",
            EventType = "Para tipo el que tengo malito",
            LoQueQuiera = "Good luck elevado al cubo"
        };

        // Save the book.
        string resultText = "Commit NOT done :(";
        Context.SaveAsync(testLog, (result) => {
            if (result.Exception == null)
                resultText = "Commit Done. :)";
        });

        print(resultText);
        
        //CognitoAWSCredentials credentials = new CognitoAWSCredentials(
        //                                    "us-west-2:438781bf-72ab-415e-8829-d83c053416f0", // Identity Pool ID
        //                                    RegionEndpoint.USWest2); // Region

        //// Initialize the Cognito Sync client
        //CognitoSyncManager syncManager = new CognitoSyncManager(credentials,
        //    new AmazonCognitoSyncConfig { RegionEndpoint = RegionEndpoint.USWest2 });// Region

        //// Create a record in a dataset and synchronize with the server
        //Dataset dataset = syncManager.OpenOrCreateDataset("myDataset");
        ////dataset.OnSyncSuccess += SyncSuccessCallback;
        //dataset.OnSyncSuccess += SyncSuccessCallback;
        //dataset.Put("myKey", "myValue");
        //dataset.SynchronizeAsync();
    }
    

    public void onClickConfigRobot() {
        activateViewGameObjects(drone, droneDualC, droneCam, DroneRenderTexture, false);
        activateViewGameObjects(vehicle, vehicleDualC, vehicleCam, VehicleRenderTexture, false);
        OnClickAction = OnClickActionEnum.OpenConfig;
        StartCoroutine(fadeInCoroutine());
    }

    void executeActionOnClick() {
        switch (OnClickAction) {
            case OnClickActionEnum.ChangeRobot:
                changeRobotControl(); break;
            case OnClickActionEnum.OpenConfig:
                SceneManager.LoadScene("Config"); break;
            case OnClickActionEnum.unknown:
            default:
                break;
        }
    }

    IEnumerator fadeInCoroutine(){
        LayerFadeInOut.SetActive(true);
        Image ImageFadeInOut = LayerFadeInOut.GetComponent<Image>();
        for (;;){
            ImageFadeInOut.color = Color.Lerp(ImageFadeInOut.color, Color.black, (FadeInOutSpeed / 2) * Time.deltaTime);
            if (ImageFadeInOut.color.a >= 0.95f){
                ImageFadeInOut.color = Color.black;
                executeActionOnClick();
                break;
            }else{
                yield return null;
            }
        }
        print("Process \"fade in\" is done.");
    }

    IEnumerator fadeOutCoroutine(){
        LayerFadeInOut.SetActive(true);
        Image ImageFadeInOut = LayerFadeInOut.GetComponent<Image>();
        for (;;){
            ImageFadeInOut.color = Color.Lerp(ImageFadeInOut.color, Color.clear, (FadeInOutSpeed / 2) * Time.deltaTime);
            if (ImageFadeInOut.color.a <= 0.05f){
                ImageFadeInOut.color = Color.clear;
                break;
            }else{
                yield return null;
            }
        }
        print("Process \"fade out\" is done.");
        LayerFadeInOut.SetActive(false);
    }

    void changeRobotControl(){
        Debug.Log("Click on change Robot State");
        switch (rstate){
            case RobotState.DRONE:
                activateViewGameObjects(drone, droneDualC, droneCam, DroneRenderTexture, false);
                activateViewGameObjects(vehicle, vehicleDualC, vehicleCam, VehicleRenderTexture, false);
                activateViewGameObjects(vehicle, vehicleDualC, vehicleCam, VehicleRenderTexture, true);
                ActualRobotName.GetComponent<Text>().text = "Light Vehicle";
                rstate = RobotState.VEHICLE;
                break;
            case RobotState.VEHICLE:
                activateViewGameObjects(drone, droneDualC, droneCam, DroneRenderTexture, false);
                activateViewGameObjects(vehicle, vehicleDualC, vehicleCam, VehicleRenderTexture, false);
                activateViewGameObjects(drone, droneDualC, droneCam, DroneRenderTexture, true);
                ActualRobotName.GetComponent<Text>().text = "Quadcopter";
                rstate = RobotState.DRONE;
                break;
            default:
                Debug.Log("Unknown Robot state.");
                rstate = RobotState.unknown;
                break;
        }
        StartCoroutine(fadeOutCoroutine());
    }

	public void onClickMiniMap() {//We display the map with full size
		Debug.Log("Click on Mini Map");
		miniMap.SetActive(false);
		fullMap.SetActive(true);
		cancelFullMapButton.SetActive(true);
		activateViewGameObjects (drone, droneDualC, droneCam, DroneRenderTexture, false);
		activateViewGameObjects (vehicle, vehicleDualC, vehicleCam, VehicleRenderTexture, false);
	}

	public void onClickCancelFullMap() {//We display the map with full size
		Debug.Log("Click on Cancel Full Map");
		miniMap.SetActive(true);
		fullMap.SetActive(false);
		cancelFullMapButton.SetActive(false);
		if (rstate == RobotState.DRONE) {
			activateViewGameObjects (drone, droneDualC, droneCam, DroneRenderTexture, true);
		} else if (rstate == RobotState.VEHICLE) {
			activateViewGameObjects (vehicle, vehicleDualC, vehicleCam, VehicleRenderTexture, true);
		}
	}

	private void activateViewGameObjects (GameObject obj, GameObject ctrl, Camera cam, RenderTexture texture, bool enable){
		obj.GetComponent<CharacterController>().enabled = enable;
		obj.GetComponent<FirstPersonController>().enabled = enable;
	
		ctrl.SetActive (enable);

		if (!enable) {
			cam.GetComponent<Camera>().targetTexture = texture;
			cam.GetComponent<Camera> ().Render();
		} else {
			cam.GetComponent<Camera> ().targetTexture = null;
		}
	}
}
