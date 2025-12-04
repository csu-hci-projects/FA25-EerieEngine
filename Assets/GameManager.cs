using UnityEngine;
using System.Collections;

public enum GamePhase
{
    Intro,
    QuietChallenge,
    NoiseReaction
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GamePhase currentPhase = GamePhase.Intro;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartCoroutine(IntroSequence());
    }

    IEnumerator IntroSequence()
    {
        // Wait for intro text to finish fading
        IntroTextFader fader = FindObjectOfType<IntroTextFader>();

        if (fader != null)
        {
            yield return fader.PlayAndWait();
        }

        // Move into quiet challenge
        SetPhase(GamePhase.QuietChallenge);
    }

    public void SetPhase(GamePhase newPhase)
    {
        currentPhase = newPhase;
        Debug.Log("PHASE → " + newPhase);

        switch (newPhase)
        {
            case GamePhase.QuietChallenge:
                EnableQuietMode();
                break;

            case GamePhase.NoiseReaction:
                EnableNoiseReaction();
                break;
        }
    }

    void EnableQuietMode()
    {
        Debug.Log("Quiet Mode Enabled");
    }

    void EnableNoiseReaction()
    {
        Debug.Log("Noise Reaction Enabled");
    }
}


