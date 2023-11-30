using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HUDSticker : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

    public static HUDSticker highlightedHUDSticker = null;
    private HUDSticker interactingWithSticker = null;

    // context menu options 
    [HideInInspector] public List<ContextMenuOption.Commands> CMOCommands;

    // input
    private float lastClickTime = 0f;

    // warp to
    protected float signatureRadius = 65.0f;
    private bool finishedWarping = true;
    private float warpSpeed = 3.5f;

    // theoretical position
    public Vector3 absoluteWorldPosition;

    // theoretical rotation
    public Vector3 rot = Vector3.zero;
    private bool finishedRotating = true;
    private float rotationSpeed = 3.5f;

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
                CameraMove.instance.ResetDistance(transform.forward, signatureRadius);        
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
    
    private void Update() {
        // Make the canvas face the camera
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                         Camera.main.transform.rotation * Vector3.up);

        // Resize the UI element so that regardless of zoom, it shows at the correct size. we can entirely remove this segment by using screen space overlay, might be more to it though
        var size = (Camera.main.transform.position - transform.position).magnitude; 
        
        float scale = 0.003f;
        transform.localScale = new Vector3(size,size,size) * scale; 

        // rotate the theoretical direction to allow for accurate forward propulsion
        // transform rotation is reserved for facing the camera
        if (finishedRotating == false)
        {
            float t = Mathf.Clamp01(Time.deltaTime * rotationSpeed);
            rot = Vector3.Slerp(rot, interactingWithSticker.absoluteWorldPosition, t);

            // Check if the rotation is complete
            if (Vector3.Angle(rot, interactingWithSticker.absoluteWorldPosition) < 0.1f)
            {
                rot = interactingWithSticker.absoluteWorldPosition;
                finishedRotating = true;
                Debug.Log("aligned");
            }
        }
        else if (finishedWarping == false)
        {
            // Move transform forward in the direction of rot
            float warpStep = warpSpeed * Time.deltaTime;
            absoluteWorldPosition = Vector3.Lerp(absoluteWorldPosition, interactingWithSticker.absoluteWorldPosition, warpStep);
            transform.position = absoluteWorldPosition;
            
            Debug.Log(Vector3.Distance(absoluteWorldPosition, interactingWithSticker.absoluteWorldPosition));

            // Check if the warping is complete
            if (Vector3.Distance(absoluteWorldPosition, interactingWithSticker.absoluteWorldPosition) < interactingWithSticker.signatureRadius) {
                finishedWarping = true;
            }
        }
    }

    public void SetRotateTo(HUDSticker sticker) {
        interactingWithSticker = sticker;
        finishedRotating = false;
    }
    
    public void SetWarpTo(HUDSticker sticker) {
        interactingWithSticker = sticker;
        finishedWarping = false;
    }
}