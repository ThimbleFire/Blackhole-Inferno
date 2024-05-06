using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Astroids are rendered in the world canvas.
// They're only visible within 1,000 distance

// Their world position is offset by the world position of the astroid belt they belong to

public class Astroid : HUDSticker
{
    private byte remainingResource = 30;
    private XMLBelt.Minerals mineral = XMLBelt.Minerals.Terylium;
    Vector3 oldPosition;

    private void Start()
    {
        oldPosition = transform.position;
    }

    void LateUpdate()
    {
        WorldSpaceToScreenSpace();

        if(oldPosition != transform.position)
        {
            oldPosition = transform.position;
            Debug.Log(transform.position);
        }

    }

    protected override void Awake()
    {
        this.name = mineral.ToString();

        base.Awake();
    }
}
