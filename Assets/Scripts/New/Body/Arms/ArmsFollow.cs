using UnityEngine;
using UnityEngine.Rendering;

public class ArmsFollow : MonoBehaviour
{
    [SerializeField]
    private FPSCamera Camera;
    [SerializeField]
    private AnimationCurve FollowPitchCurve;
    [SerializeField]
    private float MaxYawOffset;
    [SerializeField]
    private float MaxPitchOffset;
    [SerializeField]
    private float YawFollowAcc;
    [SerializeField]
    private float PitchFollowAcc;
    [SerializeField]
    private float SmoothVelocity = 10;
    [SerializeField]
    private float MaxSmoothVelocity = 100;

    private float smoothedPitch = 0;
    private float smoothedYaw = 0;

    private float targetPitch = 0;
    private float targetYaw = 0;

    private float currentPitch = 0;
    private float currentYaw = 0;


    private float pDiff = 0;
    private float yDiff = 0;

    private Quaternion targetRotation = Quaternion.Euler(Vector3.zero);
    private Quaternion currentRotation;
    private float a;
    private float b;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentRotation = transform.rotation;
        a = MaxSmoothVelocity;
        b = MaxSmoothVelocity - SmoothVelocity;
        
}

    // Update is called once per frame
    void Update()
    {
        targetPitch = FollowPitchCurve.Evaluate(Camera.Pitch * 1f / 180f) * Camera.Pitch;
        targetYaw = Camera.Yaw;

        targetRotation = Quaternion.Euler(
            targetPitch,
            targetYaw,
            0f
        );

        float v = a - b * Mathf.Exp(-0.01f * Quaternion.Angle(currentRotation, targetRotation));
        float t = 1f - Mathf.Exp(-v * Time.deltaTime);

        float diff = -SmoothVelocity * Time.deltaTime;
        float angle = Quaternion.Angle(
            currentRotation,
            targetRotation
        );
        
        //if (angle > 0.5)
        //    t = 1f - Mathf.Exp(-t);

        currentRotation = Quaternion.Slerp(
            currentRotation,
            targetRotation,
            t
        );

        transform.rotation = currentRotation;
    }
}
