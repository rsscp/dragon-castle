using UnityEngine;

public class Running : StateBehaviour
{
    private StandardMovement _move;
    private Animator[] animators;

    public Running(GameObject owner)
    {
        _move = owner.GetComponent<StandardMovement>();
        animators = owner.GetComponentsInChildren<Animator>();
    }

    public override void Enter()
    {
        Debug.Log("Entered Running");

        _move.MoveAcceleration = _move.RunAcceleration;
        _move.MaxMoveVelocity = _move.MaxRunVelocity;
        _move.MoveAlignmentCurve = _move.RunAlignmentCurve;

        foreach (Animator anim in animators)
            anim.SetBool("Running", true);
    }

    public override void Stay()
    {
        _move.ApplyMovement();
        _move.ApplyGravity();
    }

    public override void Exit()
    {
        Debug.Log("Exited Running");

        foreach (Animator anim in animators)
            anim.SetBool("Running", false);
    }
}