using UnityEngine;

public class Crouching : StateBehaviour
{
    private StandardMovement _move;
    private Transform _tripod;

    public Crouching(GameObject owner)
    {
        _move = owner.GetComponent<StandardMovement>();
    }

    public override void Enter()
    {
        Debug.Log("Entered Crouching");

        _move.MoveAcceleration = 4;
        _move.MaxMoveVelocity = 0.5f;
        _move.MoveAlignmentCurve = _move.WalkAlignmentCurve;
    }

    public override void Stay()
    {
        _move.ApplyMovement();
        _move.ApplyGravity();
    }

    public override void Exit()
    {
        Debug.Log("Exited Crouching");
    }
}
