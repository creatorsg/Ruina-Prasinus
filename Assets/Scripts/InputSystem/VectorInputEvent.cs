namespace Assets.Scripts.Utilities.InputSystem
{
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.InputSystem;

    public class VectorInputEvent : MonoBehaviour, IVectorInputEvent
    {
        public Vector2 InputValue { get; private set; }
        [field:SerializeField] public UnityEvent<Vector2> OnInputChanged { get; private set; }

        public void OnVectorInput(InputAction.CallbackContext context)
        {
            InputValue = context.ReadValue<Vector2>();
            OnInputChanged.Invoke(InputValue);
        }
    }
}