using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text;

public class Tooltip : MonoBehaviour
{
    public Transform ttTrasnsform;
    public static Tooltip instance;
    public TMP_Text text;
    public GameObject panel;
    private string hoveringName = string.Empty;

    public const float AU_IN_METERS =     149597870700f;
    public const float AU_IN_KILOMETERS = 149597870.7f;

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

        StringBuilder stringBuilder = new StringBuilder();

        // if distance is greater than 1/100th of a AU then it's AU
        float one_oneHundrethOfAnAU = AU_IN_METERS / 100.0f;
        float one_kilometer = 1000.0f;

        if (distance >= one_oneHundrethOfAnAU)
        {
            stringBuilder.Append((distance / AU_IN_METERS).ToString("F2"));
            stringBuilder.Append(" AU");
        }
        else if (distance >= one_kilometer)
        {
            stringBuilder.Append((distance / 1000).ToString("F2"));
            stringBuilder.Append(" km");
        }
        else
        {
            stringBuilder.Append(distance.ToString());
            stringBuilder.Append(" m");
        }

        text.text = $"{hoveringName} ({stringBuilder})";
    }


    public void OnAnimationComplete()
    {
        Debug.Log("context menu animation over");
    }
}
