using UnityEngine;
using System.Collections;

public class GameAmbienceController : MonoBehaviour
{
    public static GameAmbienceController Instance;

    public AudioSource ambientSource;

    [Header("Base Volume")]
    public float baseVolume = 0.4f;

    [Header("Phase 1 Behavior")]
    public float silenceReturnDelay = 1.0f; // how long ambience stays off after ANY noise
    public float fadeInSpeed = 1.5f;        // how fast ambience comes back

    private float lastNoiseTime = -999f;

    void Awake()
    {
        Instance = this;

        if (ambientSource == null)
            ambientSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        ambientSource.volume = baseVolume;
        ambientSource.loop = true;
        ambientSource.Play();
    }

    void Update()
    {
        if (GameManager.Instance.currentPhase == GamePhase.QuietChallenge)
        {
            HandlePhase1Audio();
        }
    }

    void HandlePhase1Audio()
    {
        float loud = MicInput.loudness;

        // ANY noise kills ambience instantly
        if (loud > 0.02f) // extremely sensitive threshold
        {
            ambientSource.volume = 0f;
            lastNoiseTime = Time.time;
        }

        // After delay, fade ambience back in
        if (Time.time - lastNoiseTime > silenceReturnDelay)
        {
            ambientSource.volume = Mathf.Lerp(
                ambientSource.volume,
                baseVolume,
                Time.deltaTime * fadeInSpeed
            );
        }
    }

    public void StopAmbience()
    {
        ambientSource.Stop();
    }
}


