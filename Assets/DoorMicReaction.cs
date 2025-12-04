using UnityEngine;

public enum DoorMode
{
    Idle,
    AudioReactive,
    QuietThreat,
    Aggressive
}

public class DoorMicReaction : MonoBehaviour
{
    public static DoorMicReaction Instance;

    void Awake()
    {
        Instance = this;
    }

    public DoorMode mode = DoorMode.AudioReactive;

    // ★ NEW: whether the door should respond to mic
    public bool allowAudio = true;

    public float shakeThreshold = 1.0f;
    public float cooldown = 0.5f;

    private Animator anim;
    private float nextShake = 0f;

    void Start()
    {
        anim = GetComponent<Animator>();
        if (anim == null)
            Debug.LogError("DoorMicReaction: NO ANIMATOR ON THIS OBJECT!");
        else
            Debug.Log("DoorMicReaction: Animator FOUND = " + anim.runtimeAnimatorController.name);
    }

    void Update()
    {
        // ★ NEW — If audio disabled, never shake
        if (!allowAudio)
            return;

        // ORIGINAL WORKING MIC SHAKE BEHAVIOR
        float loud = MicInput.loudness;

        if (loud > shakeThreshold && Time.time > nextShake)
        {
            Debug.Log("Triggering DoorShake (LOUDNESS = " + loud + ")");
            anim.SetTrigger("DoorShake");
            nextShake = Time.time + cooldown;
        }
    }

    // ---------------- PHASE CONTROL METHODS ----------------

    public void SetIdle()
    {
        allowAudio = false;
        mode = DoorMode.Idle;
    }

    public void EnableMicShake()
    {
        allowAudio = true;
        mode = DoorMode.AudioReactive;
    }

    public void SetQuietThreat()
    {
        allowAudio = false;  // Quiet mode: door does NOT shake but might animate later
        mode = DoorMode.QuietThreat;
    }

    public void SetAggressiveShake()
    {
        allowAudio = false;  // GameManager controls aggression directly
        mode = DoorMode.Aggressive;
        anim.SetTrigger("DoorShake"); // immediate violent shake
        nextShake = Time.time + 0.1f; // shorten cooldown for effect
    }

    public void Disable()
    {
        allowAudio = false;
        mode = DoorMode.Idle;
    }

    public void PlayOpen()
    {
        allowAudio = false;
        anim.SetTrigger("Open");
    }
}

