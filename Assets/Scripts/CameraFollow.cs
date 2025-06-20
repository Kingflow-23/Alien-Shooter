using UnityEngine;

[AddComponentMenu("First Person Camera/Third Person Camera")]
public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Camera Settings")]
    public Vector3 thirdPersonOffset = new Vector3(0f, 2f, -4f);  // for third person
    public Vector3 firstPersonOffset = new Vector3(0f, 2.3f, 0f); // for first person (head level)
        
    public float mouseSensitivityX = 4f;
    public float mouseSensitivityY = 1f;
    public float pitchMin = -60f;
    public float pitchMax = 80f;
    public float smoothTime = 0.1f;

    private Vector3 currentVelocity;
    private float yaw;
    private float pitch;
    private bool isFirstPerson = true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
    }

    void Update()
    {
        // Toggle perspective with C key
        if (Input.GetKeyDown(KeyCode.C))
        {
            isFirstPerson = !isFirstPerson;
        }

        // Update camera rotation
        yaw += Input.GetAxis("Mouse X") * mouseSensitivityX;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivityY;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);
    }

    void LateUpdate()
    {
        if (!target) return;

        // Pick the correct offset
        Vector3 offset = isFirstPerson ? firstPersonOffset : thirdPersonOffset;

        // Calculate new position and rotation
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPosition = target.position + rotation * offset;

        // Smooth follow
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothTime);
        transform.rotation = rotation;
    }
}