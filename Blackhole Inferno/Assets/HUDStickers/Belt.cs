using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Belt : HUDSticker
{
    public bool loaded = false;
    public GameObject astroidPrefab;

    private const float DespawnInterval = 900.0f;
    private float despawnTimer = DespawnInterval;

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

    void Update() {
        if(loaded && despawnTimer > 0.0f) {
            despawnTimer -= Time.deltaTime;
            if(despawnTimer <= 0.0f) {
                foreach(Transform child in transform) {
                    Destroy(child.gameObject);
                    loaded = false;
                }
            }
        }
    }

    void LateUpdate()
    {  
        WorldSpaceToScreenSpace();
    }

    public override void Arrived()
    {
        despawnTimer = DespawnInterval;
        
        if(loaded)
            return;

        loaded = true;
    
        // iterate the number of astroids in the belt
        for(int i = 0; i < 30; i++)
        {
            // calculate the angle they should appear at
            float angle = UnityEngine.Random.Range(0, Mathf.PI /* * 2.0f */);

            // calculate the distance they should be from the center of the belt
            float distance = UnityEngine.Random.Range(10.0f, 13.0f);

            // Convert spherical coordinates to Cartesian coordinates
            float x = distance * Mathf.Cos( angle );
            float y = distance * Mathf.Sin( angle );

            // Create a new position relative to the center point
            Vector3 randomPosition = worldPosition + new Vector3(x, UnityEngine.Random.Range(-5.0f, 5.0f), y);

            Astroid astroid = Instantiate(astroidPrefab, transform).GetComponent<Astroid>();
        
            astroid.worldPosition = randomPosition;
        }
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
