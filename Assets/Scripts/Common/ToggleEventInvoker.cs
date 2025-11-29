using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Utilities.Common
{
    public class ToggleEventInvoker : MonoBehaviour
    {
        [field: SerializeField] public UnityEvent OnToggleOnEvent { get; private set; }
        [field: SerializeField] public UnityEvent OnToggleOffEvent { get; private set; }

        public void Toggle(bool value)
        {
            if (value)
            {
                OnToggleOnEvent?.Invoke();
            }
            else
            {
                OnToggleOffEvent?.Invoke();
            }
        }
    }
}