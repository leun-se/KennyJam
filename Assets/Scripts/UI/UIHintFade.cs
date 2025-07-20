using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class UIHintFade : MonoBehaviour
{
    public float fadeDuration = 1.5f;
    private CanvasGroup canvasGroup;
    private Coroutine currentFadeCoroutine;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1f;  // Start fully visible or adjust as needed
    }

    public void FadeIn()
    {
        if (currentFadeCoroutine != null)
            StopCoroutine(currentFadeCoroutine);
        gameObject.SetActive(true);
        currentFadeCoroutine = StartCoroutine(FadeTo(1f));
    }

    public void FadeOut()
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        
        if (currentFadeCoroutine != null)
            StopCoroutine(currentFadeCoroutine);
        
        currentFadeCoroutine = StartCoroutine(FadeTo(0f));
    }

    private IEnumerator FadeTo(float targetAlpha)
    {
        float startAlpha = canvasGroup.alpha;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;

        if (targetAlpha == 0f)
            gameObject.SetActive(false);
    }
}
