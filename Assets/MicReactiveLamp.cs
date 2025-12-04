using UnityEngine;

public enum LampMode
{
    Idle,
    AudioReactive,
    Quiet,
    Danger
}

public class MicReactiveLamp : MonoBehaviour
{
    // ★ NEW: Singleton access so GameManager can call this script
    public static MicReactiveLamp Instance;

    void Awake()
    {
        Instance = this;
    }

    // ★ NEW: whether the lamp reacts to mic or not
    public bool allowAudio = true;

    public LampMode mode = LampMode.Idle;
    public Light lamp;

    [Header("Quiet Mode")]
    public float quietIntensity = 1f;
    public float quietFlickerSpeed = 3f;
    public float quietFlickerStrength = 0.3f;
    public float quietOffChance = 0.02f;

    [Header("Loud Mode")]
    public float loudIntensity = 15f;
    public float strobeChance = 0.4f;
    public float loudFlickerSpeed = 30f;
    public float loudFlickerStrength = 2f;

    [Header("Threshold")]
    public float loudnessThreshold = 1f;

    float seed;

    void Start()
    {
        if (lamp == null) lamp = GetComponent<Light>();
        seed = Random.Range(0f, 9999f);
    }

    void Update()
    {
        // ★ NEW: if audio is disabled (Idle, Puzzle, Ending), stay dim/off
        if (!allowAudio)
        {
            lamp.intensity = Mathf.Lerp(lamp.intensity, 0f, 3f * Time.deltaTime);
            return;
        }

        // ORIGINAL AUDIO REACTIVE BEHAVIOR — unchanged
        float loud = MicInput.loudness;

        if (loud > loudnessThreshold)
            DoLoudStrobe();
        else
            DoQuietFlicker();
    }

    void DoQuietFlicker()
    {
        float noise = Mathf.PerlinNoise(seed, Time.time * quietFlickerSpeed);
        float targetIntensity = quietIntensity + noise * quietFlickerStrength;

        if (Random.value < quietOffChance)
            targetIntensity = 0f;

        lamp.intensity = Mathf.Lerp(lamp.intensity, targetIntensity, 5f * Time.deltaTime);
    }

    void DoLoudStrobe()
    {
        bool strobeOff = Random.value < strobeChance;

        if (strobeOff)
        {
            lamp.intensity = Mathf.Lerp(lamp.intensity, 0f, 20f * Time.deltaTime);
        }
        else
        {
            float noise = Mathf.PerlinNoise(seed, Time.time * loudFlickerSpeed);
            float targetIntensity = loudIntensity + noise * loudFlickerStrength;
            lamp.intensity = Mathf.Lerp(lamp.intensity, targetIntensity, 25f * Time.deltaTime);
        }
    }

    // ----------------------- ★ PHASE CONTROL FUNCTIONS -----------------------

    public void SetIdle()
    {
        allowAudio = false;
        mode = LampMode.Idle;
    }

    public void EnableAudioMode()
    {
        allowAudio = true;
        mode = LampMode.AudioReactive;
    }

    public void SetQuietWarning()
    {
        allowAudio = false;  // quiet mode does NOT react to audio
        mode = LampMode.Quiet;
        lamp.intensity = quietIntensity;
    }

    public void SetDangerMode()
    {
        allowAudio = false; // Danger is handled externally by GameManager
        mode = LampMode.Danger;
        lamp.intensity = loudIntensity;
    }

    public void Disable()
    {
        allowAudio = false;
        lamp.intensity = 0f;
        mode = LampMode.Idle;
    }
}
