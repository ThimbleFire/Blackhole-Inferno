using System;
using System.Collections.Generic;
using UnityEngine;

public class Sun : HUDSticker
{
    public float luminocity;
    public Color color;

    internal void Load(XMLSun sun)
    {
        this.name = sun.name;
        this.signatureRadius = sun.signatureRadius;
        this.worldPosition = sun.absoluteWorldPosition;
        this.luminocity = sun.luminocity;
        this.color = sun.color;
    }

    public XMLSun Save()
    {
        XMLSun copy = new XMLSun
        {
            name = this.name,
            signatureRadius = this.signatureRadius,
            absoluteWorldPosition = this.worldPosition,
            luminocity = this.luminocity,
            color = this.color
        };

        return copy;
    }

    void LateUpdate()
    {        
        WorldSpaceToScreenSpace();
    }
}

[Serializable]
public class XMLSun
{
    public string name;
    public Vector3 absoluteWorldPosition;
    public float signatureRadius;
    public float luminocity;
    public Color color;
}