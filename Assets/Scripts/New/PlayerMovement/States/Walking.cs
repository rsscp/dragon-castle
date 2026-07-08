using UnityEngine;

public class Walking : StateBehaviour
{
    private StandardMovement _move;

    public Walking(GameObject owner)
    {
        _move = owner.GetComponent<StandardMovement>();
    }

    public override void Enter()
    {
        Debug.Log("Entered Walking");

        _move.MoveAcceleration = _move.WalkAcceleration;
        _move.MaxMoveVelocity = _move.MaxWalkVelocity;
        _move.MoveAlignmentCurve = _move.WalkAlignmentCurve;
    }

    public override void Stay()
    {
        _move.ApplyMovement();
        _move.ApplyGravity();
    }

    public override void Exit()
    {
        Debug.Log("Exited Walking");
    }
}