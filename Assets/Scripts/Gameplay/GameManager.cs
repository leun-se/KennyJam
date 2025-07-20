using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioClip LevelCompleteSoundClip;
    public static GameManager Instance;

    public int totalCharacters = 3;
    private int escapedCharacters = 0;

    public TextMeshProUGUI escapeText;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        UpdateEscapeText();
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
}