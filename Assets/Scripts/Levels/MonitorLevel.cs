using UnityEngine;

public class MonitorLevel : MonoBehaviour
{
    public string sceneName;

    public bool IsCompleted()
    {
        return LevelProgress.IsCompleted(sceneName);
    }
}