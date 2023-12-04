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
}