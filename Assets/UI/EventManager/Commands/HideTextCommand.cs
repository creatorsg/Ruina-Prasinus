using System;
using System.Collections;
using UnityEngine;

public class HideTextCommand : EventCommandBase
{
    [SerializeField] private TextWindow _textWindow;

    private MessageData _messageData => MessageData.Instance;

    private static HideTextCommand _instance;
    public static HideTextCommand Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    public override void Execute()
    {
        _textWindow.EndTransition();
        foreach (DirectionType direction in Enum.GetValues(typeof(DirectionType)))
        {
            Actor actor = _messageData.GetActorAtDirection(direction);
            if (actor != null)
                actor.Exit();
        }
        _messageData.ResetTale();
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