using UnityEngine;

public class EyePivotRotate : MonoBehaviour
{

    [SerializeField] float speed = 1;

    // Update is called once per frame
    void FixedUpdate()
    {
        Quaternion rotation = Quaternion.Euler(0, speed * Time.deltaTime, 0);
        transform.rotation = transform.rotation * rotation;
    }
}
