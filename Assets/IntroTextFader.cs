using UnityEngine;
using TMPro;
using System.Collections;

public class IntroTextFader : MonoBehaviour
{
    public TextMeshProUGUI textObject;

    [Header("Timings")]
    public float delayBeforeStart = 1.5f;   // NEW — delay before text appears
    public float fadeInDuration = 2f;
    public float holdDuration = 4f;
    public float fadeOutDuration = 2f;

    private CanvasGroup cg;

    void Awake()
    {
        if (textObject == null)
            textObject = GetComponent<TextMeshProUGUI>();

        cg = GetComponent<CanvasGroup>();
        if (cg == null)
            cg = gameObject.AddComponent<CanvasGroup>();

        cg.alpha = 0f;
    }

    public IEnumerator PlayAndWait()
    {
        // 🎬 NEW: Delay before fade in starts
        yield return new WaitForSeconds(delayBeforeStart);

        yield return StartCoroutine(FadeIn());
        yield return new WaitForSeconds(holdDuration);
        yield return StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
    {
        float t = 0f;

        while (t < fadeInDuration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(0f, 1f, t / fadeInDuration);
            yield return null;
        }

        cg.alpha = 1f;
    }

    IEnumerator FadeOut()
    {
        float t = 0f;

        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(1f, 0f, t / fadeOutDuration);
            yield return null;
        }

        cg.alpha = 0f;
    }
}