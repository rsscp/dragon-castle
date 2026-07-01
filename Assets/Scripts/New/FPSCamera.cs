using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    [SerializeField] private Transform _playerCamera;
    [SerializeField] private float mouseSensitivity = 0.08f;
    [SerializeField] private float gamepadSensitivity = 120f;

    private StandardActions input;
    private float pitch;

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

        // Mouse delta should normally not use deltaTime.
        float yaw = look.x * mouseSensitivity;
        float lookPitch = look.y * mouseSensitivity;

        pitch -= lookPitch;
        pitch = Mathf.Clamp(pitch, -85f, 85f);

        _playerCamera.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        transform.Rotate(Vector3.up * yaw);
    }
}
