using UnityEngine;

public class Walking : StateBehaviour
{
    private StandardMovement _move;

    public Walking(StandardMovement move)
    {
        _move = move;
    }

    public override void Enter()
    {
        Debug.Log("Entered Walking");
        _move.MoveAcceleration = _move.WalkAcceleration;
        _move.MaxMoveVelocity = _move.MaxWalkVelocity;
        _move.MoveAlignmentCurve = _move.WalkAlignmentCurve;
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        Debug.Log("Exited Walking");
    }
}