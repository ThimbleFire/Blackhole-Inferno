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
    public void OnPointerExit(PointerEventData eventData) => Tooltip.instance.Hide();
    private Vector3 Direction(Vector3 from, Vector3 to) => return new Vector3(to - from).normalized;
    
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

        Vector3 directionFromCameraWorldPositionToWorldPosition = Direction(CameraMove.worldPosition, worldPosition);
        float distanceBetweenCameraWorldPositionAndStickerWorldPosition = Vector3.Distance(CameraMove.worldPosition, worldPosition);
        
        // calculate stickers new position. Use its real position unless it's outside the cameras view depth
        Vector3 newWorldPointBasedOnCamera = distanceBetweenCameraWorldPositionAndStickerWorldPosition < Camera.main.farClipPlane ? worldPosition : directionFromCameraWorldPositionToWorldPosition * (Camera.main.farClipPlane - 10.0f);

        // transform the new sticker position into screen coordinates
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(newWorldPointBasedOnCamera);

        // toggle the stickers visibility depending on whether it's visible on screen
        image.enabled = viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1 && viewportPoint.z > 0;
        
        if (image.enabled)
            // if it is, set it's transform
            rectTransform.position = Camera.main.WorldToScreenPoint(newWorldPointBasedOnCamera);
    }
}
