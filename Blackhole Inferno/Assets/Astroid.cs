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

    void LateUpdate()
    {
        WorldSpaceToScreenSpace();
    }

    protected override void Awake()
    {
        this.name = mineral.ToString();

        base.Awake();
    }
}
