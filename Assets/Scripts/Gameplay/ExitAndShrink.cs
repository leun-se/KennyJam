using UnityEngine;

public class ExitAndShrink : MonoBehaviour
{
    public float shrinkDuration = 1f;
    public float exitMoveSpeed = 2f;

    private bool isExiting = false;
    private float timer = 0f;
    private Vector3 originalScale;
    private Vector3 exitDirection;

    private Rigidbody rb;

    void Awake()
    {
        originalScale = transform.localScale;
        rb = GetComponent<Rigidbody>();
    }

    public void StartExit(Vector3 direction)
    {
        if (isExiting) return;
        isExiting = true;
        timer = 0f;
        exitDirection = direction;
        exitDirection.y = 0f;
        exitDirection.Normalize();

        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    void FixedUpdate()
    {
        if (!isExiting) return;

        Vector3 targetPos = rb.position + exitDirection * exitMoveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPos);

        timer += Time.fixedDeltaTime;
        float t = timer / shrinkDuration;
        transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);

        if (timer >= shrinkDuration)
        {
            gameObject.SetActive(false);
        }
    }
}
