namespace Assets.Scripts.Utilities.InputSystem
{
    public interface IInputEvent<T>
    {
        T InputValue { get; }
    }
}