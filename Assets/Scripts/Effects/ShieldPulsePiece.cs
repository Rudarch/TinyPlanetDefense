using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class ShieldPulsePiece : MonoBehaviour
{
    public float fadeInSpeed = 2f;
    public float fadeOutSpeed = 2f;
    public float minAlpha = 0.3f;
    public float maxAlpha = 1f;
    public float holdDuration = 0.2f;

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(PulseRoutine());
    }

    IEnumerator PulseRoutine()
    {
        while (true)
        {
            // Fade in
            yield return StartCoroutine(FadeTo(maxAlpha, fadeInSpeed));
            yield return new WaitForSeconds(holdDuration);

            // Fade out
            yield return StartCoroutine(FadeTo(minAlpha, fadeOutSpeed));
            yield return new WaitForSeconds(holdDuration);
        }
    }

    IEnumerator FadeTo(float targetAlpha, float fadeSpeed)
    {
        Color c = sr.color;

        while (!Mathf.Approximately(c.a, targetAlpha))
        {
            c.a = Mathf.MoveTowards(c.a, targetAlpha, Time.deltaTime * fadeSpeed);
            sr.color = c;
            yield return null;
        }
    }
}