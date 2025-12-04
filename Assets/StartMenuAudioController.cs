using UnityEngine;
using System.Collections;   // <-- REQUIRED for IEnumerator

[RequireComponent(typeof(AudioSource))]
public class StartMenuAudioController : MonoBehaviour
{
    public float fadeInTime = 3f;
    public float targetVolume = 0.4f;   // how loud menu ambience gets

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0f;
        audioSource.Play();

        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float t = 0f;
        while (t < fadeInTime)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, targetVolume, t / fadeInTime);
            yield return null;
        }

        audioSource.volume = targetVolume;
    }
}


