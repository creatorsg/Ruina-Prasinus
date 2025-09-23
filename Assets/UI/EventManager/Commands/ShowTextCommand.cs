using System.Collections;
using UnityEngine;

public class ShowTextCommand : EventCommandBase
{
    [SerializeField] private TextWindow _textWindow;
    public KeyCode SkipKey = KeyCode.E;
    private bool PutKey => Input.GetKeyDown(SkipKey);

    private MessageData _messageData => MessageData.Instance;

    private static ShowTextCommand _instance;
    public static ShowTextCommand Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    public IEnumerator ExecuteCoroutine(string name, DirectionType direction, string text)
    {
        _textWindow.SetPrintAll(false);

        if (IsLastActorSame(name))
        {
            _textWindow.RepeatTransition(text);
        }
        else
        {
            _textWindow.RepeatTransition(text);

            Actor MainActor = _messageData.GetActorAtDirection(direction);
            MainActor.SetActive();
            Actor LastActor = _messageData.GetActorAtDirection(Opposite(direction));
            if (LastActor != null)
                LastActor.SetDeactive();

            _textWindow.SetDirectionHub(direction);
            _textWindow.NewTransition(name, direction, text);
        }
        yield return new WaitUntil(() => !Input.GetKey(SkipKey));

        while (!Input.GetKeyDown(SkipKey) && !_textWindow.IsPrintAll)
            yield return null;

        _textWindow.PrintAll(text);
        yield return new WaitUntil(() => !Input.GetKey(SkipKey));

        yield return new WaitUntil(() => Input.GetKeyDown(SkipKey));
    }

    private bool IsLastActorSame(string name)
    {
        bool isSame = _messageData.LastActorName == name;
        _messageData.SetLastActor(name);
        return isSame;
    }

    private DirectionType Opposite(DirectionType direction)
    => direction == DirectionType.Left ? DirectionType.Right : DirectionType.Left;
}