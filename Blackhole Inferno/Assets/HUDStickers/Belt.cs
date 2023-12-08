using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Belt : HUDSticker
{
    public bool loaded = false;
    public GameObject astroidPrefab;
    public List<GameObject> asteroids;

    internal void Load(XMLBelt belt)
    {
        this.name = belt.name;
        this.signatureRadius = belt.signatureRadius;
        this.worldPosition = belt.absoluteWorldPosition;
    }
    
    public XMLBelt Save()
    {
        XMLBelt copy = new XMLBelt
        {
            name = this.name,
            signatureRadius = this.signatureRadius,
            absoluteWorldPosition = this.worldPosition,
        };

        return copy;
    }

    void LateUpdate()
    {  
        WorldSpaceToScreenSpace();
    }

    public override void Arrived()
    {
        if(loaded)
            return;

        loaded = true;
    
        asteroids = new List<GameObject>();
        // iterate the number of astroids in the belt
        int count = UnityEngine.Random.Range(30, 100);
        for(int i = 0; i < count; i++)
        {
            // calculate the angle they should appear at
            float angle = UnityEngine.Random.Range(0, Mathf.PI /* * 2.0f */);

            // calculate the distance they should be from the center of the belt
            float distance = UnityEngine.Random.Range(10.0f, 13.0f);

            // Convert spherical coordinates to Cartesian coordinates
            float x = distance * Mathf.Cos( angle );
            float y = distance * Mathf.Sin( angle );

            // Create a new position relative to the center point
            Vector3 randomPosition = worldPosition + new Vector3(x, UnityEngine.Random.Range(-1.0f, 1.0f), y);

            Astroid asteroid = Instantiate(astroidPrefab, transform.parent).GetComponent<Astroid>();
        
            asteroid.worldPosition = randomPosition;

            asteroids.Add(asteroid.gameObject);
        }

        Debug.Log("Arrived at " + gameObject.name);
    }

    public override void Leaving()
    {
        base.Leaving();
        
        Debug.Log("Leaving " + gameObject.name);
    }

    protected override void Timeout() {
        foreach(GameObject child in asteroids) {
            Destroy(child);
        }
        loaded = false;
        asteroids.Clear();
        
        Debug.Log("Disposed children belonging to " + gameObject.name);
    }    
}

[Serializable]
public class XMLBelt
{
    public enum Minerals { Verillion, Neok, Coxyine, Terylium }

    public string name;
    public Vector3 absoluteWorldPosition;
    public float signatureRadius;
    public string beltPrefabName;
}
