using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Escape Tracking")]
    public int totalCharacters = 3;
    private int escapedCharacters = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterEscape()
    {
        escapedCharacters++;

        Debug.Log($"Escaped: {escapedCharacters}/{totalCharacters}");

        if (escapedCharacters >= totalCharacters)
        {
            Debug.Log("All characters escaped!");
            CompleteLevel();
        }
    }

    private void CompleteLevel()
    {
        Debug.Log("Level Complete!");

        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}