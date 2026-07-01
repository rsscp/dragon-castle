using UnityEngine;

public static class Utils
{
    public static float Normal(float mean, float standardDeviation)
    {
        float u1 = Mathf.Max(Random.value, 0.000001f);
        float u2 = Random.value;

        float standardNormal =
            Mathf.Sqrt(-2f * Mathf.Log(u1)) *
            Mathf.Cos(2f * Mathf.PI * u2);

        return mean + standardDeviation * standardNormal;
    }
}