using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Entity : MonoBehaviour, IPointerClickHandler {
    public float scale = 0.005f;
    public Transform rectTransform;

    private float lastClickTime = 0f;
    public float doubleClickTimeThreshold = 0.3f;

    public void OnPointerClick(PointerEventData eventData)
    {
        float currentTime = Time.time;

        if (currentTime - lastClickTime < doubleClickTimeThreshold)
        {
            // Double-click detected
            CameraMove.instance.ResetDistance(transform.forward);
        }
        else
        {
            // Single click, record the time
            lastClickTime = currentTime;
            // Select the node
            CameraMove.instance.RotateCameraToTarget(rectTransform);
        }
    }

    void Update() {
        // Make the canvas face the camera
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                         Camera.main.transform.rotation * Vector3.up);

        var size = (Camera.main.transform.position - transform.position).magnitude; 
        transform.localScale = new Vector3(size,size,size) * scale; 
    }
}