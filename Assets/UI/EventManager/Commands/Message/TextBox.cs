using System.Collections;
using UnityEngine;
using TMPro;

public class TextBox : UITransitionBase
{
    [SerializeField] private bool _isPrintAll = false;

    [Header("Text Box")]
    [SerializeField] private TextMeshProUGUI _textTmp;
    [SerializeField] private UITransitionBase _textTransition;
    [SerializeField] private FeedbackIcon _feedBackIcon;

    private IEnumerator _textBoxCoroutine;
    private IEnumerator _printTextCoroutine;

    public void Initialize()
    {
        Position(TransitionType.Instant, 0f, Vector3.zero, new Vector3(-546.5f, -335f, 0f));
        Scale(TransitionType.Instant, 0f, Vector3.zero, new Vector3(1f, 1f, 1f));
        Area(TransitionType.Instant, 0f, Vector2.zero, new Vector2(1153f, 287f));
        SetColor(TransitionType.Instant, 0f, Color.white, new Color(1f, 1f, 1f, 1f));
        SetAlpha(TransitionType.Instant, 0f, 0f, 0f);

        SetText("");
        _feedBackIcon.StopTransition();
    }

    private void SetText(string name) =>
        _textTmp.text = name;

    private void SetTextRotation(DirectionType direction)
    {
        if (direction == DirectionType.Left)
            _textTransition.Rotate(TransitionType.Instant, 0f, Vector3.zero, new Vector3(0f, 0f, 1f));

        if (direction == DirectionType.Right)
            _textTransition.Rotate(TransitionType.Instant, 0f, Vector3.zero, new Vector3(0f, 0f, -1f));
    }

    public void NewTransition(DirectionType direction)
    {
        if (_textBoxCoroutine != null) { StopCoroutine(_textBoxCoroutine); }
        _textBoxCoroutine = NewTransitionCoroutine();

        Initialize();
        SetAlpha(TransitionType.Instant, 0f, 0f, 1f);
        SetTextRotation(direction);
        StartCoroutine(_textBoxCoroutine);
    }

    private IEnumerator NewTransitionCoroutine()
    {
        Area(TransitionType.Lerp, 0.1f, new Vector2(0f, 287f), new Vector2(1153f, 287f));
        SetColor(TransitionType.Lerp, 0.1f, new Color(1f, 1f, 1f, 0f), new Color(1f, 1f, 1f, 1f));
        yield return new WaitForSeconds(0.1f);

        Area(TransitionType.Smooth, 0.05f, new Vector2(1175f, 287f), new Vector2(1153f, 287f));
        yield return new WaitForSeconds(0.05f);
    }

    public void RepeatTransition()
    {
        if (_textBoxCoroutine != null) { StopCoroutine(_textBoxCoroutine); }
        _textBoxCoroutine = RepeatTransitionCoroutine();

        Initialize();
        SetAlpha(TransitionType.Instant, 0f, 0f, 1f);
        StartCoroutine(_textBoxCoroutine);
    }

    private IEnumerator RepeatTransitionCoroutine()
    {
        Scale(TransitionType.Lerp, 0.1f, new Vector3(1.1f, 1.1f, 1f), new Vector3(0.9f, 0.9f, 1f));
        yield return new WaitForSeconds(0.1f);

        Scale(TransitionType.Smooth, 0.05f, new Vector3(0.9f, 0.9f, 1f), new Vector3(1f, 1f, 1f));
        yield return new WaitForSeconds(0.05f);
    }

    public void EndTransition()
    {
        SetAlpha(TransitionType.Lerp, 0.15f, 1f, 0f);
        _feedBackIcon.StopTransition();
    }

    public void PrintOneByOne(string text)
    {
        if (_printTextCoroutine != null) { StopCoroutine(_printTextCoroutine); }
        _printTextCoroutine = PrintCoroutineOneByOne(text);
        StartCoroutine(_printTextCoroutine);
    }

    private IEnumerator PrintCoroutineOneByOne(string text)
    {
        string tmp = "";
        SetText(tmp);

        foreach (char c in text)
        {
            tmp += c.ToString();
            SetText(tmp);

            float duration = 0.025f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                if (_isPrintAll)
                    break;
                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        _isPrintAll = true;
        yield return null;

        PrintAll(text);
        yield return null;
    }

    public void PrintAll(string text)
    {
        if (_printTextCoroutine != null) { StopCoroutine(_printTextCoroutine); }

        SetText(text);
        _feedBackIcon.AppearTransition();
    }

    public bool IsPrintAll => _isPrintAll;
    public void SetPrintAll(bool value) =>
        _isPrintAll = value;
}
