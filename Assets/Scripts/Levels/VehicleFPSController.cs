using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;
using UnityStandardAssets.Characters.FirstPerson;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class VehicleFPSController : MonoBehaviour
{
    [SerializeField]private MouseLook m_MouseLook;
    [SerializeField]private float m_StepInterval;
    [SerializeField]private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
    [SerializeField]public VehicleAutomaticMovement AutoMov;
    public enum Speed { LIGHT_SPEED = 3, HEAVY_SPEED = 2, ULTRA_SPEED = 6/*8*/ } ;
    public Speed m_WalkSpeed;

    private Camera m_Camera;
    private Vector2 m_Input;
    private Vector3 m_MoveDir = Vector3.zero;
    private CharacterController m_CharacterController;
    private CollisionFlags m_CollisionFlags;
    private float m_StepCycle;
    private float m_NextStep;
    private AudioSource m_AudioSource;

    public enum AutoWalkingState {     Starting, Rotating, WalkingStraight, Disabled, unknown    };
    public static AutoWalkingState m_AutoWalkingState;
    private bool CurrentlyTouchingMoveLog = false, CurrentlyTouchingLookLog = false;
    private const float DELAY_FOR_LOG_DATA = 0.3f;
    private float HorizontalMov = 0.0f, VerticalMov = 0.0f, MouseLookX = 0.0f;

    // Use this for initialization
    private void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_Camera = Camera.main;
        m_StepCycle = 0f;
        m_NextStep = m_StepCycle / 2f;
        m_AudioSource = GetComponent<AudioSource>();
        m_MouseLook.Init(transform, m_Camera.transform);
        m_AutoWalkingState = AutoWalkingState.Disabled;
        StartCoroutine(movementLogger());
    }

    // Update is called once per frame
    private void Update(){
        RotateView();
        //m_MoveDir.y = 0f;
    }
    
    private void FixedUpdate()
    {
        GetInput();
        // always move along the camera forward as it is the direction that it being aimed at
        Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;

        // get a normal for the surface that is being touched to move along it
        RaycastHit hitInfo;
        Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                            m_CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

        m_MoveDir.x = desiredMove.x * (float)m_WalkSpeed;
        m_MoveDir.z = desiredMove.z * (float)m_WalkSpeed;
        if(gameObject.transform.position.y > 1.1f)
            m_MoveDir.y = -1 * (float)m_WalkSpeed;
        else if (gameObject.transform.position.y < 1.1f)
            m_MoveDir.y = 1 * (float)m_WalkSpeed;
        else 
            m_MoveDir.y = 0;

        m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);
        ProgressStepCycle((float)m_WalkSpeed);
        m_MouseLook.UpdateCursorLock();
    }

    private void ProgressStepCycle(float speed)
    {
        if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
        {
            m_StepCycle += (m_CharacterController.velocity.magnitude + (speed * 1f)) *
                            Time.fixedDeltaTime;
        }

        if (!(m_StepCycle > m_NextStep))
        {
            return;
        }

        m_NextStep = m_StepCycle + m_StepInterval;

        PlayFootStepAudio();
    }

    private void PlayFootStepAudio()
    {
        //if (!m_CharacterController.isGrounded)
        //{
        //    return;
        //}
        //// pick & play a random footstep sound from the array,
        //// excluding sound at index 0
        //int n = Random.Range(1, m_FootstepSounds.Length);
        //m_AudioSource.clip = m_FootstepSounds[n];
        //m_AudioSource.PlayOneShot(m_AudioSource.clip);
        //// move picked sound to index 0 so it's not picked next time
        //m_FootstepSounds[n] = m_FootstepSounds[0];
        //m_FootstepSounds[0] = m_AudioSource.clip;
    }

    private void GetInput()
    {
        // Read input
        float horizontal = 0f, vertical = 0f;
        getHorizontalAndVertical(ref horizontal, ref vertical);

        // set the desired speed to be walking or running
        m_Input = new Vector2(horizontal, vertical);

        // normalize input if it exceeds 1 in combined length:
        if (m_Input.sqrMagnitude > 1){
            m_Input.Normalize();
        }
    }

    private void RotateView(){
        float MouseXDist = 0.0f;
        switch (m_AutoWalkingState)
        {
            case AutoWalkingState.Rotating:
                VehicleAutomaticMovement.VehicleAutomaticMovementState state = (VehicleAutomaticMovement.VehicleAutomaticMovementState)AutoMov.getAutomaticMovementState(transform);
                if (state == VehicleAutomaticMovement.VehicleAutomaticMovementState.RIGHT_ROTATING){
                    MouseXDist = VehicleAutomaticMovement.VEHICLE_ROTATION_SPEED;
                }
                else if (state == VehicleAutomaticMovement.VehicleAutomaticMovementState.LEFT_ROTATING){
                    MouseXDist = -VehicleAutomaticMovement.VEHICLE_ROTATION_SPEED;
                } else { 
                    m_AutoWalkingState = AutoWalkingState.WalkingStraight;
                }
                break;
            case AutoWalkingState.Disabled:
                MouseXDist = CrossPlatformInputManager.GetAxis("Mouse X");
                if((MouseXDist == 0.0f) && CurrentlyTouchingLookLog){
                    EventsDBModel.logEvent(EventsTypesDB.UserEvent, SubEventsTypesDB.EndTouchLooking, "Vehicle End Touching Looking");
                    CurrentlyTouchingLookLog = false;
                }else if ((MouseXDist != 0.0f) && !CurrentlyTouchingLookLog){
                    EventsDBModel.logEvent(EventsTypesDB.UserEvent, SubEventsTypesDB.StartTouchLooking, "Vehicle Begin Touching Looking");
                    CurrentlyTouchingLookLog = true;
                }
                MouseLookX = MouseXDist;
                break;
            case AutoWalkingState.Starting:
                AutoMov.buildWaypointsPathForCurrentConfiguration(transform);
                m_AutoWalkingState = AutoWalkingState.Rotating;
                break;
            case AutoWalkingState.WalkingStraight:
            case AutoWalkingState.unknown:
            default:
                break;
        }
        m_MouseLook.LookRotation(transform, m_Camera.transform, MouseXDist);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if((hit.collider.tag != "DestroyedWall") && (hit.collider.tag != "MapMazeObject")){
            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below){
                return;
            }

            if (body == null || body.isKinematic){
                return;
            }
            body.AddForceAtPosition(m_CharacterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
        }else{
            Physics.IgnoreCollision(hit.collider, m_CharacterController.GetComponent<Collider>());
        }
    }

    void getHorizontalAndVertical(ref float horizontal, ref float vertical)
    {
        switch (m_AutoWalkingState)
        {
            case AutoWalkingState.WalkingStraight:
                VehicleAutomaticMovement.VehicleAutomaticMovementState state = (VehicleAutomaticMovement.VehicleAutomaticMovementState)AutoMov.getAutomaticMovementState(transform);
                if (state != VehicleAutomaticMovement.VehicleAutomaticMovementState.STRAIGHT){
                    m_AutoWalkingState = AutoWalkingState.Rotating;
                }else{
                    vertical = VehicleAutomaticMovement.VEHICLE_STRAIGHT_SPEED;
                    horizontal = 0.0f;
                }
                break;
            case AutoWalkingState.Disabled:
                horizontal = CrossPlatformInputManager.GetAxisRaw("HorizontalVehicle");
                vertical = CrossPlatformInputManager.GetAxisRaw("VerticalVehicle");
                if((horizontal == 0.0f) && (vertical == 0.0f) && CurrentlyTouchingMoveLog){
                    EventsDBModel.logEvent(EventsTypesDB.UserEvent, SubEventsTypesDB.EndTouchMoving, "Vehicle End Touching Moving");
                    CurrentlyTouchingMoveLog = false;
                }else if (((horizontal != 0.0f) || (vertical != 0.0f)) && !CurrentlyTouchingMoveLog){
                    EventsDBModel.logEvent(EventsTypesDB.UserEvent, SubEventsTypesDB.StartTouchMoving, "Vehicle Begin Touching Moving");
                    CurrentlyTouchingMoveLog = true;
                }
                HorizontalMov = horizontal;
                VerticalMov = vertical;
                break;
            case AutoWalkingState.Starting:
                AutoMov.buildWaypointsPathForCurrentConfiguration(transform);
                m_AutoWalkingState = AutoWalkingState.Rotating;
                break;
            case AutoWalkingState.Rotating:
            case AutoWalkingState.unknown:
            default:
                break;
        }
    }

    private IEnumerator movementLogger(){
        while (true){
            if(m_AutoWalkingState == AutoWalkingState.Disabled){
                EventsDBModel.logEvent(EventsTypesDB.GameEvent, SubEventsTypesDB.ActualPos, "Vehicle Position: " + transform.position.ToString("G4"));
                EventsDBModel.logEvent(EventsTypesDB.GameEvent, SubEventsTypesDB.FingersPosition, 
                    "Vehicle Movement: H - " + HorizontalMov.ToString() + " V - " + VerticalMov.ToString() + " LookX - " + MouseLookX.ToString());
            }
            yield return new WaitForSeconds(DELAY_FOR_LOG_DATA);
        }
    }

    public void resetPosition(){
        transform.position = new Vector3(0.0f, 1.1f, 0.0f);
        transform.localEulerAngles = Vector3.forward;
        m_AutoWalkingState = AutoWalkingState.Disabled;
    }
    public void loadLevel1Params(){ //Light Robot
        m_WalkSpeed = Speed.LIGHT_SPEED;
    }
    public void loadLevel2Params(){ //Ultra Light Robot
        m_WalkSpeed = Speed.ULTRA_SPEED;
    }
    public void loadLevel3Params(){ //Heavy Robot
        m_WalkSpeed = Speed.HEAVY_SPEED;
    }
    public void loadLevel4Params(){ //Light Robot
        m_WalkSpeed = Speed.LIGHT_SPEED;
    }
}
