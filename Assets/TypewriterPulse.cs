using UnityEngine;
using TMPro;
using System.Collections;

public class TypewriterPulse : MonoBehaviour
{
    public TextMeshProUGUI tmp;

    [Header("Typing")]
    public float typeSpeed = 0.04f;
    public float startDelay = 0f;

    [Header("Brightness Pulse")]
    public float pulseSpeed = 2f;
    public float minBrightness = 0.6f;
    public float maxBrightness = 1.3f;

    string fullText;
    Color originalColor;

    void Awake()
    {
        if (tmp == null)
            tmp = GetComponent<TextMeshProUGUI>();

        originalColor = tmp.color;
    }

    void OnEnable()
    {
        fullText = tmp.text;
        tmp.text = "";
        StartCoroutine(TypeRoutine());
    }

    IEnumerator TypeRoutine()
    {
        yield return new WaitForSeconds(startDelay);

        foreach (char c in fullText)
        {
            tmp.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    void Update()
    {
        // Brightness pulse
        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f; // 0–1
        float brightness = Mathf.Lerp(minBrightness, maxBrightness, t);

        Color c = originalColor * brightness;
        c.a = originalColor.a; // preserve alpha
        tmp.color = c;
    }
}

