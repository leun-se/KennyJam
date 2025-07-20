using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("MonitorScene");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit(); 
    }
}