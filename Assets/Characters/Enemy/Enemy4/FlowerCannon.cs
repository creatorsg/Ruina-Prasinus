using UnityEngine;
using UnityEngine.EventSystems;

public enum Enemy4Behaviour
{
    Idle, Attack
}

public class FlowerCannon : CharacterBase
{
    [SerializeField] private CharacterEnemy4 _data;
    [SerializeField] private Transform _handlerTransform;

    public State<FlowerCannon>[] _enemy4;
    public StateMachine<FlowerCannon> _enemy4Machine;

    private Rigidbody2D _rigidBody2D;
    private AttackHandler _attackHandler;
    private PlayerDetectHandler _playerDetectHandler;

    public Transform HandlerTransform => _handlerTransform;
    public Rigidbody2D Rigidbody2D => _rigidBody2D;
    public AttackHandler AttackHandler => _attackHandler;
    public PlayerDetectHandler PlayerDetectHandler => _playerDetectHandler;

    protected override void Awake()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();

        _playerDetectHandler = _handlerTransform.GetComponent<PlayerDetectHandler>();
        _attackHandler = _handlerTransform.GetComponent<AttackHandler>();
    }

    private void Start()
    {
        _playerDetectHandler.Initialize(this);
        _attackHandler.Initialize(this, _data.BulletSpeed);

        SetUp();
    }
    public override void SetUp()
    {
        _enemy4 = new State<FlowerCannon>[2];
        _enemy4[(int)Enemy4Behaviour.Idle] = new Enemy4Idle();
        _enemy4[(int)Enemy4Behaviour.Attack] = new Enemy4Attack();

        _enemy4Machine = new StateMachine<FlowerCannon>();
        _enemy4Machine.SetUp(this, _enemy4[(int)Enemy4Behaviour.Attack]);
    }

    public override void Updated()
    {
        if (_enemy4Machine != null)
            _enemy4Machine.Execute();
    }

    public override void FixedUpdated()
    {
        if (_enemy4Machine != null)
            _enemy4Machine.FixedExecute();
    }

    public void ChangeState(Enemy4Behaviour state)
    {
        _enemy4Machine.ChangeState(_enemy4[(int)state]);
    }
}
