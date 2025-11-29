using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace Runtime.Utilities.Animators.Text.Character
{
    public abstract class AbstractCharAnimator : SerializedMonoBehaviour, ICharSequenceCreator
    {
        /// <summary>
        /// DOTween 문자 애니메이터입니다.
        /// </summary>
        protected DOTweenTMPAnimator _animator;
        public virtual DOTweenTMPAnimator Animator
        {
            get => _animator;
            set
            {
                _animator = value;
            }
        }
        /// <summary>
        /// 필요에 따라 오버라이드할 수 있는 OnCreate 콜백 메서드입니다.
        /// </summary>
        protected virtual void OnCreate() { }
        /// <summary>
        /// 애니메이션 생성 시 발생하는 이벤트입니다. 
        /// 주로 합성 애니메이션 하위 시퀀스로 생성 시 사용합니다.
        /// 인스펙터 또는 코드에서 리스너를 등록하여 시작 시 호출할 동작을 연결할 수 있습니다.
        /// </summary>
        [field: SerializeField] public UnityEvent OnCreateEvent { get; private set; }
        /// <summary>
        /// 필요에 따라 오버라이드할 수 있는 OnStart 콜백 메서드입니다.
        /// </summary>
        protected virtual void OnStart(int index) { }
        /// <summary>
        /// 문자 애니메이션 시작 시 발생하는 이벤트입니다.
        /// 인스펙터 또는 코드에서 리스너를 등록하여 시작 시 호출할 동작을 연결할 수 있습니다.
        /// </summary>
        [field: SerializeField] public UnityEvent OnStartEvent { get; private set; }
        /// <summary>
        /// 필요에 따라 오버라이드할 수 있는 OnPlay 콜백 메서드입니다.
        /// </summary>
        protected virtual void OnPlay(int index) { }
        /// <summary>
        /// 애니메이션 재생 시 발생하는 이벤트입니다. 일시정지 후 재개 시에도 발생합니다.
        /// 인스펙터 또는 코드에서 리스너를 등록하여 재생 시 호출할 동작을 연결할 수 있습니다.
        /// </summary>
        [field: SerializeField] public UnityEvent OnPlayEvent { get; private set; }
        /// <summary>
        /// 필요에 따라 오버라이드할 수 있는 OnPause 콜백 메서드입니다.
        /// </summary>
        protected virtual void OnPause(int index) { }
        /// <summary>
        /// 애니메이션 일시정지 시 발생하는 이벤트입니다. 
        /// 인스펙터 또는 코드에서 리스너를 등록하여 일시정지 시 호출할 동작을 연결할 수 있습니다.
        /// </summary>
        [field: SerializeField] public UnityEvent OnPauseEvent { get; private set; }
        /// <summary>
        /// 필요에 따라 오버라이드할 수 있는 OnComplete 콜백 메서드입니다.
        /// </summary>
        protected virtual void OnComplete(int index) { }
        /// <summary>
        /// 애니메이션 완료 시 발생하는 이벤트입니다. 반복 재생 애니메이션의 경우 마지막 회차에만 발생합니다.
        /// 인스펙터 또는 코드에서 리스너를 등록하여 완료 시 호출할 동작을 연결할 수 있습니다.
        /// </summary>
        [field: SerializeField] public UnityEvent OnCompleteEvent { get; private set; }
        [Tooltip("애니메이션 시작 전 대기 시간(초)입니다.")]
        [field: SerializeField] public float PreDelay { get; private set; }
        [Tooltip("애니메이션 종료 후 대기 시간(초)입니다.")]
        [field: SerializeField] public float PostDelay { get; private set; }
        [Tooltip("애니메이션을 반복 재생할지 여부를 결정합니다.")]
        [field: SerializeField] public bool Loop { get; private set; }
        [ShowIf(nameof(Loop))]
        [Tooltip("애니메이션 반복 횟수입니다. -1일 경우 무한 반복합니다.")]
        [field: SerializeField] private int _loopCount;
        public int LoopCount => _loopCount;
        public virtual bool IsInfiniteLoop => Loop && LoopCount < 0;
        [ShowIf(nameof(Loop))]
        [Tooltip("반복 회차 시작 전 대기 시간(초)입니다.")]
        [field: SerializeField] private float _loopPreDelay = 0f;
        public float LoopPreDelay => _loopPreDelay;
        [ShowIf(nameof(Loop))]
        [Tooltip("반복 회차 종료 후 대기 시간(초)입니다.")]
        [field: SerializeField] private float _loopPostDelay = 0f;
        public float LoopPostDelay => _loopPostDelay;
        [ShowIf(nameof(Loop))]
        [field: SerializeField] private LoopType _loopType;
        public LoopType LoopType => _loopType;
        /// <summary>
        /// 필요에 따라 오버라이드할 수 있는 OnStepComplete 콜백 메서드입니다.
        /// </summary>
        protected virtual void OnStepComplete(int index) { }
        [ShowIf(nameof(Loop))]
        /// <summary>
        /// 애니메이션 루프의 각 사이클이 완료될 때 호출되는 이벤트입니다.
        /// 인스펙터 또는 코드에서 리스너를 등록하여 각 사이클 완료 시 호출할 동작을 연결할 수 있습니다.
        /// </summary>
        [field: SerializeField] private UnityEvent _onStepCompleteEvent;
        public UnityEvent OnStepCompleteEvent => _onStepCompleteEvent;
        /// <summary>
        /// 문자 애니메이션 시퀀스를 생성합니다.
        /// </summary>
        /// <param name="index">문자 인덱스</param>
        /// <returns>문자 애니메이션 시퀀스</returns>
        protected abstract Sequence CreateCharAnimationSequence(int index);
        /// <summary>
        /// 문자 애니메이션 시퀀스를 생성합니다. 공통 콜백 메서드 및 이벤트를 자동으로 연결합니다.
        /// </summary>
        /// <param name="index">문자 인덱스</param>
        /// <returns>생성 시퀀스</returns>
        public Sequence CreateSequence(int index)
        {
            Sequence sequence = DOTween.Sequence();

            OnCreate();
            OnCreateEvent.Invoke();

            sequence.PrependInterval(PreDelay);
            
            Sequence subSequence = DOTween.Sequence();
            subSequence.Append(CreateCharAnimationSequence(index));
            if (Loop)
            {
                if (LoopPreDelay > 0)
                {
                    subSequence.PrependInterval(LoopPreDelay);
                }
                if (LoopPostDelay > 0)
                {
                    subSequence.AppendInterval(LoopPostDelay);
                }
                subSequence.SetLoops(_loopCount, _loopType)
                .OnStepComplete(() =>
                {
                    OnStepComplete(index);
                    OnStepCompleteEvent.Invoke();
                });
            }
            sequence.Append(subSequence);

            sequence
            .OnStart(() =>
            {
                OnStart(index);
                OnStartEvent.Invoke();
            })
            .OnPlay(() =>
            {
                OnPlay(index);
                OnPlayEvent.Invoke();
            })
            .OnPause(() =>
            {
                OnPause(index);
                OnPauseEvent.Invoke();
            })
            .OnComplete(() =>
            {
                OnComplete(index);
                OnCompleteEvent.Invoke();
            });

            sequence.AppendInterval(PostDelay);

            return sequence;
        }
    }
}