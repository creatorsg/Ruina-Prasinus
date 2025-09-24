using System.Collections;
using UnityEngine;
using TMPro;

public class NameBox : UITransitionBase
{
    [Header("Name Box")]
    [SerializeField] TextMeshProUGUI _textTmp;
    [SerializeField] private UITransitionBase _textTransition;

    private IEnumerator _textBoxCoroutine;

    public void Initialize()
    {
        Position(TransitionType.Instant, 0f, Vector3.zero, new Vector3(-410f, -285.5f, 0f));
        Scale(TransitionType.Instant, 0f, Vector3.zero, new Vector3(1f, 1f, 1f));
        Area(TransitionType.Instant, 0f, Vector2.zero, new Vector2(282f, 96f));
        SetColor(TransitionType.Instant, 0f, Color.white, new Color(1f, 1f, 1f, 1f));
        SetAlpha(TransitionType.Instant, 0f, 0f, 0f);

        SetText("Test Actor");
        _textTransition.SetAlpha(TransitionType.Instant, 0f, 0f, 0f);
    }

    private void SetText(string name) =>
        _textTmp.text = name;

    public void NewTransition(string name)
    {
        Initialize();
        SetAlpha(TransitionType.Instant, 0f, 0f, 1f);
        SetText(name);

        if (_textBoxCoroutine != null) { StopCoroutine(_textBoxCoroutine); }
        _textBoxCoroutine = NewTransitionCoroutine();
        StartCoroutine(_textBoxCoroutine);
    }

    private IEnumerator NewTransitionCoroutine()
    {
        Position(TransitionType.Lerp, 0.1f, new Vector3(-410f, -300f, 0f), new Vector3(-410f, -250f, 0f));
        SetColor(TransitionType.Lerp, 0.1f, new Color(1f, 1f, 1f, 0f), new Color(1f, 1f, 1f, 1f));
        _textTransition.Rotate(TransitionType.Instant, 0f, Vector3.zero, new Vector3(0f, 0f, 0f));
        _textTransition.SetAlpha(TransitionType.Lerp, 0.1f, 0f, 1f);
        yield return new WaitForSeconds(0.1f);

        Position(TransitionType.Lerp, 0.1f, new Vector3(-410f, -250f, 0f), new Vector3(-410f, -260f, 0f));
        yield return new WaitForSeconds(0.05f);
    }

    public void EndTransition()
    {
        SetAlpha(TransitionType.Lerp, 0.15f, 1f, 0f);
    }
}
