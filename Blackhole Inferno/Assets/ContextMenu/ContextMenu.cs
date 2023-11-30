using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContextMenu : MonoBehaviour
{
    public static ContextMenu instance;

    public bool Opened {get; set;} = false;
    private List<ContextMenuOption> cmos = new List<ContextMenuOption>();
    public GameObject child;
    public GameObject option;
    public HUDSticker currentSticker = null;

    private void Awake() => instance = this;

    private void Update() {
        
        if(Opened == true)
        {
            float distanceX = Input.mousePosition.x - transform.position.x;
            float distanceY = Input.mousePosition.y - transform.position.y;

            Vector2 trasnformSizeDelta = child.GetComponent<RectTransform>().sizeDelta;

            float externalPadding = 20.0f;

            if( distanceX > trasnformSizeDelta.x  / 2 + externalPadding ||
                distanceX < -trasnformSizeDelta.x / 2 - externalPadding ||
                distanceY > trasnformSizeDelta.y  / 2 + externalPadding ||
                distanceY < -trasnformSizeDelta.y / 2 - externalPadding ) {

                ClearCMOS();
            }
        }
    }

    public void OpenContextMenu(HUDSticker sticker)
    {
        if(Opened == false)
        {
            Opened = true;
            currentSticker = sticker;

            // Build the context menu options then calculate its position to the right of the cursor

            if(HUDSticker.highlightedHUDSticker == null)
            {
                // nothing is selected
                
                ContextMenuOption cmo = Instantiate(option, child.transform).GetComponent<ContextMenuOption>();

                cmo.SetText("no context menus available");

                cmos.Add(cmo);
            }
            else
            {
                for(int i = 0; i < sticker.CMOCommands.Count; i++)
                {
                    ContextMenuOption cmo = Instantiate(option, child.transform).GetComponent<ContextMenuOption>();
                    
                    switch(sticker.CMOCommands[i])
                    {
                        case ContextMenuOption.Commands.Align:
                            cmo.SetText("Align");
                            cmo.GetComponent<Button>().onClick.AddListener(() =>
                            CMOS_OnClick_Align(sticker.transform.position));                       
                        break;
                        case ContextMenuOption.Commands.WarpTo:
                            cmo.SetText("Warp");
                            cmo.GetComponent<Button>().onClick.AddListener(() =>
                            CMOS_OnClick_Warp(sticker.transform.position));                       
                        break;
                        case ContextMenuOption.Commands.Dock:
                            cmo.SetText("Dock");
                            cmo.GetComponent<Button>().onClick.AddListener(() =>
                            CMOS_OnClick_Dock(sticker.GetComponent<Station>()));                       
                        break;
                    }                        

                    cmos.Add(cmo);
                }
            }

            // Reposition the context menu so the cursor isn't directly over any options
            float optionHeight = option.GetComponent<RectTransform>().sizeDelta.y;
            //Vector3 offset = new Vector3(-15, -(optionHeight * cmos.Count / 2 + 10));
            transform.position = Input.mousePosition/* + offset*/;
        }
    }

    private void ClearCMOS()
    {
        while (cmos.Count > 0)
        {
            Destroy(cmos[0].gameObject);
            cmos.RemoveAt(0);
        }
        cmos.Clear();
        if(currentSticker != null)
                currentSticker.Deselect();
        currentSticker = null;
        Opened = false;
    }

    private void CMOS_OnClick_Align(Vector3 position) {
        Debug.Log("Align to " + position);
        ClearCMOS();
    }
    private void CMOS_OnClick_Warp(Vector3 position) {
        Debug.Log("Warp to " + position);
        ClearCMOS();
    }
    private void CMOS_OnClick_Dock(Station station) {
        Debug.Log("Dock at " + station.name);
        ClearCMOS();
    }
}