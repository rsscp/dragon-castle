using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class StandardMovement : MonoBehaviour
{
    [SerializeField]
    private Transform Camera;
    [SerializeField]
    private BoxCollider JumpVolume;
    [SerializeField]
    private LayerMask groundMask;

    [SerializeField]
    public float Gravity = 0.3f;
    [SerializeField]
    public float WalkAcceleration = 2f;
    [SerializeField]
    public float RunAcceleration = 4f;
    [SerializeField]
    public float BrakeAcceleration = 6f;
    [SerializeField]
    public float JumpVelocity = 5f;
    [SerializeField]
    public float MinJumpPitch = 20f;
    [SerializeField]
    public float MaxJumpPitch = 50f;
    [SerializeField]
    public float MaxWalkVelocity = 3f;
    [SerializeField]
    public float MaxRunVelocity = 5f;
    [SerializeField]
    public float MaxFallVelocity = 10f;
    [SerializeField]
    public AnimationCurve WalkAlignmentCurve;
    [SerializeField]
    public AnimationCurve RunAlignmentCurve;
    [SerializeField]
    public float MaxRunMisalignmentAngle = 90;

    [NonSerialized]
    public float MoveAcceleration = 0;
    [NonSerialized]
    public float MaxMoveVelocity = 0;
    [NonSerialized]
    public AnimationCurve MoveAlignmentCurve = AnimationCurve.Constant(0, 1, 0);

    private StandardActions _input;
    private StateMachine _movementSM;
    private CharacterController _characterCtrl;

    private Vector3 _moveComponent = Vector3.zero;
    private Vector3 _gravityComponent = Vector3.zero;
    private Vector3 _jumpComponent = Vector3.zero;
    private Vector3 _finalVelocity = Vector3.zero;

    private Vector3 MoveInput = Vector3.zero;
    private bool WalkingHold = false;
    private bool RunHold = false;
    private bool JumpTrigger = false;
    private bool JumpHold = false;

    private void Awake()
    {
        _input = new StandardActions();
        _characterCtrl = GetComponent<CharacterController>();
        _movementSM = GetComponent<StateMachine>();
        _setupStateMachine();
    }

    private void _setupStateMachine()
    {
        StateBehaviour idleBehaviour = new Idle(gameObject);
        StateBehaviour walkingBehaviour = new Walking(gameObject);
        StateBehaviour runningBehaviour = new Running(gameObject);
        StateBehaviour jumpingBehaviour = new Jumping(gameObject);
        StateBehaviour fallingBehaviour = new Falling(gameObject);

        _movementSM.AddState("idle", idleBehaviour);
        _movementSM.AddState("walking", walkingBehaviour);
        _movementSM.AddState("running", runningBehaviour);
        _movementSM.AddState("jumping", jumpingBehaviour);
        _movementSM.AddState("falling", fallingBehaviour);

        _movementSM.AddTransition("idle", "walking", () => MoveInput.magnitude > 0);
        _movementSM.AddTransition("walking", "idle", () => MoveInput.magnitude == 0);
        _movementSM.AddTransition("walking", "running", () =>
        {
            return RunHold
            && MoveInput.magnitude > 0
            && Vector3.Angle(
                    transform.forward,
                    transform.rotation * MoveInput
                ) < MaxRunMisalignmentAngle;
        });
        _movementSM.AddTransition("running", "walking", () =>
        {
            return !RunHold
                || Vector3.Angle(
                    transform.forward,
                    transform.rotation * MoveInput
                ) >= MaxRunMisalignmentAngle;
        });
        _movementSM.AddTransition("running", "idle", () =>
        {
            return !RunHold
                || Vector3.Angle(
                    _finalVelocity,
                    transform.rotation * MoveInput
                ) >= MaxRunMisalignmentAngle;
        });
        _movementSM.AddTransition("running", "jumping", () =>
        {
            Vector3 halfExtents = Vector3.Scale(
                JumpVolume.size * 0.5f,
                JumpVolume.transform.lossyScale
            );

            var result = JumpTrigger
            && Vector3.Angle(
                transform.forward,
                transform.rotation * MoveInput
            ) < MaxRunMisalignmentAngle
            && !Physics.CheckBox(
                JumpVolume.transform.position,
                halfExtents,
                JumpVolume.transform.rotation,
                groundMask,
                QueryTriggerInteraction.Ignore
            );

            JumpTrigger = false;

            return result;
        });
        _movementSM.AddTransition("jumping", "falling", () => true);
        _movementSM.AddTransition("falling", "running", () => _characterCtrl.isGrounded);
        _movementSM.AddTransition("running", "idle", () => MoveInput.magnitude == 0);

        _movementSM.SelectStartingState("idle");
    }

    private void OnEnable()
    {
        _input.Player.Move.started += OnMoveStarted;
        _input.Player.Move.performed += OnMovePerformed;
        _input.Player.Move.canceled += OnMoveCanceled;

        _input.Player.Run.started += OnRunStarted;
        _input.Player.Run.canceled += OnRunCanceled;

        _input.Player.Jump.started += OnJumpStarted;
        _input.Player.Jump.performed += OnJumpPerformed;
        _input.Player.Jump.canceled += OnJumpCanceled;
        _input.Player.Enable();
    }

    private void OnDisable()
    {
        _input.Player.Move.started -= OnMoveStarted;
        _input.Player.Move.performed -= OnMovePerformed;
        _input.Player.Move.canceled -= OnMoveCanceled;

        _input.Player.Run.started -= OnRunStarted;
        _input.Player.Run.canceled -= OnRunCanceled;

        _input.Player.Jump.started -= OnJumpStarted;
        _input.Player.Jump.performed -= OnJumpPerformed;
        _input.Player.Jump.canceled -= OnJumpCanceled;
        _input.Player.Disable();
    }

    private void _resetTriggers()
    {
        //This wasn't running on the right order (reset before trigger effect)
    }

    private void diableReferenceVolumes()
    {
        JumpVolume.enabled = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        diableReferenceVolumes();
    }

    // Update is called once per frame
    void Update()
    {
        _characterCtrl.Move(_finalVelocity * Time.deltaTime);
        _resetTriggers();
    }

    public void ApplyMovement()
    {
        float alignment = Vector3.Angle(
            transform.forward,
            transform.rotation * MoveInput
        ) / 180;
        float alignmentStrentgh = MoveAlignmentCurve.Evaluate(alignment);

        _moveComponent =  Vector3.MoveTowards(
            _moveComponent,
            transform.rotation
                * MoveInput
                * MaxMoveVelocity
                * alignmentStrentgh,
            MoveAcceleration * Time.deltaTime
        );

        _finalVelocity = new Vector3(
            _moveComponent.x,
            _finalVelocity.y,
            _moveComponent.z
        );
    }

    public void ApplyGravity()
    {
        if (_characterCtrl.isGrounded)
        {
            _gravityComponent = Vector3.zero;
            _finalVelocity = new Vector3(_finalVelocity.x, -2, _finalVelocity.z);
        }
        else
        {
            _gravityComponent += new Vector3(0, -Gravity, 0) * Time.deltaTime;
            _gravityComponent = Vector3.ClampMagnitude(_gravityComponent, MaxFallVelocity);
            _finalVelocity += _gravityComponent;
        }
    }

    public void ApplyJump()
    {
        if (false)
        {
            //TODO cheking for vaulting with volumes infront of player
        }
        else if (_characterCtrl.isGrounded)
        {
            Vector3 flatFinalVelocity = new Vector3(_finalVelocity.x, 0, _finalVelocity.z);
            Vector3 lookDirection = Camera.transform.forward;
            Vector3 jumpDirection = new Vector3(lookDirection.x, 0, lookDirection.z);
            float launchPitch = Mathf.Clamp(
                Mathf.DeltaAngle(0f, Camera.eulerAngles.x),
                -MaxJumpPitch,
                -MinJumpPitch
            );

            Debug.Log(Camera.localRotation.eulerAngles.x);

            jumpDirection = Quaternion.AngleAxis(launchPitch, transform.right) * jumpDirection;

            _finalVelocity = jumpDirection * JumpVelocity;
        }
    }

    public void ApplyVelocity(Vector3 addedVelociy)
    {
        _finalVelocity += addedVelociy;
    }

    public float GetCurrentVelocity()
    {
        return _finalVelocity.magnitude;
    }

    public Vector3 GetVelocityDirection()
    {
        return new Vector3(_finalVelocity.x, _finalVelocity.z).normalized;
    }

    private void OnMoveStarted(InputAction.CallbackContext context)
    {
        WalkingHold = true;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        Vector2 input2D = context.ReadValue<Vector2>();
        MoveInput = new Vector3(input2D.x, 0, input2D.y);
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        WalkingHold = false;
        MoveInput = Vector3.zero;
    }

    private void OnRunStarted(InputAction.CallbackContext context)
    {
        RunHold = true;
    }

    private void OnRunCanceled(InputAction.CallbackContext context)
    {
        RunHold = false;
    }

    private void OnJumpStarted(InputAction.CallbackContext context)
    {
        Debug.Log("HellllooooooOOOOOO??????");

        JumpTrigger = true;
        JumpHold = true;
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {

    }

    private void OnJumpCanceled(InputAction.CallbackContext context)
    {
        JumpHold = false;
    }
}
