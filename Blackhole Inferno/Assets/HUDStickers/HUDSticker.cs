using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HUDSticker : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

    public static HUDSticker highlightedHUDSticker = null;
    private float lastClickTime = 0f;
    public Image image;
    
    public float signatureRadius = 65.0f;

    /// <summary>
    ///  X = left & right
    ///  Y = up & down
    ///  Z = forward & backward
    /// </summary>
    public Vector3 worldPosition;
    public List<ContextMenuOption.Commands> CMOCommands;

    public RectTransform rectTransform;

    public Sprite Sprite {get{return GetComponent<UnityEngine.UI.Image>().sprite; } }

    public void OnPointerClick(PointerEventData eventData) {
        if(eventData.button == PointerEventData.InputButton.Left) {
            float currentTime = Time.time;
            float doubleClickTimeThreshold = 0.3f;
            // double-click
            if (currentTime - lastClickTime < doubleClickTimeThreshold) {
                //CameraMove.instance.ResetDistance(transform.forward, signatureRadius);        
            }
            // single-click
            else lastClickTime = currentTime;
        }
        if( eventData.button == PointerEventData.InputButton.Right ) {
            ContextMenu.instance.OpenContextMenu(this);
        }
    }
    public void OnPointerEnter(PointerEventData eventData) {
        Tooltip.instance.Show(this.name);
        highlightedHUDSticker = this;
    }
    public void OnPointerExit(PointerEventData eventData) {
        Tooltip.instance.Hide();
       if(highlightedHUDSticker == this) {
            highlightedHUDSticker = null;
        }
    } 
    
    protected void WorldSpaceToScreenSpace() {
        float offset = 10.0f;
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(worldPosition);
        image.enabled = viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1 && viewportPoint.z > 0;
        if (image.enabled)
        {
            float distance = Vector3.Distance(Camera.main.transform.position, worldPosition);
            if(distance > Camera.main.farClipPlane) {
                Vector3 wpos = Vector3.MoveTowards(Camera.main.transform.position, worldPosition, Camera.main.farClipPlane - offset);
                rectTransform.position = Camera.main.WorldToScreenPoint(wpos);
            }
            else rectTransform.position = Camera.main.WorldToScreenPoint(worldPosition);
        }
    }
}