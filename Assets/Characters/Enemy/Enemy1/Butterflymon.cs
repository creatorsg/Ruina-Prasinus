using UnityEngine;

public enum EnemyBehavior
{
    Idle, Move, Attack, Hit, Die
}

public class Butterflymon : CharacterBase
{
    [SerializeField] private CharacterEnemy1 _data;
    [SerializeField] private Transform _handlerTransform;

    public State<Butterflymon>[] _enemy;
    public StateMachine<Butterflymon> _enemyMachine;

    private Rigidbody2D _rigidBody2D;

    private Enemy1SpawnHandler _spawnHandler;
    private Enemy1MoveHandler _moveHandler;
    private Enemy1AttackHandler _attackHandler;
    private PauseManager _pauseManager;

    public Transform HandlerTransform => _handlerTransform;
    public Rigidbody2D Rigidbody2D => _rigidBody2D;
    public Enemy1SpawnHandler Enemy1SpawnHandler => _spawnHandler;
    public Enemy1MoveHandler Enemy1MoveHandler => _moveHandler;
    public Enemy1AttackHandler Enemy1AttackHandler => _attackHandler;

    protected override void Awake()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();

        _spawnHandler = _handlerTransform.GetComponent<Enemy1SpawnHandler>();
        _moveHandler = _handlerTransform.GetComponent <Enemy1MoveHandler>();
        _attackHandler = _handlerTransform .GetComponent<Enemy1AttackHandler>();
        _pauseManager = GetComponent<PauseManager>();
    }

    private void Start()
    {
        _spawnHandler.Initialize(this);
        _moveHandler.Initialize(this);
        _attackHandler.Initialize(this, _data.EnemyHp,_data.AttackPower);

        SetUp();
    }
    public override void SetUp()
    {
        _enemy = new State<Butterflymon>[5];
        _enemy[(int)EnemyBehavior.Idle] = new Enemy1Idle(_data.DetectDistance);
        _enemy[(int)EnemyBehavior.Move] = new Enemy1Move(_data.MoveSpeed, _data.AttackDistance);
        _enemy[(int)EnemyBehavior.Attack] = new Enemy1Attack(_data.AttackDistance, _data.MoveSpeed);
        _enemy[(int)EnemyBehavior.Die] = new Enemy1Die();

        _enemyMachine = new StateMachine<Butterflymon>();
        _enemyMachine.SetUp(this, _enemy[(int)EnemyBehavior.Idle]);
    }

    public override void Updated()
    {
        if (_enemyMachine != null)
            _enemyMachine.Execute();
    }

    public override void FixedUpdated()
    {
        if (_enemyMachine != null)
            _enemyMachine.FixedExecute();
    }

    public void ChangeState(EnemyBehavior state)
    {
        _enemyMachine.ChangeState(_enemy[(int)state]);
    }
}
