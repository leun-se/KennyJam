using UnityEngine;

public class GameManager : MonoBehaviour
{
    LandInputSubscription GetInput;

    private void Awake()
    {
        GetInput = GetComponent<LandInputSubscription>();
    }
}
