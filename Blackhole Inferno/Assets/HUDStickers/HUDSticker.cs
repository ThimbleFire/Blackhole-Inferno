using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HUDSticker : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

    public static HUDSticker selectedHUDSticker = null;
    public static HUDSticker highlightedHUDSticker = null;

    [HideInInspector]
    public List<ContextMenuOption.Commands> CMOCommands;

    private float lastClickTime = 0f;
    protected float signatureRadius = 65.0f;

    private Vector3 theoreticalFacingDirection = Vector3.zero;
    private Vector3 absoluteWorldPosition;

    // Align
    private Vector3 rot = Vector3.zero;
    private Vector3 toRot = Vector3.zero;
    private bool finishedRotating = true;
    private float rotationSpeed = 2f;

    private void Awake() => absoluteWorldPosition = transform.position;
    public void OnPointerClick(PointerEventData eventData)
    {
        float currentTime = Time.time;
        float doubleClickTimeThreshold = 0.3f;

        if (currentTime - lastClickTime < doubleClickTimeThreshold)
        {
            CameraMove.instance.ResetDistance(transform.forward, signatureRadius);        
        }
        else
        {
            lastClickTime = currentTime;
            //CameraMove.instance.RotateCameraToTarget(rectTransform);
            if(selectedHUDSticker != null)
                selectedHUDSticker.Deselect();
            GetComponent<UnityEngine.UI.Image>().color = new Color(1.0f, 1.0f, 0.0f, 1.0f);
            selectedHUDSticker = this;
        }
    }
    public void Deselect()
    {
        GetComponent<UnityEngine.UI.Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Tooltip.instance.Set($"{gameObject.name} ({Vector3.Distance(absoluteWorldPosition, CameraMove.instance.target.position)}) ");
        
        highlightedHUDSticker = this;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.instance.Hide();

        if(highlightedHUDSticker == this) {
            highlightedHUDSticker = null;
        }
    }
    
    void Update() {
        // Make the canvas face the camera
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                         Camera.main.transform.rotation * Vector3.up);

        // Resize the UI element so that regardless of zoom, it shows at the correct size
        var size = (Camera.main.transform.position - transform.position).magnitude; 
        float scale = 0.005f;
        transform.localScale = new Vector3(size,size,size) * scale; 

        // Set the transform position at most 995 distance away to prevent clipping
        // This is not the true position of the Entity, just its UI element
        float distanceFromCamera = Vector3.Distance(absoluteWorldPosition, Camera.main.transform.position);
        if( distanceFromCamera > 995)        
            transform.position = absoluteWorldPosition - transform.forward * (distanceFromCamera - 995.0f);
        else        
            transform.position = absoluteWorldPosition;

        //
        if (finishedRotating == false)
        {
            float t = Mathf.Clamp01(Time.deltaTime * rotationSpeed);
            rot = Vector3.Slerp(rot, toRot, t);
            
            // Check if the rotation is complete
            if (Vector3.Angle(rot, toRot) < 0.1f) {
                rot = toRot;
                finishedRotating = true;
            }
        }
    }

    public void SetRotateTo(Vector3 r) {
        toRot = r;
        finishedRotating = false;
    }
}