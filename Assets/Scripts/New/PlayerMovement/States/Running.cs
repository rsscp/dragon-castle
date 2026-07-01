using UnityEngine;

public class Running : StateBehaviour
{
    private StandardMovement _move;

    public Running(StandardMovement move)
    {
        _move = move;
    }

    public override void Enter()
    {
        Debug.Log("Entered Running");
        _move.MoveAcceleration = _move.RunAcceleration;
        _move.MaxMoveVelocity = _move.MaxRunVelocity;
        _move.MoveAlignmentCurve = _move.RunAlignmentCurve;
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        Debug.Log("Exited Running");
    }
}