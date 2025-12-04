using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MicReactiveVision : MonoBehaviour
{
    public float loudnessThreshold = 1f;

    [Header("Quiet Mode")]
    public float quietDistortion = -.9f;
    public float quietChroma = 0.8f;
    public float quietLerpSpeed = 5f;

    [Header("Loud Mode")]
    public float loudDistortion = -0.45f; 
    public float loudChroma = 1f;
    public float loudLerpSpeed = 8f;

    private LensDistortion lens;
    private ChromaticAberration chroma;

    void Start()
    {
        Volume v = GetComponent<Volume>();

        // These MUST already exist in your Global Volume profile!
        v.profile.TryGet(out lens);
        v.profile.TryGet(out chroma);
    }

    void Update()
    {
        float loud = MicInput.loudness;

        if (loud > loudnessThreshold)
        {
            // Loud (distorted)
            lens.intensity.Override(Mathf.Lerp(lens.intensity.value, loudDistortion, Time.deltaTime * loudLerpSpeed));
            chroma.intensity.Override(Mathf.Lerp(chroma.intensity.value, loudChroma, Time.deltaTime * loudLerpSpeed));
        }
        else
        {
            // Quiet (normal)
            lens.intensity.Override(Mathf.Lerp(lens.intensity.value, quietDistortion, Time.deltaTime * quietLerpSpeed));
            chroma.intensity.Override(Mathf.Lerp(chroma.intensity.value, quietChroma, Time.deltaTime * quietLerpSpeed));
        }
    }
}


