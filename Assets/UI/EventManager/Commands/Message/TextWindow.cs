using System.Collections;
using UnityEngine;

public class TextWindow : UITransitionBase
{
    [SerializeField] private NameBox _nameBox;
    [SerializeField] private TextBox _textBox;

    [SerializeField] private Transform _leftActorTransform;
    [SerializeField] private Transform _rightActorTransform;

    private void Start()
    {
        _nameBox.Initialize();
        _textBox.Initialize();
    }

    public void NewTransition(string name, DirectionType direction, string text)
    {
        _textBox.PrintOneByOne(text);
        _nameBox.NewTransition(name);
        _textBox.NewTransition(direction);
    }

    public void RepeatTransition(string text)
    {
        _textBox.PrintOneByOne(text);
        _textBox.RepeatTransition();
    }

    public void PrintAll(string text) =>
        _textBox.PrintAll(text);

    public bool IsPrintAll => _textBox.IsPrintAll;
    public void SetPrintAll(bool value) =>
        _textBox.SetPrintAll(value);

    public void SetDirectionHub(DirectionType direction)
    {
        Transform hub = _leftActorTransform;
        Rotate(TransitionType.Instant, 0f, Vector3.zero, new Vector3(0f, 0f, 0f));
        if (direction == DirectionType.Right)
        {
            hub = _rightActorTransform;
            Rotate(TransitionType.Instant, 0f, Vector3.zero, new Vector3(0f, 180f, 0f));
        }

        transform.SetParent(hub);
        Position(TransitionType.Instant, 0f, Vector3.zero, new Vector3(0f, 57f, -2f));
    }

    public void EndTransition()
    {
        _nameBox.EndTransition();
        _textBox.EndTransition();
    }
}
