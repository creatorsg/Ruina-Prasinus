using UnityEngine;

public class UpdateActorCommand : EventCommandBase
{
    private MessageData _messageData => MessageData.Instance;

    private static UpdateActorCommand _instance;
    public static UpdateActorCommand Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    public void Execute(string name, string gesture)
    {
        // 같은 이름의 액터가 없으면 반환.
        if (!_messageData.IsActorExist(name))
        {
            Debug.LogError($"EnterActor: \"{name}\" does not exist!");
            return;
        }

        Actor actor = _messageData.GetActorByName(name);
        actor.SetGesture(gesture);
    }
}