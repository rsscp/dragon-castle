using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DummyEnemyMove : MonoBehaviour
{
    [SerializeField] GameObject pathObject;
    [SerializeField] float walkSpeed;
    [SerializeField] float lookSpeed;

    private Vector3[] path;
    private int index;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        path = new Vector3[pathObject.transform.childCount];

        for (int i = 1; i <= path.Length; i++)
        {
            string name = i.ToString();
            Transform point = pathObject.transform.Find(name);
            path[i] = point.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        walk();
        look();
    }

    private void walk()
    {
        /*
        float step = walkSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);

        if (Vector3.Distance(transform.position, target.position) < 0.001f)
        {
            index++;
        }
        */
    }

    private void look()
    {

    }
}
