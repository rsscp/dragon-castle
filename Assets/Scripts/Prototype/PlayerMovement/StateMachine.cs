using System;
using Thunderrooster.Statemachine;
using UnityEditor;
using UnityEngine;

public enum MovementStates
{
    Idle,
    Walk,
    Run,
    Jump
}

public class MoveStateMachine : MonoBehaviour
{
    private StateMachine stateMachine;
    private Movement playerMovement;
    private InputManager inputManager;

    private void Start()
    {
        setupStateMachine();
        playerMovement = GetComponent<Movement>();
        inputManager = GetComponent<InputManager>();
    }

    private void Update()
    {
        stateMachine.update();
    }

    private void setupStateMachine()
    {
        stateMachine = new StateMachine(
            (int) MovementStates.Idle,
            new State[] {
                new State(updateIdle, enterIdle, exitIdle, new Transition[]
                {
                    new Transition(IsMoving, (int) MovementStates.Walk)
                }),
                new State(updateWalk, enterWalk, exitWalk, new Transition[]
                {
                    new Transition(IsMoving, (int) MovementStates.Walk)
                })
            }
        );
    }

    private bool IsMoving()
    {
        return inputManager.moving;
    }

    private bool IsRunning()
    {
        return inputManager.running;
    }

    private void updateIdle()
    {
        
    }
    private void enterIdle()
    {
        
    }
    private void exitIdle()
    {
        
    }

    private void updateWalk()
    {
        
    }
    private void enterWalk()
    {
        playerMovement.setDrag(15);
        playerMovement.setAcceleration(30);
        playerMovement.setMaxVelocity(5);
    }
    private void exitWalk()
    {
        
    }

    private void updateRun()
    {

    }
    private void enterRun()
    {
        playerMovement.setDrag(15);
        playerMovement.setAcceleration(60);
        playerMovement.setMaxVelocity(12);
    }
    private void exitRun()
    {

    }
}