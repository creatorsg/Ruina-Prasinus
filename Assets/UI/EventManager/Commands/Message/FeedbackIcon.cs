using System.Collections;
using UnityEngine;

public class FeedbackIcon : UITransitionBase
{
    [Header("FeddbackIcon")]
    [SerializeField] private UITransitionBase _gear;

    private IEnumerator _appearTransitionCoroutine;

    public void AppearTransition()
    {
        if (_appearTransitionCoroutine != null) { StopCoroutine(_appearTransitionCoroutine); }
        _appearTransitionCoroutine = AppearTransitionCoroutine();
        StartCoroutine(_appearTransitionCoroutine);
    }

    private IEnumerator AppearTransitionCoroutine()
    {
        Scale(TransitionType.Lerp, 0.1f, Vector3.zero, new Vector3(1f, 1f, 1f));
        SetAlpha(TransitionType.Lerp, 0.1f, 0f, 1f);
        _gear.Rotate(TransitionType.Instant, 0f, Vector3.zero, Vector3.zero);
        yield return new WaitForSeconds(0.1f);

        Scale(TransitionType.Lerp, 0.05f, new Vector3(1f, 1f, 1f), new Vector3(0.75f, 0.75f, 0.75f));
        yield return new WaitForSeconds(0.05f);

        while (true) //StopTransition()을 호출해야지만 멈춤.
        {
            _gear.Rotate(TransitionType.Smooth, 2f, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 360f));
            yield return new WaitForSeconds(2f);
        }
    }

    public void StopTransition()
    {
        if (_appearTransitionCoroutine != null) { StopCoroutine(_appearTransitionCoroutine); }
        SetAlpha(TransitionType.Instant, 0.0f, 0f, 0f);
    }
}
