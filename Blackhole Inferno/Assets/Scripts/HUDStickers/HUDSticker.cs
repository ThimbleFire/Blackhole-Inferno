using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class HUDSticker : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

    public static HUDSticker highlightedHUDSticker = null;
    public static HUDSticker selectedHUDSticker = null;
    
    private Image image;
    private RectTransform rectTransform;

    public Vector3 worldPosition;
    public float signatureRadius;
    public bool globalVisibility = false;

    // there's a bit of an oversight with these properties
    // We're checking to see whether distance is within the cameras farClipPlane but the distance is based on the sticker and the ship, not the sticker and the camera.
    // This is a problem because when we zoom out object should get further away, pulling them toward the center of the screen.
    // We need to figure out when to check the distance between the worldPosition sticker and the ship and when to check the distance between the sticker and the camera.
    protected Vector3 direction { get { return (worldPosition - Ship.LPC.worldPosition).normalized; } }
    protected float distance { get { return Vector3.Distance(worldPosition, Ship.LPC.worldPosition); } }
    public Vector3 target3Position { get { return distance < Camera.main.farClipPlane ? worldPosition : direction * (Camera.main.farClipPlane - 10.0f); } }

    public List<ContextMenuOption.Commands> CMOCommands;

    protected virtual void Awake() {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData) {
        switch(eventData.button) {
            case PointerEventData.InputButton.Left:
                selectedHUDSticker = this;
                break;
            case PointerEventData.InputButton.Right:
            ContextMenu.instance.OpenContextMenu(this);
                break;
        }
    }
    public void OnPointerEnter(PointerEventData eventData) {
        Tooltip.instance.Show(this.name);
        highlightedHUDSticker = this;
    }
    public void OnPointerExit(PointerEventData eventData) {
        Tooltip.instance.Hide();
        // could we remove making it null?
        // let's try and find out.
        //if(highlightedHUDSticker == this) {
        //     highlightedHUDSticker = null;
        // }
    }
    public virtual void Arrived() {
        StopCoroutine(DisposeCoroutine());
        Debug.Log("Arrived at " + gameObject.name);
    }
    public virtual void Leaving() {
        StartCoroutine(DisposeCoroutine());
        Debug.Log("Leaving " + gameObject.name);
    }
    private IEnumerator DisposeCoroutine() {
        yield return new WaitForSeconds(30.0f);
        Timeout();
    }
    protected virtual void Timeout() {
    }

    protected void WorldSpaceToScreenSpace() {
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(target3Position);
        image.enabled = viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1 && viewportPoint.z > 0;
        if (image.enabled)
            rectTransform.position = Camera.main.WorldToScreenPoint(target3Position);
    }
}
