using UnityEngine;
using TMPro;
using System.Collections;   // ← THIS FIXES THE ERROR

public class TitleFade : MonoBehaviour
{
    public TextMeshProUGUI title;
    public float fadeDuration = 3f;
    private CanvasGroup cg;

    void Awake()
    {
        cg = title.gameObject.AddComponent<CanvasGroup>();
        cg.alpha = 0f;
    }

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            cg.alpha = t / fadeDuration;
            yield return null;
        }

        cg.alpha = 1;
    }
}


