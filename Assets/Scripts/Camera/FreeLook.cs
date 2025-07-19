using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class FreeLook : MonoBehaviour
{
    public float lookSensitivity = 0.1f;
    private float xRotation;
    private float yRotation;
    public bool allowMouseLook = true;
    public float minYaw = -35f;
    public float maxYaw = 25f;
    public float minPitch = -10f;
    public float maxPitch = 20f;
    private float initialYaw;
    private float initialPitch;
    private Camera cam;
    private bool skipFirstMouseMovement = true;

    void Start()
    {
        cam = GetComponent<Camera>();

        xRotation = 35f;
        yRotation = -40f;

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        initialYaw = yRotation;
        initialPitch = xRotation;

        StartCoroutine(DelayedLockCursor());
    }

    void Update()
    {
        if (!allowMouseLook || Mouse.current == null)
            return;

        if (skipFirstMouseMovement)
        {
            skipFirstMouseMovement = false;
            Mouse.current.WarpCursorPosition(new Vector2(Screen.width / 2f, Screen.height / 2f));
            return;
        }

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        yRotation += mouseDelta.x * lookSensitivity;
        xRotation -= mouseDelta.y * lookSensitivity;

        yRotation = Mathf.Clamp(yRotation, initialYaw + minYaw, initialYaw + maxYaw);
        xRotation = Mathf.Clamp(xRotation, initialPitch + minPitch, initialPitch + maxPitch);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    public void SetZoom(float fov)
    {
        if (cam == null)
            cam = GetComponent<Camera>();

        if (cam != null)
            cam.fieldOfView = fov;
    }

    private IEnumerator DelayedLockCursor()
    {
        yield return null;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        skipFirstMouseMovement = true;
    }
}