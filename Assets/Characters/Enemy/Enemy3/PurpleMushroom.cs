    using UnityEngine;
    using UnityEngine.EventSystems;

    public enum Enemy3Behaviour
    {
        Detect, Walk, Rush, Die, Delete
    }
    public class PurpleMushroom : CharacterBase
    {
        [SerializeField] private CharacterEnemy3 _data;
        [SerializeField] private Transform _handlerTransform;

        public State<PurpleMushroom>[] _enemy3;
        public StateMachine<PurpleMushroom> _enemy3Machine;

        private Rigidbody2D _rigidBody2D;

        private Enemy3StateHandler _Enemy3State;
        private Enemy3MoveHandler _Enemy3Move;
        private Enemy3RushHandler _Enemy3Rush;
        public Transform HandlerTransform => _handlerTransform;
        public Rigidbody2D Rigidbody2D => _rigidBody2D;
        public Enemy3StateHandler Enemy3StateHandler => _Enemy3State;
        public Enemy3MoveHandler Enemy3MoveHandler => _Enemy3Move;
        public Enemy3RushHandler Enemy3RushHandler => _Enemy3Rush;
        
    //BugM0
        public Enemy3Behaviour CurrentBehaviour { get; private set; }

        public event System.Action<Enemy3Behaviour> OnStateChanged;

    protected override void Awake()
        {
            _rigidBody2D = GetComponent<Rigidbody2D>();

            _Enemy3State = _handlerTransform.GetComponent<Enemy3StateHandler>();
            _Enemy3Move = _handlerTransform.GetComponent <Enemy3MoveHandler>();
            _Enemy3Rush = _handlerTransform.GetComponent<Enemy3RushHandler>();
        }

        private void Start()
        {
            _Enemy3State.Initialize(this, _data.DetectDistance, _data.ExplodeDistance);
            _Enemy3Move.Initialize(this);
            _Enemy3Rush.Initialize(this);

            SetUp();
        }

        public override void SetUp()
        {
            _enemy3 = new State<PurpleMushroom>[5];
            _enemy3[(int)Enemy3Behaviour.Detect] = new Enemy3Detect();
            _enemy3[(int)Enemy3Behaviour.Walk] = new Enemy3Walk(_data.WalkSpeed);
            _enemy3[(int)Enemy3Behaviour.Rush] = new Enemy3Rush(_data.RushSpeed);
            _enemy3[(int)Enemy3Behaviour.Die] = new Enemy3Die();
            _enemy3[(int)Enemy3Behaviour.Delete] = new Enemy3Delete();

            _enemy3Machine = new StateMachine<PurpleMushroom>();
            _enemy3Machine.SetUp(this, _enemy3[(int)Enemy3Behaviour.Detect]);
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
        CurrentBehaviour = state; // 현재 상태 기록
        _enemy3Machine.ChangeState(_enemy3[(int)state]);
        OnStateChanged?.Invoke(state); // 상태 변경 이벤트 발생
    }
}
