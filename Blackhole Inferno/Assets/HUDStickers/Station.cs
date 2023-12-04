using System;
using System.Collections.Generic;
using UnityEngine;

public class Station : HUDSticker
{
    public XMLStation station;

    void Awake()
    {
        this.name = station.name;
        this.signatureRadius = station.signatureRadius;
        this.absoluteWorldPosition = station.absoluteWorldPosition;
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