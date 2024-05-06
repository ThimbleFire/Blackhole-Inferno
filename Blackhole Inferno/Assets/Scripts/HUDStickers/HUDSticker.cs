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
    public float distanceToCamera;
    public float distanceToShip;

    public List<ContextMenuOption.Commands> CMOCommands;

    private ScaleOnDistance model;

    protected virtual void Awake() {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData) {
        switch (eventData.button) {
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
    private Vector3 Direction(Vector3 from, Vector3 to) { return (to - from).normalized; }
    
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

    protected void WorldSpaceToScreenSpace()
    {
        Vector3 cameraPositionOffsetByShip = Ship.LPC.worldPosition + Camera.main.transform.position;
        Vector3 directionFromCameraToSticker = Direction(worldPosition, cameraPositionOffsetByShip);

        distanceToCamera = Vector3.Distance(cameraPositionOffsetByShip, worldPosition);
        distanceToShip = Vector3.Distance(worldPosition, Ship.LPC.worldPosition);

        // new plan. If the HUDSticker is within 70K distance from the camera, stop using worldPosition. Instead, start using transform.position


        Vector3 newWorldPointBasedOnCamera;
        if (distanceToCamera < Camera.main.farClipPlane)
        {
            // ONCE WE'RE HERE WE HAVE TO USE AN ENTIRELY DIFFERENT COORDINATE SYSTEM. WE CANNOT RELY ON WORLDPOSITION BECAUSE IT CAUSES ROUNDING ERRORS
            // THIS CODE DOESN'T FUCKING WORK, FIX IT, LOSER!

            // first project the object away from the ship to its worldPosition, but wouldn't that just be its normal world position?
            Vector3 directionFromShipToWorldPosition = Direction(worldPosition, Ship.LPC.worldPosition);
            Vector3 worldPosiitonBasedOnShipAndWorldPosition = directionFromShipToWorldPosition * distanceToShip;

            // then project that world position 
            Vector3 diretionBetweenZeroAndWorldPosition = Direction(worldPosiitonBasedOnShipAndWorldPosition, Vector3.zero);
            float wp2 = Vector3.Distance(cameraPositionOffsetByShip, worldPosiitonBasedOnShipAndWorldPosition);
            newWorldPointBasedOnCamera = diretionBetweenZeroAndWorldPosition * wp2;
        
            // then project that position from the camera
        }
        else newWorldPointBasedOnCamera = directionFromCameraToSticker * (Camera.main.farClipPlane - 10.0f);
        

        // transform the new sticker position into screen coordinates
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(newWorldPointBasedOnCamera);

        // toggle the stickers visibility depending on whether it's visible on screen
        image.enabled = viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1 && viewportPoint.z >= 0;

        if (image.enabled)
            // if it is, set it's transform
            rectTransform.position = Camera.main.WorldToScreenPoint(newWorldPointBasedOnCamera);

        if (model != null)
            model.transform.position = newWorldPointBasedOnCamera;
    }
}
