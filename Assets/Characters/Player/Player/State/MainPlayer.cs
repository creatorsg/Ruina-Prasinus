using Player;
using UnityEngine;

public enum MoveBehavior
{
    Idle, Walk, Dash, Jump
}

public enum EventBehavior
{
    None, Attack, Hit, Die
}

public class MainPlayer : CharacterBase
{
    [SerializeField] private CharacterPlayer _data;
    [SerializeField] private Transform _handlerTransform;
    [SerializeField] private AnimatorManager _animatorManager;

    public State<MainPlayer>[] _move;
    public State<MainPlayer>[] _event;

    public StateMachine<MainPlayer> _moveMachine;
    public StateMachine<MainPlayer> _eventMachine;

    private Rigidbody2D _rigidBody2D;

    private InputHandler _inputHandler;
    private MoveHandler _moveHandler;
    private MoveStatusHandler _moveStatusHandler;
    private playerHpHandler _playerHpHandler;

    public Transform HandlerTransform => _handlerTransform;
    public AnimatorManager AnimatorManager => _animatorManager;
    public Rigidbody2D Rigidbody2D => _rigidBody2D;
    public InputHandler InputHandler => _inputHandler;
    public MoveHandler MoveHandler => _moveHandler;
    public MoveStatusHandler MoveStatusHandler => _moveStatusHandler;
    public playerHpHandler PlayerHpHandler => _playerHpHandler;
    protected override void Awake()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();

        _inputHandler = _handlerTransform.GetComponent<InputHandler>();
        _moveStatusHandler = _handlerTransform.GetComponent<MoveStatusHandler>();
        _moveHandler = _handlerTransform.GetComponent<MoveHandler>();
        _playerHpHandler = _handlerTransform.GetComponent<playerHpHandler>();
    }

    private void Start()
    {
        _inputHandler.Initialize(this, _data.DashCooltime);
        _moveStatusHandler.Initialize(this);
        _moveHandler.Initialize(this);
        _playerHpHandler.Initialize(this,_data.PlayerHp);

        SetUp();
    }

    public override void SetUp()
    {
        _move = new State<MainPlayer>[4];
        _move[(int)MoveBehavior.Walk] = new Walk(_data.MaxWalkSpeed, _data.WalkAccelTime);
        _move[(int)MoveBehavior.Dash] = new Dash(_data.MaxDashSpeed, _data.DashAccelTime, _data.RemainDashTime);
        _move[(int)MoveBehavior.Jump] = new Jump(_data.JumpPower, _data.JumpAccelPower, _data.JumpRemainTime);
        _move[(int)MoveBehavior.Idle] = new Idle();
        _moveMachine = new StateMachine<MainPlayer>();
        _moveMachine.SetUp(this, _move[(int)MoveBehavior.Idle]);
    }

    public override void Updated()
    {
        if (_moveMachine != null)
            _moveMachine.Execute();
    }

    public override void FixedUpdated()
    {
        if (_moveMachine != null)
            _moveMachine.FixedExecute();
    }

    public void ChangeMoveState(MoveBehavior state)
    {
        _moveMachine.ChangeState(_move[(int)state]);
    }
}
