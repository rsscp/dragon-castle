using UnityEngine;

public class Jumping : StateBehaviour
{
    private StandardMovement _move;

    public Jumping(GameObject owner)
    {
        _move = owner.GetComponent<StandardMovement>();
    }

    public override void Enter()
    {
        //Vector2 dir = _move.GetVelocityDirection();
        //_move.ApplyVelocity(new Vector3(dir.x, 1.3f, dir.y) * _move.JumpVelocity);

        _move.ApplyJump();

        Debug.Log("Entered Jump");
    }

    public override void Stay()
    {
        
    }

    public override void Exit()
    {
        Debug.Log("Exited Jump");
    }
}
