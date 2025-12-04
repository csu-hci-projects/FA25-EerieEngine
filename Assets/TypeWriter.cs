using UnityEngine;
using TMPro;
using System.Collections;

public class TypeWriter : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    public float typeSpeed = 0.04f;  // seconds per character
    public float startDelay = 0f;    // delay before typing starts

    string fullText;

    void Awake()
    {
        if (tmp == null)
            tmp = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        fullText = tmp.text;
        tmp.text = "";
        StartCoroutine(TypeRoutine());
    }

    IEnumerator TypeRoutine()
    {
        yield return new WaitForSeconds(startDelay);

        foreach (char c in fullText)
        {
            tmp.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
    }
}


