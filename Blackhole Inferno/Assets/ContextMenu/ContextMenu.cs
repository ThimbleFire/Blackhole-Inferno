using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContextMenu : MonoBehaviour
{
    public bool Opened {get; set;} = false;
    private List<ContextMenuOption> cmos = new List<ContextMenuOption>();
    public GameObject child;
    public GameObject option;

    private void Awake()
    {
        Debug.Log(gameObject.name);
    }

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
                Opened = false;
            }
        }
        else if (Input.GetMouseButton(1))
        {
            if(Opened == false)
            {
                Opened = true;

                // Build the context menu options then calculate its position to the right of the cursor

                if(HUDSticker.highlightedHUDSticker == null)
                {
                    // nothing is selected

                    ClearCMOS();

                    ContextMenuOption cmo_align = Instantiate(option, child.transform).GetComponent<ContextMenuOption>();
                    cmo_align.SetText("no context menus available, try mousing over a sticker");
                    cmos.Add(cmo_align);
                }
                else
                {
                    ClearCMOS();

                    for(int i = 0; i < HUDSticker.highlightedHUDSticker.CMOCommands.Count; i++)
                    {
                        ContextMenuOption cmo = Instantiate(option, child.transform).GetComponent<ContextMenuOption>();
                        cmo.SetText(HUDSticker.highlightedHUDSticker.CMOCommands[i].ToString());
                        cmos.Add(cmo);
                    }
                }

                // Reposition the context menu so the cursor isn't directly over any options
                float optionHeight = option.GetComponent<RectTransform>().sizeDelta.y;
                Vector3 offset = new Vector3(-35, -(optionHeight * cmos.Count / 2 + 18));
                transform.position = Input.mousePosition + offset;
            }
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
    }
}
