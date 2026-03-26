using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private InputAction moveAction;
    private InputAction runAction;

    public bool moving = false;
    public bool running = false;
    public Vector3 direction = Vector3.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        runAction = InputSystem.actions.FindAction("Run");
        runAction.started += run;
    }

    // Update is called once per frame
    void Update()
    {
        setDirection();
        moving = !direction.Equals(Vector3.zero);
    }

    private void run(InputAction.CallbackContext obj)
    {
        
    }

    public void setDirection()
    {
        Vector2 input = moveAction.ReadValue<Vector2>().normalized;
        direction = new Vector3(input.x, 0, input.y);
    }
}
