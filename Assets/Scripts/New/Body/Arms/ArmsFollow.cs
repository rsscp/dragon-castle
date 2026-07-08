using UnityEngine;
using UnityEngine.Rendering;

public class ArmsFollow : MonoBehaviour
{
    [SerializeField]
    private Transform Body;
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

    private float currentPitch = 0;
    private float currentYaw = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float pitchTarget = FollowPitchCurve.Evaluate(Camera.Pitch * 1f/180f) * Camera.Pitch;
        float yawTarget = Camera.Yaw;
       
        float pitchDiff = (pitchTarget - currentPitch) * PitchFollowAcc * Time.deltaTime;
        float yawDiff = (yawTarget - currentYaw) * YawFollowAcc * Time.deltaTime;

        currentPitch += pitchDiff;
        currentYaw += yawDiff;

        transform.rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
    }
}
