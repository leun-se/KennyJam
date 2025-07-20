using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Level Complete")]
    [SerializeField] private AudioClip LevelCompleteSoundClip;
    public static GameManager Instance;

    public int totalCharacters = 3;
    private int escapedCharacters = 0;
    public TextMeshProUGUI escapeText;

    [Header("Pause Menu")]
    public GameObject pauseMenu;
    private bool isPaused = false;

    private PlayerControls controls;

    public FreeLook freeLookCamera;

    public GameObject crosshair;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.Player.Pause.performed += ctx => TogglePause();
    }

    private void OnDisable()
    {
        controls.Player.Pause.performed -= ctx => TogglePause();
        controls.Disable();
    }

    private void Start()
    {
        UpdateEscapeText();
        if (pauseMenu != null) pauseMenu.SetActive(false);

        if (freeLookCamera == null)
        {
            freeLookCamera = Camera.main.GetComponent<FreeLook>();
        }
    }

    public void RegisterEscape()
    {
        escapedCharacters++;
        UpdateEscapeText();

        if (escapedCharacters >= totalCharacters)
        {
            LevelComplete();
        }
    }

    private void UpdateEscapeText()
    {
        if (escapeText != null)
        {
            escapeText.text = $"Escaped: {escapedCharacters} / {totalCharacters}";
        }
    }

    private void LevelComplete()
    {
        SoundEffectsManager.instance.PlaySoundFXClip(LevelCompleteSoundClip, transform, 1f);
        Debug.Log("Level Complete!");

        string currentScene = SceneManager.GetActiveScene().name;
        LevelProgress.MarkCompleted(currentScene);

        StartCoroutine(ReturnToMonitorSceneAfterDelay(2f));
    }

    private IEnumerator ReturnToMonitorSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("MonitorScene");
    }

    private void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);

        Time.timeScale = isPaused ? 0f : 1f;

        if (freeLookCamera != null)
        {
            freeLookCamera.allowMouseLook = !isPaused;
        }

        if (crosshair != null)
        {
            crosshair.SetActive(!isPaused);
        }

        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}