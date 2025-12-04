using UnityEngine;
using System.Collections;

public class MicInput : MonoBehaviour
{
    public static float loudness = 0f;
    AudioClip clip;
    string device;

    void Start()
    {
        StartCoroutine(FindBestMic());
    }

    IEnumerator FindBestMic()
    {
        device = Microphone.devices[0];
        clip = Microphone.Start(device, true, 10, 44100);
        yield return null;
    }

    void Update()
    {
        if (clip == null) return;
        loudness = GetAverageVolume() * 10f;
    }

    float GetAverageVolume()
    {
        float[] data = new float[128];
        int micPos = Microphone.GetPosition(device) - 128;
        if (micPos < 0) return 0;

        clip.GetData(data, micPos);
        float sum = 0;
        for (int i = 0; i < data.Length; i++)
            sum += Mathf.Abs(data[i]);

        return sum / 128f;
    }
}



