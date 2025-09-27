using Player;
using Unity.VisualScripting;
using UnityEngine;

public enum BossBehaviour
{
    Idle ,Attack1, Attack2, Attack3, Attack4 , Stun, Die
}

public class Preston : CharacterBase
{
    [SerializeField] private PrestonData _data;
    [SerializeField] private Transform _handlerTransform;
    [SerializeField] private AnimatorManager _animatorManager;

    public State<Preston>[] _pattern;
    public StateMachine<Preston> _patternMachine;

    private Rigidbody2D _rigidBody2D;
    private Pattern1Attack _pattern1;
    private Boss1HpHandelr _bossHp;
    private DetectHandler _detect;

    public Transform HandlerTransform => _handlerTransform;
    public AnimatorManager AnimatorManager => _animatorManager;
    public Rigidbody2D Rigidbody2D => _rigidBody2D;
    public Pattern1Attack Pattern1Attack => _pattern1;
    public Boss1HpHandelr Boss1HpHandelr => _bossHp;
    public DetectHandler DetectHandler => _detect;

    protected override void Awake()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();

        _pattern1 = _handlerTransform.GetComponent<Pattern1Attack>();
        _bossHp = _handlerTransform.GetComponent <Boss1HpHandelr>();
        _detect = _handlerTransform.GetComponent<DetectHandler>();
    }

    public void Start()
    {
        _detect.Initialize(this);
        _pattern1.Initialize(this);
        _bossHp.Initialize(this, _data.BossHp);
        
        SetUp();
    }


    public override void SetUp()
    {
        _pattern = new State<Preston>[7];
        _pattern[(int)BossBehaviour.Idle] = new IdleState();
        _pattern[(int)BossBehaviour.Attack1] = new Attack1State();
        _pattern[(int)BossBehaviour.Attack2] = new Attack2State();
        _pattern[(int)BossBehaviour.Attack3] = new Attack3State();
        _pattern[(int)BossBehaviour.Attack4] = new Attack4State();
        _pattern[(int)BossBehaviour.Stun] = new StunState();
        _pattern[(int)BossBehaviour.Die] = new Attack1State();
        _patternMachine = new StateMachine<Preston>();
        _patternMachine.SetUp(this, _pattern[(int)BossBehaviour.Idle]);
    }

    public override void Updated()
    {
        if (_patternMachine != null)
            _patternMachine.Execute();
    }

    public override void FixedUpdated()
    {
        if (_patternMachine != null)
            _patternMachine.FixedExecute();
    }
    public void ChangeState(BossBehaviour state)
    {
        _patternMachine.ChangeState(_pattern[(int)state]);
    }
}
