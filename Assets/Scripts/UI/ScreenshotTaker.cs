using UnityEngine;
using UnityEngine.InputSystem;

public class ScreenshotTaker : MonoBehaviour
{
    [Tooltip("Assign a key like 'k' in the inspector or via scriptable input actions.")]
    public InputAction screenshotAction;

    private void OnEnable()
    {
        screenshotAction.Enable();
    }

    private void OnDisable()
    {
        screenshotAction.Disable();
    }

    private void Update()
    {
        if (screenshotAction.triggered)
        {
            string path = string.Format("screenshot_{0:yyyyMMdd_HHmmss}.png", System.DateTime.Now);
            ScreenCapture.CaptureScreenshot(path);
            Debug.Log("Screenshot saved to: " + path);
        }
    }
}
