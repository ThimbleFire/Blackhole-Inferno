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
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void LateUpdate()
    {
        HandleInput();
        OrbitCamera();
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastMousePos = Input.mousePosition;
            dragVelocity = Vector3.zero;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            Vector3 orbitRotation = new Vector3(-mouseY, mouseX, 0f) * rotationSpeed;

            transform.RotateAround(target.position, transform.right, orbitRotation.x);
            transform.RotateAround(target.position, Vector3.up, orbitRotation.y);

            Vector3 currentMousePos = Input.mousePosition;
            dragVelocity = (currentMousePos - lastMousePos) * dragSpeed;
            lastMousePos = currentMousePos;
        }
        else
        {
            dragVelocity = Vector3.Lerp(dragVelocity, Vector3.zero, Time.deltaTime * inertiaFactor);
        }
        
    }

    void OrbitCamera()
    {
        // Separate camera for looking at the target without affecting its position
        Camera lookAtCamera = new GameObject("LookAtCamera").AddComponent<Camera>();
        lookAtCamera.transform.position = transform.position;
        lookAtCamera.transform.rotation = transform.rotation;

        // Move the look-at camera to orbit around the target
        lookAtCamera.transform.RotateAround(target.position, transform.right, -dragVelocity.y * Time.deltaTime);
        lookAtCamera.transform.RotateAround(target.position, Vector3.up, dragVelocity.x * Time.deltaTime);

        // Update the main camera's position and rotation based on the look-at camera
        transform.position = lookAtCamera.transform.position;
        transform.rotation = lookAtCamera.transform.rotation;

        // Clean up the temporary look-at camera
        Destroy(lookAtCamera.gameObject);
        
        // Zoom
        transform.position = target.position - transform.forward * 10;
    }
}