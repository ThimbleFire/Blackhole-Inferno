using System;
using System.Collections.Generic;
using UnityEngine;

public class Station : HUDSticker
{
    public bool repairsAvailable;
    public float standardDeviation;
    public float taxRate;
    public byte moduleSellLevel;
    public bool trade_modules;
    public List<XMLStation.Ships> shipsForSale;

    internal void Load(XMLStation station)
    {
        this.name = station.name;
        this.signatureRadius = station.signatureRadius;
        this.worldPosition = station.absoluteWorldPosition;
        this.repairsAvailable = station.repairsAvailable;
        this.standardDeviation = station.standardDeviation;
        this.taxRate = station.taxRate;
        this.moduleSellLevel = station.moduleSellLevel;
        this.trade_modules = station.trade_modules;
        this.shipsForSale = station.shipsForSale;
    }
    
    public XMLStation Save()
    {
        XMLStation copy = new XMLStation
        {
            name = this.name,
            signatureRadius = this.signatureRadius,
            absoluteWorldPosition = this.worldPosition,
            repairsAvailable = this.repairsAvailable,
            standardDeviation = this.standardDeviation,
            taxRate = this.taxRate,
            moduleSellLevel = this.moduleSellLevel,
            trade_modules = this.trade_modules,
            shipsForSale = this.shipsForSale
        };

        return copy;
    }

    void LateUpdate()
    {  
        WorldSpaceToScreenSpace();
    }
}

[Serializable]
public class XMLStation
{
    public enum Ships { Sparrow, Starhawk, Raptor, Sentinel, Minotaur, Megalodon, Leviathan, Borealis, Austrialis, Operator, Macro, Telsaq, Ankh, Embalmer, Shroud, Lotus, Mimosa, Scarab, Scorpion, Obelisk, Sistrum, Prometheus, Dune, Rorschach, Blaze, Viper, Torque, Cyclone, Hurricane, Nova, Inferno, Phantom, Apex, Halo, Cyrilis, Charon }

    public string name;
    public Vector3 absoluteWorldPosition;
    public float signatureRadius;
    public bool repairsAvailable;
    public float standardDeviation;
    public float taxRate;
    public byte moduleSellLevel;
    public bool trade_modules;
    public List<Ships> shipsForSale;
}