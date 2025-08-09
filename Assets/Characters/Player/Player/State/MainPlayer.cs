using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MoveBehavior
{
    Walk, Dash, Jump
}

public enum EventBehabior
{
    Attack, Hit, Die
}

public class MainPlayer : CharacterBase
{
    public State<MainPlayer>[] _move;
    public State<MainPlayer>[] _eventBehavior;

    public StateMachine<MainPlayer> _moveMachine;
    public StateMachine<MainPlayer> _eventMachine;

    [SerializeField] private CharacterPlayer _data;
    [SerializeField] private Transform _handlerTransform;
    [SerializeField] private SpriteRenderer _character;

    private Rigidbody2D _rigidBody2D;

    private WalkHandler _walkHandler;
    private MoveStatusHandler _moveStatusHandler;
    private DashHandler _dashHandler;
    private InputHandler _inputHandler;

    public Transform HandlerTransform => _handlerTransform;
    public Rigidbody2D Rigidbody2D => _rigidBody2D;
    public InputHandler InputHandler => _inputHandler;
    public WalkHandler WalkHandler => _walkHandler;
    public MoveStatusHandler MoveStatusHandler => _moveStatusHandler;
    public DashHandler DashHandler => _dashHandler;
    protected override void Awake()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();

        _walkHandler = _handlerTransform.GetComponent<WalkHandler>();
        _moveStatusHandler = _handlerTransform.GetComponent <MoveStatusHandler>();
        _dashHandler = _handlerTransform.GetComponent<DashHandler>();
        _inputHandler = _handlerTransform.GetComponent<InputHandler>();
    }

    private void Start()
    {
        _walkHandler.Initialize(this);
        _moveStatusHandler.Initialize(this);
        _dashHandler.Initialize(this);
        _inputHandler.Initialize(this, _data.DashCooltime);

        SetUp();
    }

    public override void SetUp()
    {
        _move = new State<MainPlayer>[2];
        _move[(int)MoveBehavior.Walk] = new Walk(_data.MaxWalkSpeed, _data.WalkAccelTime);
        _move[(int)MoveBehavior.Dash] = new Dash(_data.MaxDashSpeed, _data.DashAccelTime, _data.RemainDashTime,_data.MaxWalkSpeed); 

        _moveMachine = new StateMachine<MainPlayer>();
        _moveMachine.SetUp(this, _move[(int)MoveBehavior.Walk]);    
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
