using Player;
using Unity.VisualScripting;
using UnityEngine;

public enum BossBehaviour
{
    Idle ,Attack1, Attack2, Attack3, Attack4 , Stun
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

    public Transform HandlerTransform => _handlerTransform;
    public AnimatorManager AnimatorManager => _animatorManager;
    public Rigidbody2D Rigidbody2D => _rigidBody2D;
    public Pattern1Attack Pattern1Attack => _pattern1;

    protected override void Awake()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();

        _pattern1 = _handlerTransform.GetComponent<Pattern1Attack>();
    }

    public void Start()
    {
        _pattern1.Initialize(this);
    }


    public override void SetUp()
    {
        _pattern = new State<Preston>[6];
        _pattern[(int)BossBehaviour.Idle] = new Attack1State();
        _pattern[(int)BossBehaviour.Attack1] = new Attack1State();
        _pattern[(int)BossBehaviour.Attack2] = new Attack2State();
        _pattern[(int)BossBehaviour.Attack3] = new Attack1State();
        _pattern[(int)BossBehaviour.Attack4] = new Attack1State();
        _pattern[(int)BossBehaviour.Stun] = new Attack1State();
        _patternMachine = new StateMachine<Preston>();
        _patternMachine.SetUp(this, _pattern[(int)BossBehaviour.Idle]);
    }
}
