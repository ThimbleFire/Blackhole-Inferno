using System;
using System.Collections.Generic;
using UnityEngine;

public class Sun : HUDSticker
{
    public XMLSun sun;

    internal void Load(XMLSun sun)
    {
        this.sun = sun;
        this.name = sun.name;
        this.signatureRadius = sun.signatureRadius;
        this.worldPosition = sun.absoluteWorldPosition;
    }

    void Start()
    {
        this.name = sun.name;
        this.signatureRadius = sun.signatureRadius;
        this.worldPosition = sun.absoluteWorldPosition;
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
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