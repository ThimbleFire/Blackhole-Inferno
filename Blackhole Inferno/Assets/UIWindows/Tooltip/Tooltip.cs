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
            
        float distance = Vector3.Distance(Ship.LPC.worldPosition, HUDSticker.highlightedHUDSticker.worldPosition);

        string distanceText = string.Empty;

        if(HUDSticker.highlightedHUDSticker.globalVisibility)
        {
            if (distance > 1500.0f) {
                distanceText = $"{(distance / 1500.0f).ToString("F2")} AU";
            }
            else if (distance > 1000f) {
                distanceText = $"{(distance / 1.5f).ToString("F2")} km";
            }
        }
        else
        {
            if (distance > 1.0f) {
                distanceText = $"{(distance / 1.0f).ToString("F2")} km";
            }
            else {
                distanceText = $"({distance}  m)";
            }
        }

        /* //1km = 1.0f
        if (distance > 150000000)
            distanceText = (distance / 150000000.0f).ToString("F2") + " AU";
        else if (distance > 1500000)
            distanceText = (distance / 1500000.0f).ToString("F2") + " km";
        else distanceText = Mathf.FloorToInt(distance) + " m";
        */

        text.text = $"{hoveringName} {distanceText}";
    }
    
    public void OnAnimationComplete()
    {
        Debug.Log("context menu animation over");
    }
}
