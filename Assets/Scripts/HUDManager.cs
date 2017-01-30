using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput.PlatformSpecific;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {
	[SerializeField] private GameObject drone;
	[SerializeField] private GameObject vehicle;
	[SerializeField] private GameObject droneDualC;
	[SerializeField] private GameObject vehicleDualC;
	[SerializeField] private Camera droneCam;
	[SerializeField] private Camera vehicleCam;

	//Variables for map
	[SerializeField] private GameObject miniMap, fullMap;
	private RenderTexture DroneRenderTexture;
	private RenderTexture VehicleRenderTexture;

	private RobotState rstate = RobotState.unknown;
	public GameObject LayerFadeInOut;
	public float FadeInOutSpeed = 0;

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

		activateMapGameObjects (fullMap, false);
		rstate = RobotState.VEHICLE;
	}
	
	// Update is called once per frame
	void Update () { }

	public void onClickChangeRobot() {//Change state	
		StartCoroutine(fadeInAndOutCoroutine());
		StartCoroutine(changeRobotCoroutine());
	}

	IEnumerator changeRobotCoroutine(){
		Debug.Log("Click on change Robot State");
		switch (rstate) {
		case RobotState.DRONE:
			activateViewGameObjects (drone, droneDualC, droneCam, DroneRenderTexture, false);
			activateViewGameObjects (vehicle, vehicleDualC, vehicleCam, VehicleRenderTexture, false);
			yield return new WaitForSeconds (0.35f);
			activateViewGameObjects (vehicle, vehicleDualC, vehicleCam, VehicleRenderTexture, true);
			rstate = RobotState.VEHICLE;
			break;
		case RobotState.VEHICLE:
			activateViewGameObjects (drone, droneDualC, droneCam, DroneRenderTexture, false);
			activateViewGameObjects (vehicle, vehicleDualC, vehicleCam, VehicleRenderTexture, false);
			yield return new WaitForSeconds (0.35f);
			activateViewGameObjects (drone, droneDualC, droneCam, DroneRenderTexture, true);
			rstate = RobotState.DRONE;
			break;
		default:
			Debug.Log ("Unknown Robot state.");
			rstate = RobotState.unknown;
			break;
		}
	}

	IEnumerator fadeInAndOutCoroutine(){
		print ("fading in and out...");
		LayerFadeInOut.SetActive (true);
		print ("LayerFadeInOut is: " + LayerFadeInOut.activeSelf);
		Image ImageFadeInOut = LayerFadeInOut.GetComponent<Image> ();
		for (;;) {
			ImageFadeInOut.color = Color.Lerp (ImageFadeInOut.color, Color.black, (FadeInOutSpeed/2) * Time.deltaTime);

			if (ImageFadeInOut.color.a >= 0.95f) {
				for (;;) {
					ImageFadeInOut.color = Color.Lerp (ImageFadeInOut.color, Color.clear, (FadeInOutSpeed/2) * Time.deltaTime);
					if (ImageFadeInOut.color.a <= 0.05f) {
						ImageFadeInOut.color = Color.clear;
						break;
					} else {
						print ("Yielding in fade out loop");
						yield return null;
					}
				}
				break;
			} else {
				print ("Yielding in fade in loop");
				yield return null;
			}
		}
		print ("Process is done.");
		LayerFadeInOut.SetActive (false);
	}

	public void onClickMiniMap() {//We display the map with full size
		Debug.Log("Click on Mini Map");
		//miniMap [0].GetComponent<RectTransform> ().anchorMin 	= new Vector2(0.5f, 0.5f);
		//miniMap [0].GetComponent<RectTransform> ().anchorMax 	= new Vector2(0.5f, 0.5f);
		//miniMap [0].GetComponent<RectTransform> ().pivot 		= new Vector2(0.5f, 0.5f);
		//miniMap [0].GetComponent<RectTransform> ().localScale 	= new Vector3(2.5f, 2.5f, 2.5f);
		//miniMap [0].GetComponent<RectTransform> ().position 	= new Vector3(0, -20, 0);
		activateMapGameObjects (miniMap, false);
		activateMapGameObjects (fullMap, true);
		activateViewGameObjects (drone, droneDualC, droneCam, DroneRenderTexture, false);
		activateViewGameObjects (vehicle, vehicleDualC, vehicleCam, VehicleRenderTexture, false);
	}

	public void onClickCancelFullMap() {//We display the map with full size
		Debug.Log("Click on Cancel Full Map");

		//miniMap [0].GetComponent<RectTransform> ().anchorMin 	= new Vector2(0, 1);
		//miniMap [0].GetComponent<RectTransform> ().anchorMax 	= new Vector2(0, 1);
		//miniMap [0].GetComponent<RectTransform> ().pivot 		= new Vector2(0, 1);
		//miniMap [0].GetComponent<RectTransform> ().localScale 	= new Vector3(1, 1, 1);
		//miniMap [0].GetComponent<RectTransform> ().position 	= new Vector3(20, -20, 0);
		activateMapGameObjects (miniMap, true);
		activateMapGameObjects (fullMap, false);

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
		//cam.enabled = enable;
	}

	private void activateMapGameObjects (GameObject obj, bool enable){
		Debug.Log ("activating: " + obj.name + " to: " + (enable? "true" : "false"));
		obj.SetActive(enable);
	}
}
