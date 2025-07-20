using UnityEngine;
using UnityEngine.InputSystem;

public class MindController : MonoBehaviour
{
    [SerializeField] private AudioClip mindControlSoundClip;
    public GameObject possessionArrowPrefab;
    public GameObject crosshairUI;
    public GameObject uiHint; // Assign this in Inspector (make sure it has CanvasGroup + UIHintFade)

    private GameObject currentControlledCharacter;
    private GameObject currentArrow;

    private float targetOrthoSize = 5f;
    private float zoomSpeed = 5f;

    public GameObject CurrentControlledCharacter => currentControlledCharacter;

    private FreeLook freeLook;
    private UIHintFade uiHintFade;

    private Vector3 lastPosition;
    private float stationaryTimer = 0f;
    private float timeToShowHint = 1.5f;
    private bool isHintVisible = false;

    void Start()
    {
        freeLook = Camera.main.GetComponent<FreeLook>();
        if (uiHint != null)
            uiHintFade = uiHint.GetComponent<UIHintFade>();
    }

    private void Update()
    {
        // Return control to mind with R key
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            ReturnControlToMind();
        }

        // Rotate possession arrow toward camera
        if (currentArrow != null && Camera.main != null)
        {
            Vector3 dir = Camera.main.transform.position - currentArrow.transform.position;
            dir.y = 0;
            currentArrow.transform.rotation = Quaternion.LookRotation(-dir);
        }

        // Zoom camera orthographic size smoothly
        if (Camera.main != null && Camera.main.orthographic)
        {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetOrthoSize, Time.deltaTime * zoomSpeed);
        }

        // Show/hide hint UI based on whether player is moving or standing still
        if (currentControlledCharacter != null)
        {
            Vector3 currentPos = currentControlledCharacter.transform.position;
            bool isMoving = Vector3.Distance(currentPos, lastPosition) > 0.01f;
            lastPosition = currentPos;

            if (isMoving)
            {
                stationaryTimer = 0f;

                if (isHintVisible && uiHintFade != null)
                {
                    uiHintFade.FadeOut();
                    isHintVisible = false;
                }
            }
            else
            {
                stationaryTimer += Time.deltaTime;

                if (!isHintVisible && stationaryTimer >= timeToShowHint && uiHintFade != null)
                {
                    uiHintFade.FadeIn();
                    isHintVisible = true;
                }
            }
        }
    }

    public void LockInput()
    {
        if (CurrentControlledCharacter != null)
        {
            var playerMovement = CurrentControlledCharacter.GetComponent<PlayerController>();
            if (playerMovement != null)
            {
                playerMovement.enabled = false;
            }
        }
    }

    public bool IsPossessed(GameObject obj)
    {
        return obj == currentControlledCharacter;
    }

    public void Possess(GameObject newCharacter)
    {
        if (currentControlledCharacter != null || newCharacter == null)
            return;

        if (currentControlledCharacter != null)
        {
            var oldController = currentControlledCharacter.GetComponent<PlayerController>();
            if (oldController != null)
                oldController.enabled = false;

            if (currentArrow != null)
                Destroy(currentArrow);
        }

        SoundEffectsManager.instance.PlaySoundFXClip(mindControlSoundClip, transform, 1f);

        currentControlledCharacter = newCharacter;

        var newController = currentControlledCharacter.GetComponent<PlayerController>();
        if (newController != null)
            newController.enabled = true;

        if (possessionArrowPrefab != null)
        {
            currentArrow = Instantiate(possessionArrowPrefab, currentControlledCharacter.transform);
            currentArrow.transform.localPosition = new Vector3(0, 1.6f, 0);
            currentArrow.transform.localRotation = Quaternion.identity;
        }

        Renderer rend = newCharacter.GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            float size = rend.bounds.size.magnitude;
            targetOrthoSize = Mathf.Clamp(size, 3f, 5f);
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

        // Show hint immediately when possessing
        if (uiHintFade != null)
        {
            uiHint.SetActive(true);
            uiHintFade.FadeIn();
            isHintVisible = true;
            stationaryTimer = 0f;
        }

        lastPosition = currentControlledCharacter.transform.position;
    }

    public void ReturnControlToMind()
    {
        if (currentControlledCharacter != null)
        {
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

        if (uiHint != null)
        {
            if (uiHintFade != null)
            {
                uiHintFade.FadeOut();
            }
            else
            {
                uiHint.SetActive(false);
            }
            isHintVisible = false;
        }
    }
}
