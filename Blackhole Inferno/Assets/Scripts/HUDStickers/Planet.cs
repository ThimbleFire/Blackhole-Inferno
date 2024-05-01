using System;
using System.Collections.Generic;
using UnityEngine;

public class Planet : HUDSticker
{
    public byte humidity;
    public byte temperature;
    public byte gravity;
    public bool magnetosphere;
    public bool vegetation;
    public bool water;
    public bool ice;
    public XMLPlanet.Composition composition;
    public ScaleOnDistance model;

    internal void Load(XMLPlanet planet)
    {
        this.name = planet.name;
        this.signatureRadius = planet.signatureRadius;
        this.worldPosition = planet.absoluteWorldPosition;
        this.humidity = planet.humidity;
        this.temperature = planet.temperature;
        this.gravity = planet.gravity;
        this.magnetosphere = planet.magnetosphere;
        this.vegetation = planet.vegetation;
        this.water = planet.water;
        this.ice  = planet.ice;
    }
    
    public XMLPlanet Save()
    {
        XMLPlanet copy = new XMLPlanet
        {
            name = this.name,
            signatureRadius = this.signatureRadius,
            absoluteWorldPosition = this.worldPosition,
            humidity = this.humidity,
            temperature = this.temperature,
            gravity = this.gravity,
            magnetosphere = this.magnetosphere,
            vegetation = this.vegetation,
            water = this.water,
            ice = this.ice
        };

        return copy;
    }
    
    void LateUpdate()
    {    
        WorldSpaceToScreenSpace();
        model.transform.position = target3Position;
    }

    public override void Arrived()
    {

    }

    public override void Leaving()
    {
        base.Leaving();
    }
}

[Serializable]
public class XMLPlanet
{
    public enum Composition { DIRT, METAL, ROCK };
        
    public string name;
    public Vector3 absoluteWorldPosition;
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