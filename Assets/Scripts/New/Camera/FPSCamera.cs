using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    [SerializeField]
    private Transform _playerCamera;
    [SerializeField]
    private float mouseSensitivity = 0.08f;
    [SerializeField]
    private float gamepadSensitivity = 120f;

    private StandardActions input;

    public float Pitch { get; private set; } = 0f;
    public float Yaw { get; private set; } = 0f;

    private void Awake()
    {
        input = new StandardActions();
    }

    private void OnEnable()
    {
        input.Player.Enable();
    }

    private void OnDisable()
    {
        input.Player.Disable();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Vector2 look = input.Player.Look.ReadValue<Vector2>();

        float lookYaw = look.x * mouseSensitivity;
        float lookPitch = look.y * mouseSensitivity;

        Pitch -= lookPitch;
        Pitch = Mathf.Clamp(Pitch, -85f, 85f);

        Yaw += lookYaw;

        _playerCamera.localRotation = Quaternion.Euler(Pitch, 0f, 0f);
        transform.Rotate(Vector3.up * lookYaw);
    }
}
