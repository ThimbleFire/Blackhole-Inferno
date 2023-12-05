using System;
using System.Collections.Generic;
using UnityEngine;

public class Planet : HUDSticker
{
    public XMLPlanet planet;

    internal void Load(XMLPlanet planet)
    {
        this.planet = planet;
        this.name = planet.name;
        this.signatureRadius = planet.signatureRadius;
        this.worldPosition = planet.worldPosition;
    }

    void Start()
    {
        this.name = planet.name;
        this.signatureRadius = planet.signatureRadius;
        this.worldPosition = planet.worldPosition;
    }
}

[Serializable]
public class XMLPlanet
{
    public enum Composition { DIRT, METAL, ROCK };
        
    public string name;
    public Vector3 worldPosition;
    public float signatureRadius;
    public byte humidity;
    public byte temperature;
    public byte gravity;
    public bool magnetosphere;
    public bool vegetation;
    public bool water;
    public bool ice;
    public Composition composition;
}