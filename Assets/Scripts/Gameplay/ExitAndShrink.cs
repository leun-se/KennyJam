using UnityEngine;

public class ExitAndShrink : MonoBehaviour
{
    public float exitMoveSpeed = 2f;
    public float shrinkDuration = 1f;

    private bool isExiting = false;
    private float timer = 0f;
    private Vector3 originalScale;
    private Vector3 exitDirection;

    void Awake()
    {
        originalScale = transform.localScale;
    }

    public void StartExit(Vector3 direction)
    {
        if (isExiting) return;
        isExiting = true;
        timer = 0f;
        exitDirection = direction.normalized;
    }

    void Update()
    {
        if (!isExiting) return;

        timer += Time.deltaTime;
        float t = timer / shrinkDuration;
        transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);

        if (timer >= shrinkDuration)
        {
            gameObject.SetActive(false);
        }
    }
}
