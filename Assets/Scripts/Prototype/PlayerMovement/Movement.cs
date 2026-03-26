using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UIElements;

public class Movement : MonoBehaviour
{
    float dragScalar = 0;
    float accelerationScalar = 0;
    float velocityMaxMagnitude = 0;

    [SerializeField] float velocityDropRatio;

    private InputAction moveAction;

    private CharacterController controller;
    private InputManager inputManager;
    private Vector3 direction;
    private Vector3 velocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        controller = GetComponent<CharacterController>();
        inputManager = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        direction = transform.rotation * inputManager.direction;

        Vector3 drag = -velocity.normalized * dragScalar;
        Vector3 acceleration = direction * accelerationScalar;

        velocity += drag * Time.deltaTime;
        velocity += acceleration * Time.deltaTime;

        if (velocity.magnitude > velocityMaxMagnitude)
            velocity = velocity.normalized * velocityMaxMagnitude;

        controller.SimpleMove(velocity);
    }

    public void setDrag(float drag)
    {
        dragScalar = drag;
    }

    public void setAcceleration(float acceleration)
    {
        accelerationScalar = acceleration;
    }

    public void setMaxVelocity(float maxVelocity) { 
        velocityMaxMagnitude = maxVelocity;
    }
}
