using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;
using UnityStandardAssets.Characters.FirstPerson;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class DroneFPSController : MonoBehaviour
{
    [SerializeField]private float m_StickToGroundForce;
    [SerializeField]private float m_GravityMultiplier;
    [SerializeField]private MouseLook m_MouseLook;
    [SerializeField]private float m_StepInterval;
    [SerializeField]private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
    [SerializeField]private DroneAutomaticMovement AutoMov;
    public enum Speed { NORMAL_SPEED = 5, SLOW_SPEED = 3, FAST_SPEED = 10 };
    public Speed m_WalkSpeed;

    private Camera m_Camera;
    private Vector2 m_Input;
    private Vector3 m_MoveDir = Vector3.zero;
    private CharacterController m_CharacterController;
    private CollisionFlags m_CollisionFlags;
    private bool m_PreviouslyGrounded;
    private float m_StepCycle;
    private float m_NextStep;
    private AudioSource m_AudioSource;

    public enum AutoWalkingState { Starting, Started, Disabled, unknown };
    public static AutoWalkingState m_AutoWalkingState;

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
    }

    // Update is called once per frame
    private void Update()
    {
        RotateView();
        if (!m_CharacterController.isGrounded && m_PreviouslyGrounded){
            m_MoveDir.y = 0f;
        }

        m_PreviouslyGrounded = m_CharacterController.isGrounded;
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

        if (m_CharacterController.isGrounded){
            m_MoveDir.y = -m_StickToGroundForce;
        }
        else{
            m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
        }
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
        if (!m_CharacterController.isGrounded)
        {
            return;
        }
        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        int n = Random.Range(1, m_FootstepSounds.Length);
        m_AudioSource.clip = m_FootstepSounds[n];
        m_AudioSource.PlayOneShot(m_AudioSource.clip);
        // move picked sound to index 0 so it's not picked next time
        m_FootstepSounds[n] = m_FootstepSounds[0];
        m_FootstepSounds[0] = m_AudioSource.clip;
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

    private void RotateView()
    {
        float MouseXDist = 0.0f;
        MouseXDist = CrossPlatformInputManager.GetAxis("Mouse XDrone");
        m_MouseLook.LookRotation(transform, m_Camera.transform, MouseXDist);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        //dont move the rigidbody if the character is on top of it
        if (m_CollisionFlags == CollisionFlags.Below){
            return;
        }
        if (body == null || body.isKinematic){
            return;
        }
        body.AddForceAtPosition(m_CharacterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
    }

    void getHorizontalAndVertical(ref float horizontal, ref float vertical)
    {
        switch (m_AutoWalkingState)
        {
            case AutoWalkingState.Started:
                AutomaticMovement.Direction State = (AutomaticMovement.Direction)AutoMov.getAutomaticMovementState(transform);
                if (State == AutomaticMovement.Direction.EAST){
                    vertical = 0.0f;
                    horizontal = -DroneAutomaticMovement.DRONE_AUTOMATIC_SPEED;
                }
                else if (State == AutomaticMovement.Direction.WEST){
                    vertical = 0.0f;
                    horizontal = DroneAutomaticMovement.DRONE_AUTOMATIC_SPEED;
                }
                else if (State == AutomaticMovement.Direction.NORTH){
                    vertical = DroneAutomaticMovement.DRONE_AUTOMATIC_SPEED;
                    horizontal = 0.0f;
                }
                else if (State == AutomaticMovement.Direction.SOUTH) { 
                    vertical = -DroneAutomaticMovement.DRONE_AUTOMATIC_SPEED;
                    horizontal = 0.0f;
                }
                break;
            case AutoWalkingState.Disabled:
                horizontal = CrossPlatformInputManager.GetAxisRaw("HorizontalDrone");
                vertical = CrossPlatformInputManager.GetAxisRaw("VerticalDrone");
                break;
            case AutoWalkingState.Starting:
                AutoMov.buildWaypointsPathForCurrentConfiguration(transform);
                m_AutoWalkingState = AutoWalkingState.Started;
                break;
            case AutoWalkingState.unknown:
            default:
                break;
        }
    }
    public void resetPosition(){
        transform.position = new Vector3(0.0f, 10.0f, 0.0f);
        m_AutoWalkingState = AutoWalkingState.Disabled;
    }
    public void loadLevel1Params(){ //Normal Quadcopter
        m_WalkSpeed = Speed.NORMAL_SPEED;
    }
    public void loadLevel2Params(){ //Slow Quadcopter
        m_WalkSpeed = Speed.SLOW_SPEED;
    }
    public void loadLevel3Params(){ //Normal Quadcopter
        m_WalkSpeed = Speed.NORMAL_SPEED;
    }
    public void loadLevel4Params(){ //Fast Quadcopter
        m_WalkSpeed = Speed.FAST_SPEED;
    }
}