using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[XmlRoot("System")]
public class SystemContent
{
    public SystemContent()
    {
        jumpGates = new List<XMLJumpGate>();
        planets = new List<XMLPlanet>();
        stations = new List<XMLStation>();
        sun = new List<XMLSun>();
        belts = new List<XMLBelt>();
    }

    public string Name;

    [XmlArray("JumpGates")]
    public List<XMLJumpGate> jumpGates;
    [XmlArray("Planets")]
    public List<XMLPlanet> planets;
    [XmlArray("Stations")]
    public List<XMLStation> stations;
    [XmlArray("Suns")]
    public List<XMLSun> sun;
    [XmlArray("Belts")]
    public List<XMLBelt> belts;
}