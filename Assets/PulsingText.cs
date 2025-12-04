using UnityEngine;
using TMPro;

public class PulsingText : MonoBehaviour
{
    public TextMeshProUGUI tmp;

    [Header("Pulse Settings")]
    public float pulseSpeed = 2f;
    public float scaleAmount = 0.08f;     // how much the text grows/shrinks
    public float alphaAmount = 0.4f;      // how transparent it gets at minimum

    Vector3 originalScale;
    Color originalColor;

    void Start()
    {
        if (tmp == null)
            tmp = GetComponent<TextMeshProUGUI>();

        originalScale = transform.localScale;
        originalColor = tmp.color;
    }

    void Update()
    {
        float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f;

        // Scale pulsing
        transform.localScale = originalScale * (1f + pulse * scaleAmount);

        // Alpha pulsing (fade in/out)
        Color c = originalColor;
        c.a = Mathf.Lerp(1f - alphaAmount, 1f, pulse);
        tmp.color = c;
    }
}

