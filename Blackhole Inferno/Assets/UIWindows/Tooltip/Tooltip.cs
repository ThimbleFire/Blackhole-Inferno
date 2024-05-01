using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public Transform ttTrasnsform;
    public static Tooltip instance;
    public TMP_Text text;
    public GameObject panel;
    private string hoveringName = string.Empty;

    void Awake() {
        if (instance == null) {
            instance = this;
            Hide();
        }
        else {
            Destroy(gameObject);
        }
    }

    public void Hide()
    {
        panel.SetActive(false);
    }

    public void Show(string nameOfItemBeingHoveredOver)
    {
        hoveringName = nameOfItemBeingHoveredOver;
        panel.SetActive(true);
    }

    // This only updates while mousing over an entity
    private void Update() {
        if(panel.activeInHierarchy == false)
           return;
        
        ttTrasnsform.position = Input.mousePosition + Vector3.right * text.preferredWidth / 2;
            
        float distance = Vector3.Distance(Ship.LPC.worldPosition, HUDSticker.highlightedHUDSticker.worldPosition);

        //if (distance >= 1495978.707f) distanceText += "AU"; // if distance > 0.01 AU, write AU
        //else distanceText += " km"; // else write km, like 1495978.7 km
        float d = distance >= 1.0 ? distance / 149597870.7f : distance * 100.0f;
        text.text = $"{hoveringName} {d:F2} {(distance >= 1495978.707f ? "AU" : "km")}";
    }
    
    public void OnAnimationComplete()
    {
        Debug.Log("context menu animation over");
    }
}
