using UnityEngine;
using UnityEngine.InputSystem;

public class FixedPositionMouseLook : MonoBehaviour
{
    public float lookSensitivity = 0.1f;

    private float xRotation = 0f; // vertical pitch
    private float yRotation = 0f; // horizontal yaw

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Mouse.current == null) return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        yRotation += mouseDelta.x * lookSensitivity;
        xRotation -= mouseDelta.y * lookSensitivity;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
