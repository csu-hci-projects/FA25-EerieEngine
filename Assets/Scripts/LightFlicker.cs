using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light flickerLight;
    public float minIntensity = 10.0f;
    public float maxIntensity = 15.0f;
    public float flickerSpeed = 0.1f;

    void Start()
    {
        if (flickerLight == null)
            flickerLight = GetComponent<Light>();
    }

    void Update()
    {
        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0.0f);
        flickerLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
    }
}