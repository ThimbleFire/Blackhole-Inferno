using UnityEngine;
using UnityEditor;
using System;

public class SystemEditor : BaseEditor
{    
    //
    private SystemContent systemContent = new SystemContent();
    private TextAsset obj;
    //

    [MenuItem("Window/Editor/System Editor")]
    private static void ShowWindow()
    {
        GetWindow(typeof(SystemEditor));
    }

    protected override void CreationWindow()
    {
        //PaintLoadList();
        obj = PaintXMLLookup(obj, "Resource File", true);
        if (PaintButton("Save")) {
            Save();
        }
        PaintTextField(ref systemContent.Name, "System Name");
    }

    protected override void LoadProperties(TextAsset asset)
    {
        systemContent = XMLUtility.Load<SystemContent>(asset);

        if(systemContent == null)
            return;

        RectTransform canvas = ClearCanvas();

        GameObject prefabPlanet = Resources.Load<GameObject>("Prefabs/Planet");
        foreach (XMLPlanet _planet in systemContent.planets) {
            Instantiate(prefabPlanet, canvas).GetComponent<Planet>().Load(_planet);
        }
        GameObject prefabStation = Resources.Load<GameObject>("Prefabs/Station");
        foreach (XMLStation _station in systemContent.stations) {
            Instantiate(prefabStation, canvas).GetComponent<Station>().Load(_station);
        }
        GameObject prefabSun = Resources.Load<GameObject>("Prefabs/Sun");
        foreach (XMLSun _sun in systemContent.sun) {
            Instantiate(prefabSun, canvas).GetComponent<Sun>().Load(_sun);
        }
        GameObject prefabJumpGate = Resources.Load<GameObject>("Prefabs/JumpGate");
        foreach (XMLJumpGate _jumpGate in systemContent.jumpGates) {
            Instantiate(prefabJumpGate, canvas).GetComponent<JumpGate>().Load(_jumpGate);
        }
    }

    protected override void OnClick_SaveButton()
    {
        
    }

    protected override void Save()
    {
        systemContent.jumpGates.Clear();
        systemContent.planets.Clear();
        systemContent.sun.Clear();
        systemContent.stations.Clear();

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Gate")) {
            systemContent.jumpGates.Add(obj.GetComponent<JumpGate>().jumpGate);
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Planet")) {
            systemContent.planets.Add(obj.GetComponent<Planet>().planet);
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Sun")) {
            systemContent.sun.Add(obj.GetComponent<Sun>().sun);
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Station")) {
            systemContent.stations.Add(obj.GetComponent<Station>().station);
        }

        XMLUtility.Save<SystemContent>(systemContent, "Resources/Systems/", systemContent.Name);
    }

    private RectTransform ClearCanvas()
    {        
        RectTransform canvasWorld = GameObject.Find("Canvas-World").GetComponent<RectTransform>();

        foreach(RectTransform obj in canvasWorld.GetComponentsInChildren<RectTransform>()) {
            if(obj != canvasWorld)
                DestroyImmediate(obj.gameObject);
        }

        return canvasWorld;
    }
}