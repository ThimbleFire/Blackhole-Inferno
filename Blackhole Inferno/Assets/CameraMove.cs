using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public static CameraMove instance; // Singleton instance
    public RectTransform target; // The target object to orbit around
    public float rotationSpeed = 5f;
    public float zoomSpeed = 25f;

    public float dragSpeed = 1.0f;
    public float inertiaFactor = 1.0f;
    private bool isDragging = false;
    private Vector3 lastMousePos;
    private Vector3 dragVelocity;

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

    private void LateUpdate() {
        if (target == null)
            return;
        Orbit();
        Zoom();
    }

    private void Orbit() {

        // Orbit around the target when left mouse button is held down
        if (Input.GetMouseButtonDown(0)) {
            isDragging = true;
            lastMousePos = Input.mousePosition;
            dragVelocity = Vector3.zero;
        }
        else if (Input.GetMouseButtonUp(0)) {
            isDragging = false;
        }

        if (isDragging) {

            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Calculate rotation based on mouse movement
            Vector3 orbitRotation = new Vector3(-mouseY, mouseX, 0f) * rotationSpeed;

            // Apply rotation to the camera
            transform.RotateAround(target.position, transform.right, orbitRotation.x);
            transform.RotateAround(target.position, Vector3.up, orbitRotation.y);

            // Calculate drag velocity
            Vector3 currentMousePos = Input.mousePosition;
            dragVelocity = (currentMousePos - lastMousePos) * dragSpeed;
            lastMousePos = currentMousePos;
        }
        else {

            // Apply inertia/drag effect
            transform.RotateAround(target.position, transform.right, -dragVelocity.y * Time.deltaTime);
            transform.RotateAround(target.position, Vector3.up, dragVelocity.x * Time.deltaTime);

            // Gradually decrease drag velocity over time
            dragVelocity = Vector3.Lerp(dragVelocity, Vector3.zero, Time.deltaTime * inertiaFactor);
        } 
    }

    private void Zoom() {

        // Zooming functionality
        //float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        //float zoomAmount = scrollWheel * zoomSpeed;
        //float currentZoomDistance = Vector3.Distance(transform.position, target.position);
        //float newZoomDistance = Mathf.Clamp(currentZoomDistance - zoomAmount, 1, 128);
        
        // Update camera position based on the new zoom distance
       transform.position = target.position - transform.forward * 10;
    }

    private Vector3 GetDirection(Vector3 from, Vector3 to)
    {
        return to - from;
    }
}