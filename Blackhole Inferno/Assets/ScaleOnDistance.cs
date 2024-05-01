using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOnDistance : MonoBehaviour
{
    public Vector3 worldPosition;
    private float aMin = 1500.0f;

    void FixedUpdate()
    {
        float d = Vector3.Distance(worldPosition, Ship.LPC.worldPosition);
        float aMax = 149597870.7f; // planets become visible at 1-quater of an AU
        float bMin = 0.0f;
        float bMax = 1.0f;

        float t = Mathf.InverseLerp(aMin, aMax, d);
        float b = Mathf.InverseLerp(bMax, bMin, t);

        transform.localScale = Vector3.one * b * aMin;
    }
}
