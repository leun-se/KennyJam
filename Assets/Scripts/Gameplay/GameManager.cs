using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
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
        Debug.Log("Level Complete!");
    }
}