namespace Runtime.Utilities.Common
{    
    using UnityEngine;
    using UnityEngine.Events;

    public class LifecycleEventInvoker : MonoBehaviour
    {
        [field: SerializeField] public UnityEvent OnAwakeEvent { get; private set; }
        [field: SerializeField] public UnityEvent OnStartEvent { get; private set; }
        [field: SerializeField] public UnityEvent OnEnableEvent { get; private set; }
        [field: SerializeField] public UnityEvent OnDisableEvent { get; private set; }
        [field: SerializeField] public UnityEvent OnDestroyEvent { get; private set; }
        private void Awake()
        {
            OnAwakeEvent.Invoke();
        }
        private void Start()
        {
            OnStartEvent.Invoke();
        }
        private void OnEnable()
        {
            OnEnableEvent.Invoke();
        }
        private void OnDisable()
        {
            OnDisableEvent.Invoke();
        }
        private void OnDestroy()
        {
            OnDestroyEvent.Invoke();
        }
    }
}