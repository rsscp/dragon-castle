using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] float sensibility;
    [SerializeField] float upAngleLimit;
    [SerializeField] float downAngleLimit;
    [SerializeField] Camera camera;

    private InputAction lookAction;
    private float rotationX;
    private float rotationY;

    void Start()
    {
        lookAction = InputSystem.actions.FindAction("Look");
        rotationX = camera.transform.localRotation.eulerAngles.x;
        rotationY = transform.localRotation.eulerAngles.y;
    }

    void Update()
    {
        Vector2 lookInput = lookAction.ReadValue<Vector2>() * sensibility;

        rotationX += -lookInput.y;
        rotationY += lookInput.x;

        rotationX = Mathf.Clamp(rotationX, -upAngleLimit, downAngleLimit);

        transform.localRotation = Quaternion.Euler(0, rotationY, 0);
        camera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }
}
