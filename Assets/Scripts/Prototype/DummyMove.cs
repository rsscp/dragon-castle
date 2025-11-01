using UnityEngine;

public class DummyMove : MonoBehaviour
{

    private bool switchDirection = true;
    private Vector3 movement;

    [SerializeField] float speed = 1;
    [SerializeField] float range = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movement = new Vector3(speed * Time.deltaTime, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (switchDirection)
            transform.Translate(movement);
        else
            transform.Translate(-movement);

        if (Mathf.Abs(transform.position.x) > range)
            switchDirection = !switchDirection;
    }
}
