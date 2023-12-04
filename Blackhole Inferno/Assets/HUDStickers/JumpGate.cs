using System;
using System.Collections.Generic;
using UnityEngine;

public class JumpGate : HUDSticker
{
    public XMLJumpGate jumpGate;

    internal void Load(XMLJumpGate jumpGate)
    {
        this.jumpGate = jumpGate;
        this.signatureRadius = jumpGate.signatureRadius;
        this.absoluteWorldPosition = jumpGate.absoluteWorldPosition;
    }

    void Start()
    {
        this.signatureRadius = jumpGate.signatureRadius;
        this.absoluteWorldPosition = jumpGate.absoluteWorldPosition;
    }
}

[Serializable]
public class XMLJumpGate
{
    public Vector3 absoluteWorldPosition;
    public float signatureRadius;
    public string destination;
}