using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HUDSticker : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

    public static HUDSticker highlightedHUDSticker = null;

    // context menu options 
    [HideInInspector] public List<ContextMenuOption.Commands> CMOCommands;

    // input
    private float lastClickTime = 0f;

    // warp to
    public float signatureRadius = 65.0f;

    // theoretical position
    public Vector3 absoluteWorldPosition;

    // theoretical rotation
    public Vector3 rot = Vector3.zero;

    private void Awake() => absoluteWorldPosition = transform.position;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            float currentTime = Time.time;
            float doubleClickTimeThreshold = 0.3f;

            // double-click
            if (currentTime - lastClickTime < doubleClickTimeThreshold) 
            {
                //CameraMove.instance.ResetDistance(transform.forward, signatureRadius);        
            }
            // single-click
            else 
            {
                lastClickTime = currentTime;
            }
        }
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            ContextMenu.instance.OpenContextMenu(this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Tooltip.instance.Set($"{gameObject.name} ({Vector3.Distance(absoluteWorldPosition, Ship.LPC.absoluteWorldPosition)}) ");
        
        highlightedHUDSticker = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.instance.Hide();

        if(highlightedHUDSticker == this) {
            highlightedHUDSticker = null;
        }
    }
    
    
    protected virtual void Update() {

        // Make the canvas face the camera
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                         Camera.main.transform.rotation * Vector3.up);

        // Resize the UI element so that regardless of zoom, it shows at the correct size. we can entirely remove this segment by using screen space overlay, might be more to it though
        var size = (Camera.main.transform.position - transform.position).magnitude; 
        
        float scale = 0.003f;
        transform.localScale = new Vector3(size,size,size) * scale; 
    }
}