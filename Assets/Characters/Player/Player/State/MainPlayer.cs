using Player;
using System;
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
    private YDeltaChecker _yDeltaChecker;
    private footsound _footSound;

    public Transform HandlerTransform => _handlerTransform;
    public AnimatorManager AnimatorManager => _animatorManager;
    public Rigidbody2D Rigidbody2D => _rigidBody2D;
    public InputHandler InputHandler => _inputHandler;
    public MoveHandler MoveHandler => _moveHandler;
    public MoveStatusHandler MoveStatusHandler => _moveStatusHandler;
    public playerHpHandler PlayerHpHandler => _playerHpHandler;
    public YDeltaChecker YDeltaChecker => _yDeltaChecker;
    public footsound footsound => _footSound;

    //BugM0
    public event Action<MoveBehavior> OnMoveStateChanged;
    protected override void Awake()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();

        _inputHandler = _handlerTransform.GetComponent<InputHandler>();
        _moveStatusHandler = _handlerTransform.GetComponent<MoveStatusHandler>();
        _moveHandler = _handlerTransform.GetComponent<MoveHandler>();
        _playerHpHandler = _handlerTransform.GetComponent<playerHpHandler>();
        _yDeltaChecker = _handlerTransform.GetComponent<YDeltaChecker>();
        _footSound = _handlerTransform.GetComponent<footsound>();
    }

    private void Start()
    {
        _inputHandler.Initialize(this, _data.DashCooltime);
        _moveStatusHandler.Initialize(this);
        _moveHandler.Initialize(this);
        _playerHpHandler.Initialize(this,_data.PlayerHp);
        _yDeltaChecker.Initialize(this);
        _footSound.Initialize(this);

        SetUp();
    }

    public override void SetUp()
    {
        _move = new State<MainPlayer>[4];
        _move[(int)MoveBehavior.Walk] = new Walk();
        _move[(int)MoveBehavior.Dash] = new Dash();
        _move[(int)MoveBehavior.Jump] = new Jump(_data.JumpPower, _data.JumpAccelPower, _data.JumpRemainTime);
        _move[(int)MoveBehavior.Idle] = new Idle();
        _moveMachine = new StateMachine<MainPlayer>();
        _moveMachine.SetUp(this, _move[(int)MoveBehavior.Idle]);

        _event = new State<MainPlayer>[4];
        _event[(int)EventBehavior.None] = new None();
        _event[(int)EventBehavior.Attack] = new Attack();
        _event[(int)EventBehavior.Hit] = new Hit();
        _event[(int)EventBehavior.Die] = new Die();
        _eventMachine = new StateMachine<MainPlayer>();
        _eventMachine.SetUp(this, _event[(int)EventBehavior.None]);
    }

    public override void Updated()
    {
        if (_moveMachine != null)
            _moveMachine.Execute();

        if(_eventMachine != null)
            _eventMachine.Execute();
    }

    public override void FixedUpdated()
    {
        if (_moveMachine != null)
            _moveMachine.FixedExecute();

        if (_eventMachine != null)
            _eventMachine.FixedExecute();
    }

    public void ChangeMoveState(MoveBehavior state)
    {
        _moveMachine.ChangeState(_move[(int)state]);
        //BugM0
        OnMoveStateChanged?.Invoke(state);
        Debug.Log(state);
    }
}
