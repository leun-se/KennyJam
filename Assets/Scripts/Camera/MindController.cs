using UnityEngine;
using UnityEngine.InputSystem;

public class MindController : MonoBehaviour
{
    public GameObject possessionArrowPrefab;
    public GameObject crosshairUI;
    private GameObject currentControlledCharacter;
    private GameObject currentArrow;

    private float targetOrthoSize = 5f;
    private float zoomSpeed = 5f;

    public GameObject CurrentControlledCharacter => currentControlledCharacter;

    private FreeLook freeLook;

    void Start()
    {
        freeLook = Camera.main.GetComponent<FreeLook>();
    }

    private void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            ReturnControlToMind();
        }

        if (currentArrow != null && Camera.main != null)
        {
            Vector3 dir = Camera.main.transform.position - currentArrow.transform.position;
            dir.y = 0;
            currentArrow.transform.rotation = Quaternion.LookRotation(-dir);
        }

        if (Camera.main != null && Camera.main.orthographic)
        {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetOrthoSize, Time.deltaTime * zoomSpeed);
        }
    }

    public bool IsPossessed(GameObject obj)
    {
        return obj == currentControlledCharacter;
    }

    public void SetPossessedHighlight(GameObject obj, bool possessed)
    {
        if (obj == null) return;

        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in renderers)
        {
            rend.material.color = possessed ? Color.yellow : Color.white;
        }
    }

    public void Possess(GameObject newCharacter)
    {
        if (currentControlledCharacter != null || newCharacter == null)
            return;

        if (currentControlledCharacter != null)
        {
            SetPossessedHighlight(currentControlledCharacter, false);

            var oldController = currentControlledCharacter.GetComponent<PlayerController>();
            if (oldController != null)
                oldController.enabled = false;

            if (currentArrow != null)
                Destroy(currentArrow);
        }

        currentControlledCharacter = newCharacter;

        SetPossessedHighlight(currentControlledCharacter, true);

        var newController = currentControlledCharacter.GetComponent<PlayerController>();
        if (newController != null)
            newController.enabled = true;

        if (possessionArrowPrefab != null)
        {
            currentArrow = Instantiate(possessionArrowPrefab, currentControlledCharacter.transform);
            currentArrow.transform.localPosition = new Vector3(0, 1.8f, 0);
            currentArrow.transform.localRotation = Quaternion.identity;
        }

        Renderer rend = newCharacter.GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            float size = rend.bounds.size.magnitude;
            targetOrthoSize = Mathf.Clamp(size, 2.5f, 5f);
        }
        else
        {
            targetOrthoSize = 3f;
        }

        if (crosshairUI != null)
            crosshairUI.SetActive(false);

        CameraFollow camFollow = Camera.main.GetComponent<CameraFollow>();
        if (camFollow != null)
        {
            camFollow.SetTarget(currentControlledCharacter.transform);
        }

        if (freeLook != null)
            freeLook.allowMouseLook = false;
    }

    public void ReturnControlToMind()
    {
        if (currentControlledCharacter != null)
        {
            SetPossessedHighlight(currentControlledCharacter, false);

            var controller = currentControlledCharacter.GetComponent<PlayerController>();
            if (controller != null)
                controller.enabled = false;

            currentControlledCharacter = null;
        }

        if (currentArrow != null)
        {
            Destroy(currentArrow);
            currentArrow = null;
        }

        targetOrthoSize = 5f;

        if (crosshairUI != null)
            crosshairUI.SetActive(true);

        CameraFollow camFollow = Camera.main.GetComponent<CameraFollow>();
        if (camFollow != null)
        {
            camFollow.SetTarget(null);
        }

        if (freeLook != null)
            freeLook.allowMouseLook = true;
    }
}