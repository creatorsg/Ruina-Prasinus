namespace Assets.Scripts.Utilities.InputSystem
{
    using UnityEngine.Events;

    public interface IButtonInputEvent : IInputEvent<bool>
    {
        UnityEvent OnButtonStarted { get; }
        UnityEvent OnButtonPerformed { get; }
        UnityEvent OnButtonCanceled { get; }
    }
}