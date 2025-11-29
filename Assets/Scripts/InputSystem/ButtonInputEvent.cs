namespace Assets.Scripts.Utilities.InputSystem
{
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.InputSystem;

    public class ButtonInputEvent : MonoBehaviour, IButtonInputEvent
    {
        public bool InputValue { get; private set; }
        [field:SerializeField] public UnityEvent OnButtonStarted { get; private set; }
        [field:SerializeField] public UnityEvent OnButtonPerformed { get; private set; }
        [field:SerializeField] public UnityEvent OnButtonCanceled { get; private set; }

        public void OnButtonInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                InputValue = true;
                OnButtonStarted.Invoke();
            }
            if (context.performed)
            {
                OnButtonPerformed.Invoke();
            }
            if (context.canceled)
            {
                InputValue = false;
                OnButtonCanceled.Invoke();
            }
        }
    }
}