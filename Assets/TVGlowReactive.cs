using UnityEngine;

public class TVGlowColor : MonoBehaviour
{
    public Light tvLight;
    public RenderTexture videoTexture;

    [Header("Glow Settings")]
    public float minIntensity = 0.2f;
    public float maxIntensity = 4f;
    public float smoothing = 6f;

    private Texture2D reader;

    void Start()
    {
        if (tvLight == null)
            tvLight = GetComponent<Light>();

        // Make a small, fast texture for sampling
        reader = new Texture2D(32, 32, TextureFormat.RGB24, false);
    }

    void Update()
    {
        if (videoTexture == null) return;

        // Copy video into low-res reader to get the color
        RenderTexture.active = videoTexture;
        reader.ReadPixels(new Rect(0, 0, videoTexture.width, videoTexture.height), 0, 0);
        reader.Apply();
        RenderTexture.active = null;

        // Average color
        Color[] pixels = reader.GetPixels();
        Color avg = Color.black;

        for (int i = 0; i < pixels.Length; i++)
            avg += pixels[i];

        avg /= pixels.Length;

        // Use brightness from color to scale intensity
        float brightness = avg.maxColorComponent;

        float targetIntensity = Mathf.Lerp(minIntensity, maxIntensity, brightness);
        tvLight.intensity = Mathf.Lerp(tvLight.intensity, targetIntensity, smoothing * Time.deltaTime);

        // Light color follows the video color
        tvLight.color = Color.Lerp(tvLight.color, avg, Time.deltaTime * smoothing);
    }
}



