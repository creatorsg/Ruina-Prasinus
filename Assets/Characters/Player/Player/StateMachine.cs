public class StateMachine<T> where T : class
{
    private T ownerObject;
    public State<T> curState;

    public void SetUp(T owner, State<T> entryState)
    {
        ownerObject = owner;
        curState = entryState;

        ChangeState(entryState);
    }

    public void Execute()
    {
        if (curState != null)
            curState.Execute(ownerObject);
    }

    public void FixedExecute()
    {
        if (curState != null)
            curState.FixedExecute(ownerObject);
    }

    public void ChangeState(State<T> newState)
    {
        if (newState == null) return;
        if (curState != null)
            curState.Exit(ownerObject);

        curState = newState;
        curState.Enter(ownerObject);
    }
}