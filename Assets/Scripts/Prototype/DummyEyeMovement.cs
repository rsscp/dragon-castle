using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DummyEyeMovement : MonoBehaviour
{
    [SerializeField] float lookThreshold = 20;
    [SerializeField] float transitionThreshold = 1;
    [SerializeField] float speed = 4;

    private float targetY;

    // Update is called once per frame
    void Update()
    {
        Quaternion prevRotation = transform.rotation;
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            Quaternion.Euler(0, targetY, 0),
            speed * Time.deltaTime
        );

        if (Quaternion.Angle(prevRotation, transform.rotation) < transitionThreshold)
            updateTarget();
    }

    private void updateTarget()
    {
        targetY = -targetY;
    }
}
