using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class EyeTarget : MonoBehaviour
{
    [SerializeField] EyeDetect eye;
    [SerializeField] float height = 3;
    [SerializeField] float width = 2;
    [SerializeField] public int rows = 7;
    [SerializeField] public int columns = 5;
    
    private float rowsOffset;
    private float columnsOffset;
    private Vector3[,] rayTargetsReference;
    public Vector3[,] rayTargets;

#if UNITY_EDITOR
    private bool startCalled = false;
#endif

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rowsOffset = height / (rows - 1);
        columnsOffset = width / (columns - 1);
        rayTargetsReference = new Vector3[rows, columns];
        rayTargets = new Vector3[rows, columns];

        Vector3 origin = new Vector3 (-width/2, -height / 2, 0);

        for (int i = 0; i < rows; i++)
            for (int j = 0; j < columns; j++)
                rayTargetsReference[i, j] = origin + new Vector3(
                    j * columnsOffset,
                    i * rowsOffset,
                    0
                );

#if UNITY_EDITOR
        startCalled = true;
#endif
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < columns; j++)
                rayTargets[i, j] = eye.transform.rotation * rayTargetsReference[i, j];
    }

#if UNITY_EDITOR
    private void OnDrawGizmos() // Visual debug
    {
        if (startCalled)
        {
            Gizmos.color = Color.blue;

            if (rayTargets != null)
                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < columns; j++)
                        Gizmos.DrawSphere(transform.position + rayTargets[i, j], 0.05f);
        }
    }
#endif
}
