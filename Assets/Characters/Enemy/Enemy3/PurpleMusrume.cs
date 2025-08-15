using UnityEngine;
using UnityEngine.EventSystems;


public enum Enemy3Behaviour
{
    Idle, Stop, Rush, Die, Delete
}
public class PurpleMushrooms : CharacterBase
{
    [SerializeField] private CharacterEnemy3 _data;
    [SerializeField] private Transform _handlerTransform;

    private Rigidbody2D _rigidBody2D;

    private GroundEnemyMoveHandler _groundCheck;
    private Enemy3MoveHandler _moveHandler;
    private ExplodeHandler _exploreHandler;

    public State<PurpleMushrooms>[] _enemy3;
    public StateMachine<PurpleMushrooms> _enemy3Machine;

    public Transform HandlerTransform => _handlerTransform;
    public Rigidbody2D Rigidbody2D => _rigidBody2D;
    public GroundEnemyMoveHandler GroundCheck => _groundCheck;
    public Enemy3MoveHandler MoveHandler => _moveHandler;
    public ExplodeHandler ExploreHandler => _exploreHandler;

    protected override void Awake()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();

        _groundCheck = _handlerTransform.GetComponent<GroundEnemyMoveHandler>();
        _moveHandler = _handlerTransform.GetComponent <Enemy3MoveHandler>();
        _exploreHandler = _handlerTransform.GetComponent<ExplodeHandler>();
    }
    private void Start()
    {
        _groundCheck.Initialize(this);  
        _moveHandler.Initialize(this);
        _exploreHandler.Initilaize(this);

        SetUp();
    }
    public override void SetUp()
    {
        _enemy3 = new State<PurpleMushrooms>[5];
        _enemy3[(int)Enemy3Behaviour.Idle] = new Enemy3Idle(_data.MoveSpeed, _data.DetectionRange);
        _enemy3[(int)Enemy3Behaviour.Stop] = new Enemy3Stop();
        _enemy3[(int)Enemy3Behaviour.Rush] = new Enemy3Rush(_data.RushSpeed);
        _enemy3[(int)Enemy3Behaviour.Die] = new Enemy3Die();
        _enemy3[(int)Enemy3Behaviour.Delete] = new Enemy3Delete();

        _enemy3Machine = new StateMachine<PurpleMushrooms>();
        _enemy3Machine.SetUp(this, _enemy3[(int)EnemyBehavior.Idle]);
    }

    public override void Updated()
    {
        if (_enemy3Machine != null)
            _enemy3Machine.Execute();
    }

    public override void FixedUpdated()
    {
        if (_enemy3Machine != null)
            _enemy3Machine.FixedExecute();
    }

    public void ChangeState(Enemy3Behaviour state)
    {
        _enemy3Machine.ChangeState(_enemy3[(int)state]);
    }
}
