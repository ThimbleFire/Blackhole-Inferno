using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOnDistance : MonoBehaviour
{
    public Vector3 worldPosition;
    public float aMin = 300.0f;
    public float scale = 0.0f;

    void FixedUpdate()
    {
        float d = Vector3.Distance(worldPosition, Ship.LPC.worldPosition);
        float aMax = 1000.0f;
        float bMin = 0.0f;
        float bMax = 1.0f;

        float t = Mathf.InverseLerp(aMin, aMax, d);
        float b = Mathf.InverseLerp(bMax, bMin, t);

        transform.localScale = Vector3.one * b * aMin;


        Vector3 direction = (worldPosition - Ship.LPC.worldPosition).normalized;
        Vector3 target3Position = direction * d;

        transform.position = target3Position;
    }
}
