using Unity.VisualScripting;
using UnityEngine;

// Camera code courtesy of Veli V
public class CameraMove : MonoBehaviour
{
    public static Vector3 worldPosition = Vector3.zero;
    public static float distance = 30.0f;
    public float xSpeed = 0.25f;
    public float ySpeed = 0.25f;
    public float yMinLimit = -90f;
    public float yMaxLimit = 90f;
    public float distanceMin = 30f;
    public float distanceMax = 5000f;
    public float smoothTime = 2f;
    float rotationYAxis = 0.0f;
    float rotationXAxis = 0.0f;
    float velocityX = 0.0f;
    float velocityY = 0.0f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        rotationYAxis = angles.y;
        rotationXAxis = angles.x;
    }

    void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            velocityX += xSpeed * Input.GetAxis("Mouse X") * distance * 0.02f;
            velocityY += ySpeed * Input.GetAxis("Mouse Y") * distance * 0.02f;
        }

        rotationYAxis += velocityX;
        rotationXAxis -= velocityY;

        rotationXAxis = ClampAngle(rotationXAxis, yMinLimit, yMaxLimit);

        Quaternion toRotation = Quaternion.Euler(rotationXAxis, rotationYAxis, 0);
        Quaternion rotation = toRotation;

        distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);

        RaycastHit hit;
        if (Physics.Linecast(Vector2.zero, transform.position, out hit))
        {
            distance -= hit.distance;
        }
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * negDistance;

        transform.rotation = rotation;
        transform.position = position;

        velocityX = Mathf.Lerp(velocityX, 0, Time.deltaTime * smoothTime);
        velocityY = Mathf.Lerp(velocityY, 0, Time.deltaTime * smoothTime);
    }
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
