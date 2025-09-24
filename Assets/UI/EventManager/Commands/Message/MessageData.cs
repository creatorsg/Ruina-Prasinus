using System.Collections.Generic;
using UnityEngine;

public class MessageData : MonoBehaviour
{
    // 마지막으로 말을 한 액터의 이름.
    private string _lastActorName;

    // 액터의 이름을 입력하면 실제 인스턴스를 가져옴.
    private Dictionary<string, Actor> _actorTable =
        new Dictionary<string, Actor>() { };

    // 방향을 입력하면 액터의 이름을 가져옴.
    private Dictionary<DirectionType, string> _directionToActor =
        new Dictionary<DirectionType, string>(2)
        {
            { DirectionType.Left, "" },
            { DirectionType.Right, "" }
        };

    // 싱글턴 구현.
    private static MessageData _instance;
    public static MessageData Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    public void ResetTale()
    {
        _lastActorName = "";
        _actorTable =
            new Dictionary<string, Actor>() { };
        _directionToActor =
        new Dictionary<DirectionType, string>(2)
        {
            { DirectionType.Left, "" },
            { DirectionType.Right, "" }
        };
    }

    public void AddActorAtTable(string name, Actor actor, DirectionType direction)
    {
        _actorTable.Add(name, actor);
        _directionToActor[direction] = name;
    }

    public void RemoveActorAtTable(string name, DirectionType direction)
    {
        _actorTable.Remove(name);
        _directionToActor[direction] = "";
    }

    public void SetLastActor(string name)
    {
        _lastActorName = name;
    }

    public string LastActorName => _lastActorName;

    public bool IsActorExist(string name) =>
        _actorTable.ContainsKey(name);

    public bool IsActorAt(DirectionType direction)
    {
        if (_directionToActor[direction] != "")
            return false;

        return true;
    }

    public Actor GetActorByName(string name)
    {
        Actor actor = _actorTable[name];
        return actor;
    }

    public Actor GetActorAtDirection(DirectionType direction)
    {
        string actorAtDirectionName = _directionToActor[direction];
        Actor actor = null;
        if (_directionToActor[direction] != "")
            actor = _actorTable[actorAtDirectionName];
        return actor;
    }
}