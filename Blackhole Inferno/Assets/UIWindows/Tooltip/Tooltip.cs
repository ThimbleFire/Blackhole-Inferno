using UnityEngine;
using TMPro;

public class Tooltip : MonoBehaviour
{
    public Transform ttTrasnsform;
    public static Tooltip instance;
    public TMP_Text text;
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
        text.enabled = false;
    }

    public void Show(string nameOfItemBeingHoveredOver)
    {
        hoveringName = nameOfItemBeingHoveredOver;
        text.enabled = true;
    }

    // This only updates while mousing over an entity
    private void Update() {
        if(text.enabled == false)
           return;
        
        ttTrasnsform.position = Input.mousePosition + Vector3.right * text.preferredWidth / 2;
            
        float distance = Vector3.Distance(Ship.LPC.absoluteWorldPosition, HUDSticker.highlightedHUDSticker.absoluteWorldPosition);

        string distanceText;
        if (distance > 150.0f) {
            distanceText = (distance / 1500.0f).ToString("F2") + " AU";
        } else {
            distanceText = (distance * 1000.0f).ToString("F2") + " km"; // Convert to km for distances less than 1500 units
        }

        text.text = $"{hoveringName} ({distanceText})";
    }
    
    public void OnAnimationComplete()
    {
        Debug.Log("context menu animation over");
    }
}
