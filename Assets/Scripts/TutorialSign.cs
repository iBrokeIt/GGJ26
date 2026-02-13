using UnityEngine;
using TMPro;
using System.Collections;

public class TutorialSign : MonoBehaviour
{
    [Header("Configuration")]
    public TextMeshPro signText;
    public float fadeDuration = 0.3f;

    private Coroutine fadeCoroutine;

    void Start()
    {
        if (signText != null)
        {
            Color c = signText.color;
            c.a = 0f;
            signText.color = c;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && signText != null)
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeText(1f));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && signText != null)
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeText(0f));
        }
    }

    private IEnumerator FadeText(float targetAlpha)
    {
        Color c = signText.color;
        float startAlpha = c.a;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            signText.color = c;
            yield return null;
        }

        c.a = targetAlpha;
        signText.color = c;
    }
}
