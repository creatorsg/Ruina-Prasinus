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

    private Rigidbody2D _rigidBody2D;

    private WalkHandler _walkHandler;
    private MoveStatusHandler _moveStatusHandler;


    public Transform HandlerTransform => _handlerTransform;
    public Rigidbody2D Rigidbody2D => _rigidBody2D;
    public WalkHandler WalkHandler => _walkHandler;
    public MoveStatusHandler MoveStatusHandler => _moveStatusHandler;
    protected override void Awake()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();

        _walkHandler = _handlerTransform.GetComponent<WalkHandler>();
        _moveStatusHandler = _handlerTransform.GetComponent <MoveStatusHandler>();
    }

    private void Start()
    {
        _walkHandler.Initialize(this);
        _moveStatusHandler.Initialize(this);

        SetUp();
    }

    public override void SetUp()
    {
        _move = new State<MainPlayer>[1];
        _move[(int)MoveBehavior.Walk] = new Walk(_data.maxWalkSpeed, _data.walkAccelTime);

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
