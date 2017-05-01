using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;
using UnityStandardAssets.Characters.FirstPerson;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class VehicleFPSController : MonoBehaviour
{
    [SerializeField]private float m_WalkSpeed;
    [SerializeField]private float m_JumpSpeed;
    [SerializeField]private MouseLook m_MouseLook;
    [SerializeField]private LerpControlledBob m_JumpBob = new LerpControlledBob();
    [SerializeField]private float m_StepInterval;
    [SerializeField]private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
    [SerializeField]private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
    [SerializeField]private AudioClip m_LandSound;           // the sound played when character touches back on ground.
    [SerializeField]public VehicleAutomaticMovement AutoMov;

    private Camera m_Camera;
    private bool m_Jump;
    private Vector2 m_Input;
    private Vector3 m_MoveDir = Vector3.zero;
    private CharacterController m_CharacterController;
    private CollisionFlags m_CollisionFlags;
    private bool m_PreviouslyGrounded;
    private float m_StepCycle;
    private float m_NextStep;
    private bool m_Jumping;
    private AudioSource m_AudioSource;

    public enum AutoWalkingState {     Starting, Rotating, WalkingStraight, Disabled, unknown    };
    public static AutoWalkingState m_AutoWalkingState;

    // Use this for initialization
    private void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_Camera = Camera.main;
        m_StepCycle = 0f;
        m_NextStep = m_StepCycle / 2f;
        m_Jumping = false;
        m_AudioSource = GetComponent<AudioSource>();
        m_MouseLook.Init(transform, m_Camera.transform);
        m_AutoWalkingState = AutoWalkingState.Disabled;
    }

    // Update is called once per frame
    private void Update()
    {
        RotateView();
        // the jump state needs to read here to make sure it is not missed
        if (!m_Jump){
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }

        if (!m_PreviouslyGrounded && m_CharacterController.isGrounded){
            PlayLandingSound();
            m_MoveDir.y = 0f;
            m_Jumping = false;
        }
        if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded){
            m_MoveDir.y = 0f;
        }
        m_PreviouslyGrounded = m_CharacterController.isGrounded;
    }

    private void PlayLandingSound()
    {
        m_AudioSource.clip = m_LandSound;
        m_AudioSource.Play();
        m_NextStep = m_StepCycle + .5f;
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

        m_MoveDir.x = desiredMove.x * m_WalkSpeed;
        m_MoveDir.z = desiredMove.z * m_WalkSpeed;

        if (m_CharacterController.isGrounded)
        {
            m_MoveDir.y = 0;
            if (m_Jump)
            {
                m_MoveDir.y = m_JumpSpeed;
                PlayJumpSound();
                m_Jump = false;
                m_Jumping = true;
            }
        }

        m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);

        ProgressStepCycle(m_WalkSpeed);

        m_MouseLook.UpdateCursorLock();
    }

    private void PlayJumpSound()
    {
        m_AudioSource.clip = m_JumpSound;
        m_AudioSource.Play();
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
            case AutoWalkingState.WalkingStraight:
                VehicleAutomaticMovement.VehicleAutomaticMovementState state = (VehicleAutomaticMovement.VehicleAutomaticMovementState)AutoMov.getAutomaticMovementState(transform);
                if (state != VehicleAutomaticMovement.VehicleAutomaticMovementState.STRAIGHT){
                    m_AutoWalkingState = AutoWalkingState.Rotating;
                }else{
                    vertical = VehicleAutomaticMovement.VEHICLE_STRAIGHT_SPEED;;
                    horizontal = 0.0f;
                }
                break;
            case AutoWalkingState.Disabled:
                horizontal = CrossPlatformInputManager.GetAxisRaw("HorizontalVehicle");
                vertical = CrossPlatformInputManager.GetAxisRaw("VerticalVehicle");
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

    public void resetPosition(){
        transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        transform.localEulerAngles = Vector3.right;
    }
}
