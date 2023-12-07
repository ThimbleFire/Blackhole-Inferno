using System;
using System.Collections.Generic;
using UnityEngine;

public class JumpGate : HUDSticker
{
    public XMLJumpGate jumpGate;

    public void Load(XMLJumpGate jumpGate)
    {
        this.jumpGate = jumpGate;
        this.signatureRadius = jumpGate.signatureRadius;
        this.worldPosition = jumpGate.absoluteWorldPosition;
    }

    void Start()
    {
        this.signatureRadius = jumpGate.signatureRadius;
        this.worldPosition = jumpGate.absoluteWorldPosition;
    }
    void LateUpdate()
    {
        WorldSpaceToScreenSpace();
    }
}

[Serializable]
public class XMLJumpGate
{
    public Vector3 absoluteWorldPosition;
    public float signatureRadius;
    public string destination;
}