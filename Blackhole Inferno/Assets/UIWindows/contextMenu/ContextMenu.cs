using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContextMenu : MonoBehaviour
{
    public static ContextMenu instance;

    public bool Opened {get; set;} = false;
    private List<ContextMenuOption> cmos = new List<ContextMenuOption>();
    public RectTransform child;
    public Transform cmTransform;
    public GameObject option;
    public HUDSticker currentSticker = null;
    public Ship playerShip;
    public UIToggle uIToggle;

    private void Awake() => instance = this;

    private void Update() {
        
        if(Opened == true)
        {
            float distanceX = Input.mousePosition.x - cmTransform.position.x;
            float distanceY = Input.mousePosition.y - cmTransform.position.y;

            Vector2 transformSizeDelta = child.sizeDelta;

            float externalPadding = 20.0f;

            if( distanceX > transformSizeDelta.x  / 2 + externalPadding ||
                distanceX < -transformSizeDelta.x / 2 - externalPadding ||
                distanceY > transformSizeDelta.y  / 2 + externalPadding ||
                distanceY < -transformSizeDelta.y / 2 - externalPadding ) {

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
                            cmo.GetComponent<Button>().onClick.AddListener(() => playerShip.SetRotateTo(sticker));
                            cmo.GetComponent<Button>().onClick.AddListener(() => ClearCMOS());                       
                        break;
                        case ContextMenuOption.Commands.WarpTo:
                            cmo.SetText("Warp");
                            cmo.GetComponent<Button>().onClick.AddListener(() => playerShip.SetWarpTo(sticker));
                            cmo.GetComponent<Button>().onClick.AddListener(() => ClearCMOS());  
                        break;
                        case ContextMenuOption.Commands.Dock:
                            cmo.SetText("Dock");
                            //cmo.GetComponent<Button>().onClick.AddListener(() => CMOS_OnClick_Dock(sticker.GetComponent<Station>()));
                            cmo.GetComponent<Button>().onClick.AddListener(() => ClearCMOS());  
                        break;
                    }                        

                    cmos.Add(cmo);
                }
            }

            // Reposition the context menu so the cursor isn't directly over any options
            float optionHeight = option.GetComponent<RectTransform>().sizeDelta.y;
            Vector3 offset = new Vector3(-15, -(optionHeight * cmos.Count / 2));
            cmTransform.position = Input.mousePosition + offset;

            LayoutRebuilder.ForceRebuildLayoutImmediate(child);
            uIToggle.Enable();
        }
    }    

    private void ClearCMOS()
    {
        Opened = false;        
        while (cmos.Count > 0)
        {
            Destroy(cmos[0].gameObject);
            cmos.RemoveAt(0);
        }
        cmos.Clear();
        currentSticker = null;
    }

    public void OnAnimationComplete()
    {


    }
}