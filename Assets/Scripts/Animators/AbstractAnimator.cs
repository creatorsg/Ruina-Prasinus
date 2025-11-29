using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Runtime.Utilities.Animators.Composite;

namespace Runtime.Utilities.Animators
{
    public abstract class AbstractAnimator : SerializedMonoBehaviour, IAnimator, ISequenceCreator
    {
        /// <summary>
        /// 현재 이 애니메이터에서 관리 중인 애니메이션 시퀀스입니다.
        /// 재생, 일시 정지 상태의 시퀀스를 참조하며, 내부에서만 설정됩니다.
        /// </summary>
        public Sequence Sequence { get; private set; }
        /// <summary>
        /// 현재 시퀀스의 재생 여부를 반환합니다.
        /// </summary>
        public bool IsPlaying => Sequence != null && Sequence.IsPlaying();
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
        protected virtual void OnStart() { }
        /// <summary>
        /// 애니메이션 시작 시 발생하는 이벤트입니다. 
        /// 인스펙터 또는 코드에서 리스너를 등록하여 시작 시 호출할 동작을 연결할 수 있습니다.
        /// </summary>
        [field: SerializeField] public UnityEvent OnStartEvent { get; private set; }
        /// <summary>
        /// 필요에 따라 오버라이드할 수 있는 OnPlay 콜백 메서드입니다.
        /// </summary>
        protected virtual void OnPlay() { }
        /// <summary>
        /// 애니메이션 재생 시 발생하는 이벤트입니다. 일시정지 후 재개 시에도 발생합니다.
        /// 인스펙터 또는 코드에서 리스너를 등록하여 재생 시 호출할 동작을 연결할 수 있습니다.
        /// </summary>
        [field: SerializeField] public UnityEvent OnPlayEvent { get; private set; }
        /// <summary>
        /// 필요에 따라 오버라이드할 수 있는 OnPause 콜백 메서드입니다.
        /// </summary>
        protected virtual void OnPause() { }
        /// <summary>
        /// 애니메이션 일시정지 시 발생하는 이벤트입니다. 
        /// 인스펙터 또는 코드에서 리스너를 등록하여 일시정지 시 호출할 동작을 연결할 수 있습니다.
        /// </summary>
        [field: SerializeField] public UnityEvent OnPauseEvent { get; private set; }
        /// <summary>
        /// 필요에 따라 오버라이드할 수 있는 OnComplete 콜백 메서드입니다.
        /// </summary>
        protected virtual void OnComplete() { }
        /// <summary>
        /// 애니메이션 완료 시 발생하는 이벤트입니다. 반복 재생 애니메이션의 경우 마지막 회차에만 발생합니다.
        /// 인스펙터 또는 코드에서 리스너를 등록하여 완료 시 호출할 동작을 연결할 수 있습니다.
        /// </summary>
        [field: SerializeField] public UnityEvent OnCompleteEvent { get; private set; }
        /// <summary>
        /// 필요에 따라 오버라이드할 수 있는 OnKill 콜백 메서드입니다.
        /// </summary>
        protected virtual void OnKill() { }
        /// <summary>
        /// 애니메이션 중단 시 발생하는 이벤트입니다. 
        /// 인스펙터 또는 코드에서 리스너를 등록하여 중단 시 호출할 동작을 연결할 수 있습니다.
        /// </summary>
        [field: SerializeField] public UnityEvent OnKillEvent { get; private set; }
        [Tooltip("업데이트 루프를 사용할지 여부를 결정합니다.")]
        [field: SerializeField] public bool UseUnscaledTime = false;
        [ShowIf(nameof(UseUnscaledTime))]
        [Tooltip("업데이트 루프를 설정합니다.")]
        [field: SerializeField] public UpdateType UpdateType = UpdateType.Normal;
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
        [ShowIf(nameof(Loop))]
        [Tooltip("무한 루프 상태에서 즉시 완료 시 동작을 정의합니다.")]
        [field: SerializeField] private CompleteModeOnInfiniteLoop _completeModeOnInfiniteLoop;
        /// <summary>
        /// 필요에 따라 오버라이드할 수 있는 OnStepComplete 콜백 메서드입니다.
        /// </summary>
        protected virtual void OnStepComplete() { }
        [ShowIf(nameof(Loop))]
        /// <summary>
        /// 애니메이션 루프의 각 사이클이 완료될 때 호출되는 이벤트입니다.
        /// 인스펙터 또는 코드에서 리스너를 등록하여 각 사이클 완료 시 호출할 동작을 연결할 수 있습니다.
        /// </summary>
        [field: SerializeField] private UnityEvent _onStepCompleteEvent;
        public UnityEvent OnStepCompleteEvent => _onStepCompleteEvent;
        /// <summary>
        /// 애니메이션의 시퀀스를 생성합니다.
        /// </summary>
        /// <returns>애니메이션 시퀀스</returns>
        protected abstract Sequence CreateAnimationSequence();
        /// <summary>
        /// 애니메이션 시퀀스를 생성합니다. 공통 콜백 메서드 및 이벤트를 자동으로 연결합니다.
        /// </summary>
        /// <returns>생성 시퀀스</returns>
        public Sequence CreateSequence()
        {
            Sequence = DOTween.Sequence();

            OnCreate();
            OnCreateEvent.Invoke();

            Sequence.PrependInterval(PreDelay);

            Sequence subSequence = DOTween.Sequence();
            subSequence.Append(CreateAnimationSequence());
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
                    OnStepComplete();
                    OnStepCompleteEvent.Invoke();
                });
            }
            Sequence.Append(subSequence);

            Sequence
            .OnStart(() =>
            {
                OnStart();
                OnStartEvent.Invoke();
            })
            .OnPlay(() =>
            {
                OnPlay();
                OnPlayEvent.Invoke();
            })
            .OnPause(() =>
            {
                OnPause();
                OnPauseEvent.Invoke();
            })
            .OnComplete(() =>
            {
                OnComplete();
                OnCompleteEvent.Invoke();
            })
            .OnKill(() =>
            {
                OnKill();
                OnKillEvent.Invoke();
                Sequence = null;
            });

            Sequence.AppendInterval(PostDelay);
            
            if(UseUnscaledTime)
            {
                Sequence.SetUpdate(UpdateType, true);
            }

            return Sequence;
        }
        /// <summary>
        /// 애니메이션을 초기화합니다. 재생 시 자동 호출됩니다.
        /// </summary>
        protected void ResetAnimation()
        {
            StopAnimation();
            CreateSequence();
        }
        /// <summary>
        /// 애니메이션을 재생합니다.
        /// </summary>
        public void PlayAnimation()
        {
            if (IsPlaying)
            {
                return;
            }
            ResetAnimation();
            Sequence.Play();
        }
        /// <summary>
        /// 애니메이션을 재시작합니다.
        /// </summary>
        public void RestartAnimation()
        {
            if (Sequence == null)
            {
                return;
            }
            Sequence.Restart();
        }
        /// <summary>
        /// 애니메이션을 중단합니다.
        /// </summary>
        public void StopAnimation()
        {
            if (Sequence == null)
            {
                return;
            }
            Sequence.Kill();
            Sequence = null;
        }
        /// <summary>
        /// 애니메이션을 즉시 완료 처리합니다.
        /// </summary>
        public void CompleteAnimation()
        {
            if (IsInfiniteLoop)
            {
                switch (_completeModeOnInfiniteLoop)
                {
                    case CompleteModeOnInfiniteLoop.None:
                        return;
                    case CompleteModeOnInfiniteLoop.Restart:
                        RestartAnimation();
                        return;
                    case CompleteModeOnInfiniteLoop.Pause:
                        PauseAnimation();
                        return;
                    case CompleteModeOnInfiniteLoop.Stop:
                        StopAnimation();
                        return;
                }
            }

            if (Sequence == null || !IsPlaying)
            {
                return;
            }
            Sequence.Complete();
        }
        /// <summary>
        /// 애니메이션을 일시정지/재개합니다.
        /// </summary>
        public void TogglePauseAnimation()
        {
            if (Sequence == null)
            {
                return;
            }
            Sequence.TogglePause();
        }
        /// <summary>
        /// 애니메이션을 일시정지합니다.
        /// </summary>
        public void PauseAnimation()
        {
            if (Sequence == null || !IsPlaying)
            {
                return;
            }
            Sequence.Pause();
        }
        /// <summary>
        /// 일시정지된 애니메이션을 재개합니다.
        /// </summary>
        public void ResumeAnimation()
        {
            if (Sequence == null)
            {
                return;
            }
            Sequence.Play();
        }
    }
}