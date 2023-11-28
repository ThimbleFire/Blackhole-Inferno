using UnityEngine;
using UnityEngine.EventSystems;

public class Entity : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

    public float debugLineDistance = 500.0f;

    public float scale = 0.005f;
    public Transform rectTransform;

    private float lastClickTime = 0f;
    public float doubleClickTimeThreshold = 0.3f;

    public Vector3 absoluteWorldPosition;
    public Vector3 uiPosition;

    private void Awake() => absoluteWorldPosition = transform.position;
    public void OnPointerClick(PointerEventData eventData)
    {
        float currentTime = Time.time;

        if (currentTime - lastClickTime < doubleClickTimeThreshold)
            CameraMove.instance.ResetDistance(transform.forward);        
        else
        {
            lastClickTime = currentTime;
            CameraMove.instance.RotateCameraToTarget(rectTransform);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Tooltip.instance.Set($"{gameObject.name} ({Vector3.Distance(absoluteWorldPosition, CameraMove.instance.target.position)}) ");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.instance.Hide();
    }
    
    void Update() {
        // Make the canvas face the camera
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                         Camera.main.transform.rotation * Vector3.up);

        // Resize the UI element so that regardless of zoom, it shows at the correct size
        var size = (Camera.main.transform.position - transform.position).magnitude; 
        transform.localScale = new Vector3(size,size,size) * scale; 

        // Set the transform position at most 995 distance away to prevent clipping
        // This is not the true position of the Entity, just its UI element
        float distanceFromCamera = Vector3.Distance(absoluteWorldPosition, Camera.main.transform.position);
        if( distanceFromCamera > 995)        
            transform.position = absoluteWorldPosition - transform.forward * (distanceFromCamera - 995.0f);
        else        
            transform.position = absoluteWorldPosition;
        
    }
}