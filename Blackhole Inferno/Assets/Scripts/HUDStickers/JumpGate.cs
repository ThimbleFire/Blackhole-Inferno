using System;
using System.Collections.Generic;
using UnityEngine;

public class JumpGate : HUDSticker
{
    public string destination;

    public void Load(XMLJumpGate jumpGate)
    {
        this.signatureRadius = jumpGate.signatureRadius;
        this.worldPosition = jumpGate.absoluteWorldPosition;
        this.destination = jumpGate.destination;
    }

    public XMLJumpGate Save()
    {
        XMLJumpGate copy = new XMLJumpGate
        {
            signatureRadius = this.signatureRadius,
            absoluteWorldPosition = this.worldPosition,
            destination = this.destination
        };

        return copy;
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