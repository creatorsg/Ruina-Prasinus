namespace Assets.Scripts.Utilities.InputSystem
{
    using UnityEngine;
    using UnityEngine.Events;

    public interface IVectorInputEvent : IInputEvent<Vector2>
    {
        UnityEvent<Vector2> OnInputChanged { get; }
    }
}