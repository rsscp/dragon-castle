using UnityEngine;

public class Idle: StateBehaviour
{
    private StandardMovement _move;

    public Idle(StandardMovement move)
    {
        _move = move;
    }

    public override void Enter()
    {
        Debug.Log("Entered Idle");
        _move.MoveAcceleration = _move.BrakeAcceleration;
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        Debug.Log("Exited Idle");
    }
}