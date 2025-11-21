using UnityEngine;

public class DramaticFlicker : MonoBehaviour
{
    [SerializeField] Light streetLight;
    [SerializeField] float minIntensity = 0f;
    [SerializeField] float maxIntensity = 8f;
    [SerializeField] float flickerSpeedMin = 0.05f;
    [SerializeField] float flickerSpeedMax = 0.5f;

    void Start()
    {
        if (streetLight == null)
            streetLight = GetComponent<Light>();

        StartCoroutine(FlickerRoutine());
    }

    System.Collections.IEnumerator FlickerRoutine()
    {
        while (true)
        {
            // Random intensity: sometimes off, sometimes blinding
            streetLight.intensity = Random.Range(minIntensity, maxIntensity);

            // Random wait time between flickers
            float waitTime = Random.Range(flickerSpeedMin, flickerSpeedMax);
            yield return new WaitForSeconds(waitTime);
        }
    }
}