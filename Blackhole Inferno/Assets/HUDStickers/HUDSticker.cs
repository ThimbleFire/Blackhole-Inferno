using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HUDSticker : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

    public static HUDSticker highlightedHUDSticker = null;
    private float lastClickTime = 0f;
    private Image image;
    private RectTransform rectTransform;

    public Vector3 worldPosition;
    public float signatureRadius;
    public bool globalVisibility = false;

    public List<ContextMenuOption.Commands> CMOCommands;

    protected virtual void Awake() {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

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
    public virtual void Arrived()
    {
        StopCoroutine(DisposeCoroutine());
    }
    public virtual void Leaving()
    {
        StartCoroutine(DisposeCoroutine());
    }
    private IEnumerator DisposeCoroutine()
    {
        yield return new WaitForSeconds(30.0f);

        Timeout();
    }
    protected virtual void Timeout()
    {

    }
    
    protected void WorldSpaceToScreenSpace() {
        float offset = 10.0f;
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(worldPosition);
        image.enabled = viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1 && viewportPoint.z > 0;
        if (image.enabled)
        {
            float distance = Vector3.Distance(Camera.main.transform.position, worldPosition);
            
            // If we're far away and the object has global visibility, bring it closer
            if(distance > Camera.main.farClipPlane && globalVisibility) {
                Vector3 wpos = Vector3.MoveTowards(Camera.main.transform.position, worldPosition, Camera.main.farClipPlane - offset);
                rectTransform.position = Camera.main.WorldToScreenPoint(wpos);
            }
            // If the object is nearby, just draw it to screen like normal
            else if (distance <= Camera.main.farClipPlane)
            {
                rectTransform.position = Camera.main.WorldToScreenPoint(worldPosition);
            }
            // If it's far away and doesn't have global visibility, hide it
            else if(globalVisibility == false)
            {
                image.enabled = false;
            }
        }
    }
}