using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Utilities.Common
{
    public class Collision2DEventInvoker : MonoBehaviour
    {
        [field: SerializeField] public UnityEvent OnCollisionEnter2DEvent { get; private set; }
        [field: SerializeField] public UnityEvent OnCollisionStay2DEvent { get; private set; }
        [field: SerializeField] public UnityEvent OnCollisionExit2DEvent { get; private set; }
        private void OnCollisionEnter2D(Collision2D other)
        {
            OnCollisionEnter2DEvent.Invoke();
        }
        private void OnCollisionStay2D(Collision2D other)
        {
            OnCollisionStay2DEvent.Invoke();
        }
        private void OnCollisionExit2D(Collision2D other)
        {
            OnCollisionExit2DEvent.Invoke();
        }
    }
}