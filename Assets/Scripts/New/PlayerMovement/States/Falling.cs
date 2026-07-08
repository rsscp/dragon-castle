using UnityEngine;

public class Falling : StateBehaviour
{
    private StandardMovement _move;

    public Falling(GameObject owner)
    {
        _move = owner.GetComponent<StandardMovement>();
    }

    public override void Enter()
    {
        Debug.Log("Entered Falling");
    }

    public override void Stay()
    {
        _move.ApplyGravity();
    }

    public override void Exit()
    {
        Debug.Log("Exited Falling");
    }
}
