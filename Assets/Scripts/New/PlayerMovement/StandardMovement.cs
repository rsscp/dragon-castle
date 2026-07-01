using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using static UnityEngine.GraphicsBuffer;

public class StandardMovement : MonoBehaviour
{
    [SerializeField]
    public float Gravity = 0.3f;
    [SerializeField]
    public float WalkAcceleration = 2f;
    [SerializeField]
    public float RunAcceleration = 4f;
    [SerializeField]
    public float BrakeAcceleration = 6f;
    [SerializeField]
    public float JumpVelocity = 3f;
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

    private Vector2 _moveComponent = Vector2.zero;
    private Vector3 _gravityComponent = Vector3.zero;
    private Vector3 _jumpComponent = Vector3.zero;
    private Vector3 _finalVelocity = Vector3.zero;

    private Vector2 MoveInput = Vector2.zero;
    private bool WalkingHold = false;
    private bool RunHold = false;
    private bool JumpTrigger = false;
    private bool JumpHold = false;

    private void Awake()
    {
        _input = new StandardActions();
        _setupStateMachine();
    }

    private void _setupStateMachine()
    {
        _characterCtrl = GetComponent<CharacterController>();
        _movementSM = GetComponent<StateMachine>();

        StateBehaviour idleBehaviour = new Idle(this);
        StateBehaviour walkingBehaviour = new Walking(this);
        StateBehaviour runningBehaviour = new Running(this);

        _movementSM.AddState("idle", idleBehaviour);
        _movementSM.AddState("walking", walkingBehaviour);
        _movementSM.AddState("running", runningBehaviour);

        _movementSM.AddTransition("idle", "walking", () => MoveInput.magnitude > 0);
        _movementSM.AddTransition("walking", "idle", () => MoveInput.magnitude == 0);
        _movementSM.AddTransition("walking", "running", () =>
        {
            return
            RunHold &&
            MoveInput.magnitude > 0 &&
            Vector3.Angle(
                    transform.forward,
                    transform.rotation * new Vector3(MoveInput.x, 0, MoveInput.y)
                ) < MaxRunMisalignmentAngle;
        });
        _movementSM.AddTransition("running", "walking", () =>
        {
            return
                !RunHold ||
                Vector3.Angle(
                    transform.forward,
                    transform.rotation * new Vector3(MoveInput.x, 0, MoveInput.y)
                ) > MaxRunMisalignmentAngle;
        });
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MoveInput = Vector2.zero;
        JumpTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        _applyMovement();
        _applyGravity();
        _applyJump();
        _characterCtrl.Move(transform.rotation * _finalVelocity * Time.deltaTime);
    }

    private void _applyMovement()
    {
        float alignment = Vector3.Angle(
            transform.forward,
            transform.rotation * new Vector3(MoveInput.x, 0, MoveInput.y)
        ) / 180;
        float alignmentStrentgh = MoveAlignmentCurve.Evaluate(alignment);

        _moveComponent = Vector2.MoveTowards(
            _moveComponent,
            MoveInput * MaxMoveVelocity * alignmentStrentgh,
            MoveAcceleration * Time.deltaTime
        );

        _finalVelocity = new Vector3(
            _moveComponent.x,
            _finalVelocity.y,
            _moveComponent.y
        );
    }

    private void _applyGravity()
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

    private void _applyJump()
    {
        if (JumpTrigger && _characterCtrl.isGrounded)
            _finalVelocity = new Vector3(_finalVelocity.x, JumpVelocity, _finalVelocity.z);
        JumpTrigger = false;
    }

    private void OnMoveStarted(InputAction.CallbackContext context)
    {
        MoveAcceleration = WalkAcceleration;
        MaxMoveVelocity = MaxWalkVelocity;
        WalkingHold = true;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        WalkingHold = false;
        MoveInput = Vector2.zero;
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
