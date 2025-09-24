using System;
using System.Collections.Generic;
using UnityEngine;

public class ExitActorCommand : EventCommandBase
{
    private MessageData _messageData => MessageData.Instance;

    private static ExitActorCommand _instance;
    public static ExitActorCommand Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    public void Execute(string name, DirectionType direction)
    {
        // 해당 방향에 액터가 없다면 반환.
        if (_messageData.IsActorAt(direction))
        {
            Debug.LogError($"EnterActor: \"{name}\" does not exist in \"{direction}\"!");
            return;
        }

        // 기존의 액터 퇴장.
        Actor previousActor =
            _messageData.GetActorAtDirection(direction);
        previousActor.Exit();

        _messageData.RemoveActorAtTable(name, direction);
    }
}