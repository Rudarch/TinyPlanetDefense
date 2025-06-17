using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ColorLerpCoroutine : MonoBehaviour
{
    private Color colorFrom = Color.white;
    private Color colorTo = Color.white;
    public float duration = 2f;
    [Range(0, 1)] public float alphaFrom = 1f;
    [Range(0, 1)] public float alphaTo = 1f;
    public Image image;

    private Coroutine colorRoutine;

    public void StartColorTransition()
    {
        this.colorFrom = image.color;
        this.colorFrom.a = alphaFrom;
        this.colorTo = image.color;
        this.colorTo.a = alphaTo;
        if (colorRoutine == null)
            colorRoutine = StartCoroutine(LerpColorLoop());
    }

    public void StopColorTransition()
    {
        if (colorRoutine != null)
        {
            StopCoroutine(colorRoutine);
            colorRoutine = null;
        }
    }

    IEnumerator LerpColorLoop()
    {
        while (true)
        {
            yield return StartCoroutine(LerpColor(colorFrom, colorTo));
            yield return StartCoroutine(LerpColor(colorTo, colorFrom));
        }
    }

    IEnumerator LerpColor(Color from, Color to)
    {
        float timer = 0f;
        while (timer < duration && colorRoutine != null)
        {
            float t = timer / duration;
            image.color = Color.Lerp(from, to, t);
            timer += Time.deltaTime;
            yield return null;
        }
        //image.color = from;
    }
}
