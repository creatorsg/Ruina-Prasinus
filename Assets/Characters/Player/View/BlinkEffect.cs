using UnityEngine;
using System.Collections;

public class BlinkEffect : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    private Coroutine blinkCoroutine;

    public void StartBlink()
    {
        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
        blinkCoroutine = StartCoroutine(BlinkEffect2());
    }
    public void StopBlink()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }

        if (sr != null)
        {
            Color c = sr.color;
            c.a = 1f;
            sr.color = c;
        }
    }
    private IEnumerator BlinkEffect2()
    {
        Color color = sr.color;

        while (true)
        {
            color.a = 0.3f;
            sr.color = color;
            yield return new WaitForSeconds(0.2f);

            color.a = 1f;
            sr.color = color;
            yield return new WaitForSeconds(0.2f);
        }
    }
}