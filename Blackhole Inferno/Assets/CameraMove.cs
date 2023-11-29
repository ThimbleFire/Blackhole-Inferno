using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public static CameraMove instance; // Singleton instance
    public Transform target; // The target object to orbit around
    public float rotationSpeed = 5f;
    public float zoomSpeed = 25f;
    private Vector3 targetPosition = Vector3.zero;

    void Awake()
    {
        // Ensure only one instance of the CameraController exists
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RotateCameraToTarget(Transform _target) => target = _target;

    // orbit the the target
    public void ResetDistance(Vector3 dir, float signatureRadius) => targetPosition = target.position - dir * signatureRadius;

    private void MouseWheel()
    {
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        float zoomAmount = scrollWheel * zoomSpeed;

        transform.Translate(Vector3.forward * zoomAmount, Space.Self);
    }
    private void FaceTarget() {
        
        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    private void Pan()
    {
        // Orbit around the target when left mouse button is held down
        if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Calculate rotation based on mouse movement
            Vector3 orbitRotation = new Vector3(-mouseY, mouseX, 0f) * rotationSpeed;

            // Apply rotation to the camera
            transform.RotateAround(target.position, transform.right, orbitRotation.x);
            transform.RotateAround(target.position, Vector3.up, orbitRotation.y);
        }
    }

    private void Update()
    {
        if (target == null)
            return;
        
        if(targetPosition != Vector3.zero)
        {
            float distance = Vector3.Distance(transform.position, targetPosition);
            float epsilon = .5f; // Adjust this threshold as needed
            if (distance > epsilon)
                transform.position = Vector3.Lerp(transform.position, targetPosition, rotationSpeed * Time.deltaTime);            
            else {
                // If the distance is small enough, consider the position reached
                transform.position = targetPosition;
                targetPosition = Vector3.zero;
            }
        }

        FaceTarget();
        MouseWheel();
        Pan();
    }

    private Vector3 GetDirection(Vector3 from, Vector3 to)
    {
        return to - from;
    }
}