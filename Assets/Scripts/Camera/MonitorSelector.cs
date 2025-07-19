using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MonitorSelector : MonoBehaviour
{
    [Header("Monitor Selection")]
    public float maxSelectDistance = 10f;

    [Header("Zoom Settings")]
    public Camera mainCamera;
    public float zoomFOV = 30f;
    public float zoomSpeed = 5f;

    private GameObject currentLookedAtMonitor;
    private Color[] originalColors;
    private Renderer[] currentRenderers;
    private float originalFOV;
    private bool isZooming = false;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        originalFOV = mainCamera.fieldOfView;
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        isZooming = false;

        if (Physics.Raycast(ray, out hit, maxSelectDistance))
        {
            if (hit.collider.CompareTag("Monitor"))
            {
                GameObject monitor = hit.collider.gameObject;

                if (currentLookedAtMonitor != monitor)
                {
                    ClearHighlight();

                    currentRenderers = monitor.GetComponentsInChildren<Renderer>();
                    originalColors = new Color[currentRenderers.Length];

                    for (int i = 0; i < currentRenderers.Length; i++)
                    {
                        if (currentRenderers[i].material.HasProperty("_Color"))
                        {
                            originalColors[i] = currentRenderers[i].material.color;
                            currentRenderers[i].material.color = Color.green;
                        }
                    }

                    currentLookedAtMonitor = monitor;
                }

                isZooming = true;

                if (Keyboard.current.eKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
                {
                    MonitorLevel level = monitor.GetComponent<MonitorLevel>();
                    if (level != null)
                    {
                        Debug.Log("Loading scene: " + level.sceneName);
                        SceneManager.LoadScene(level.sceneName);
                    }
                    else
                    {
                        Debug.LogWarning("MonitorLevel component missing on: " + monitor.name);
                    }
                }

                return;
            }
        }

        ClearHighlight();

        float targetFOV = isZooming ? zoomFOV : originalFOV;
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
    }

    void ClearHighlight()
    {
        if (currentLookedAtMonitor != null && currentRenderers != null)
        {
            for (int i = 0; i < currentRenderers.Length; i++)
            {
                if (currentRenderers[i] != null && currentRenderers[i].material.HasProperty("_Color"))
                {
                    currentRenderers[i].material.color = originalColors[i];
                }
            }
        }

        currentLookedAtMonitor = null;
        currentRenderers = null;
        originalColors = null;
    }
}
