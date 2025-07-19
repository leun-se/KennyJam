using UnityEngine;
using UnityEngine.InputSystem;

public class FreeLook : MonoBehaviour
{
    public float lookSensitivity = 0.1f;
    private float xRotation = 0f;
    private float yRotation = 0f;

    public bool allowMouseLook = true;

    public float minYaw = -85f;
    public float maxYaw = -25f;

    public float minPitch = 15f;
    public float maxPitch = 50f;

    private float initialPitch;

    private float initialYaw;

    private Camera cam;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cam = GetComponent<Camera>();

        initialYaw = transform.localEulerAngles.y;
        yRotation = initialYaw;

        initialPitch = transform.localEulerAngles.x;
        xRotation = initialPitch;
    }

    void Update()
    {
        if (!allowMouseLook || Mouse.current == null)
            return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        yRotation += mouseDelta.x * lookSensitivity;
        xRotation -= mouseDelta.y * lookSensitivity;

        float yawMin = initialYaw + minYaw;
        float yawMax = initialYaw + maxYaw;
        yRotation = Mathf.Clamp(yRotation, yawMin, yawMax);

        float pitchMin = initialPitch + minPitch;
        float pitchMax = initialPitch + maxPitch;
        xRotation = Mathf.Clamp(xRotation, pitchMin, pitchMax);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    public void SetZoom(float fov)
    {
        if (cam == null)
            cam = GetComponent<Camera>();

        if (cam != null)
            cam.fieldOfView = fov;
    }
}