using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Utilities.Common
{
    public class Collider2DEventInvoker : MonoBehaviour
    {
        [field: SerializeField] public UnityEvent OnTriggerEnter2DEvent { get; private set; }
        [field: SerializeField] public UnityEvent OnTriggerStay2DEvent { get; private set; }
        [field: SerializeField] public UnityEvent OnTriggerExit2DEvent { get; private set; }
        private void OnTriggerEnter2D(Collider2D other)
        {
            OnTriggerEnter2DEvent.Invoke();
        }
        private void OnTriggerStay2D(Collider2D other)
        {
            OnTriggerStay2DEvent.Invoke();
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            OnTriggerExit2DEvent.Invoke();
        }
    }
}