using UnityEngine;
using UnityEngine.InputSystem;

public class FreeLook : MonoBehaviour
{
    public float lookSensitivity = 0.1f;
    private float xRotation = 0f;
    private float yRotation = 0f;

    public bool allowMouseLook = true;

    private Camera cam;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (!allowMouseLook || Mouse.current == null)
            return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        yRotation += mouseDelta.x * lookSensitivity;
        xRotation -= mouseDelta.y * lookSensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

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
