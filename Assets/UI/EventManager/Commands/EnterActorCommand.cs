using UnityEngine;

public class EnterActorCommand : EventCommandBase
{
    [SerializeField] private GameObject _actorPrefab;
    [SerializeField] private Transform _leftActorTransform, _rightActorTransform;

    private MessageData _messageData => MessageData.Instance;

    private static EnterActorCommand _instance;
    public static EnterActorCommand Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    public void Execute(string name, DirectionType direction, string gesture)
    {
        // 같은 이름의 액터가 있으면 반환.
        if (_messageData.IsActorExist(name))
        {
            Debug.LogError($"EnterActor: \"{name}\" already exist!");
            return;
        }

        // 해당 방향에 액터가 있는지 감지.
        if (!_messageData.IsActorAt(direction))
        {
            // 기존의 액터 퇴장.
            Actor previousActor =
                _messageData.GetActorAtDirection(direction);
            previousActor.Exit();

            _messageData.RemoveActorAtTable(name, direction);
            Debug.LogWarning($"EnterActor: \"{name}\" exited from \"{direction}\"!");
        }
        
        // 새 액터 등장.
        EnterActor(name, direction, gesture);
    }

    private void EnterActor(string name, DirectionType direction, string gesture)
    {
        Transform hub = _leftActorTransform;
        if (direction == DirectionType.Right)
            hub = _rightActorTransform;

        Actor actor = Instantiate(_actorPrefab, hub).GetComponent<Actor>();
        actor.Initialize(name, gesture);
        actor.Enter();

        _messageData.AddActorAtTable(name, actor, direction);
        Debug.Log($"EnterActor: \"{name}\" enter to \"{direction}\".");
    }
}