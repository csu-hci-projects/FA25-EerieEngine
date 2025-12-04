using UnityEngine;

public enum RadioMode
{
    Idle,
    AudioReactive,
    Quiet,
    Puzzle,
    Final
}

public class MicReactiveRadioLight : MonoBehaviour
{
    public static MicReactiveRadioLight Instance;

    void Awake()
    {
        Instance = this;
    }

    // ★ NEW: GameManager can disable audio reaction
    public bool allowAudio = true;

    public RadioMode mode = RadioMode.Idle;
    public Light radioLight;

    [Header("Quiet Settings")]
    public float quietIntensity = 0.2f;
    public float quietFlickerSpeed = 0.5f;
    public float quietFlickerStrength = 0.05f;

    [Header("Loud Settings")]
    public float maxIntensity = 6f;
    public float loudFlickerSpeed = 25f;
    public float loudFlickerStrength = 2f;

    [Header("Static Burst / Hard Flicker")]
    public float staticThreshold = 4f;
    public float staticOffChance = 0.4f;

    [Header("Sensitivity")]
    public float loudnessDivide = 5f;

    float seed;

    void Start()
    {
        if (radioLight == null) radioLight = GetComponent<Light>();
        seed = Random.Range(0f, 10000f);
    }

    void Update()
    {
        // ★ NEW: If audio is disabled, fade the light down
        if (!allowAudio)
        {
            radioLight.intensity = Mathf.Lerp(radioLight.intensity, 0f, Time.deltaTime * 3f);
            return;
        }

        // ------- ORIGINAL AUDIO LOGIC (unchanged) -------
        float loud = MicInput.loudness;
        float loudNorm = Mathf.Clamp01(loud / loudnessDivide);

        if (loudNorm < 0.1f)
        {
            // quiet flicker
            float n = Mathf.PerlinNoise(seed, Time.time * quietFlickerSpeed);
            float quiet = quietIntensity + (n - 0.5f) * quietFlickerStrength;

            radioLight.intensity = Mathf.Lerp(radioLight.intensity, quiet, Time.deltaTime * 4f);
            return;
        }

        float flickerSpeed = Mathf.Lerp(quietFlickerSpeed, loudFlickerSpeed, loudNorm);
        float flickNoise = Mathf.PerlinNoise(seed, Time.time * flickerSpeed);

        if (loud > staticThreshold && Random.value < staticOffChance)
        {
            radioLight.intensity = Mathf.Lerp(radioLight.intensity, 0f, Time.deltaTime * 25f);
            return;
        }

        float targetIntensity =
            Mathf.Lerp(quietIntensity, maxIntensity, loudNorm)
            + (flickNoise - 0.5f) * loudFlickerStrength;

        targetIntensity = Mathf.Max(0f, targetIntensity);

        radioLight.intensity = Mathf.Lerp(radioLight.intensity, targetIntensity, Time.deltaTime * 20f);
    }

    // ---------------------- ★ Phase Methods ----------------------

    public void SetIdle()
    {
        allowAudio = false;
        mode = RadioMode.Idle;
    }

    public void EnableAudioMode()
    {
        allowAudio = true;
        mode = RadioMode.AudioReactive;
    }

    public void SetQuietMode()
    {
        allowAudio = false;
        mode = RadioMode.Quiet;
        radioLight.intensity = quietIntensity;
    }

    public void StartPuzzlePattern()
    {
        allowAudio = false;
        mode = RadioMode.Puzzle;
        // You can add a pulse pattern later
    }

    public void StartFinalPattern()
    {
        allowAudio = true;
        mode = RadioMode.Final;
        // Uses your loud flicker logic but stays intense
    }

    public void Disable()
    {
        allowAudio = false;
        mode = RadioMode.Idle;
        radioLight.intensity = 0f;
    }
}

