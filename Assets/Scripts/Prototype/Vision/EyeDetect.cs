using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using static UnityEngine.UI.Image;
using System;
using UnityEngine.Animations;

public class EyeDetect : MonoBehaviour
{
    [SerializeField] EyeTarget target;
    [SerializeField] float range = 20;
    [SerializeField] float angle = 40;
    [SerializeField] float detectionIncrement = 0.1f;
    [SerializeField] float detectionDecrement = -0.01f;

    private LayerMask mask;
    private float detectionState = 0;

#if UNITY_EDITOR
    struct DebugRay
    {
        public Vector3 from;
        public Vector3 to;
        public float alpha;
        public bool hitTarget;
    }

    private List<DebugRay> debugRays = new List<DebugRay>();
    private bool startCalled = false;
#endif

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mask = LayerMask.GetMask("Default");

#if UNITY_EDITOR
        startCalled = true;
#endif
    }

    // Update is called once per frame
    void FixedUpdate()
    {
#if UNITY_EDITOR // Visual debug
        debugRays.Clear();
#endif

        lookForTarget();
        addDetectionState(detectionDecrement);
    }

    private void lookForTarget()
    {
        transform.LookAt(target.transform.position);

        for (int i = 0; i < target.rows; i++)
        {
            for (int j = 0; j < target.columns; j++)
            {
                float score = castVision(
                    target.transform.position -
                    transform.position +
                    target.rayTargets[i, j]
                , "Dummy");
                addDetectionState(score * detectionIncrement);
            }
        }
    }

    private void addDetectionState(float amount)
    {
        detectionState = Mathf.Clamp(detectionState + amount, 0, 1);
    }

    private float castVision(Vector3 direction, string tag)
    {
        RaycastHit hit;
        bool hitAny = Physics.Raycast(transform.position, direction.normalized, out hit, range, mask);
        bool hitTarget = hitAny && hit.transform.gameObject.CompareTag(tag);

        float score = 0;
        if (hitTarget)
            score = calculateScore(direction);

#if UNITY_EDITOR // Visual debug
        float alpha = calculateScore(direction);

        debugRays.Add(new DebugRay
        {
            from = transform.position,
            to = transform.position + direction.normalized * range,
            alpha = alpha,
            hitTarget = hitTarget
        });
#endif

        return score;
    }

    private float calculateScore(Vector3 direction)
    {
        Vector3 lookDirection = transform.parent.forward;
        float centerOffset = Vector3.Angle(direction, lookDirection);
        float angleToLimit = Mathf.Clamp(angle - centerOffset, 0, angle);

        return angleToLimit / angle;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (startCalled)
        {
            Gizmos.color = new Color(detectionState, 1-detectionState, 0);
            Gizmos.DrawCube(transform.position + new Vector3(0, 1, 0), new Vector3(0.2f, 0.5f, 0.2f));

            foreach (DebugRay ray in debugRays)
            {
                Color color = new Color(1,1,1,ray.alpha);
                if (ray.hitTarget)
                    color = new Color(1, 0, 0, ray.alpha); ;

                Gizmos.color = color;
                Gizmos.DrawLine(ray.from, ray.to);
            }

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(
                transform.parent.position,
                transform.parent.position + transform.parent.forward * 2
            );
        }
    }
#endif
}