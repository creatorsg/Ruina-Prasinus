using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using Runtime.Utilities.Animators.Composite;

namespace Runtime.Utilities.Animators.Text
{
    public abstract class AbstractTextAnimator : SerializedMonoBehaviour, IAnimator, ISequenceCreator
    {
        [Tooltip("애니메이션 대상 텍스트 메쉬입니다.")]
        [field: SerializeField] protected TMP_Text _target;
        /// <summary>
        /// DOTween 문자 애니메이터입니다.
        /// </summary>
        protected DOTweenTMPAnimator _animator;
        public abstract DOTweenTMPAnimator Animator { get; set; }
        /// <summary>
        /// 현재 이 애니메이터에서 관리 중인 출력 애니메이션 시퀀스입니다.
        /// 재생, 일시 정지 상태의 시퀀스를 참조하며, 내부에서만 설정됩니다.
        /// </summary>
        public Sequence PrintSequence { get; private set; }
        /// <summary>
        /// 현재 이 애니메이터에서 관리 중인 루프 애니메이션 시퀀스입니다. 출력 애니메이션과 별도로 텍스트의 상시 애니메이션을 담당합니다.
        /// 설정에 따라 출력 애니메이션 시작 또는 종료 시 재생되며, 종료할 수 없습니다.
        /// 재생, 일시 정지 상태의 시퀀스를 참조하며, 내부에서만 설정됩니다.
        /// </summary>
        public Sequence LoopSequence { get; private set; }
        /// <summary>
        /// 출력 애니메이션 시퀀스의 재생 여부를 반환합니다.
        /// 루프 애니메이션은 재생 여부를 검사하지 않습니다.
        /// </summary>
        public bool IsPlaying => PrintSequence != null && PrintSequence.IsPlaying();
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
        /// 출력 애니메이션 시작 시 발생하는 이벤트입니다. 
        /// 인스펙터 또는 코드에서 리스너를 등록하여 시작 시 호출할 동작을 연결할 수 있습니다.
        /// </summary>
        [field: SerializeField] public UnityEvent OnStartEvent { get; private set; }
        /// <summary>
        /// 필요에 따라 오버라이드할 수 있는 OnPlay 콜백 메서드입니다.
        /// </summary>
        protected virtual void OnPlay() { }
        /// <summary>
        /// 출력 애니메이션 재생 시 발생하는 이벤트입니다. 일시정지 후 재개 시에도 발생합니다.
        /// 인스펙터 또는 코드에서 리스너를 등록하여 재생 시 호출할 동작을 연결할 수 있습니다.
        /// </summary>
        [field: SerializeField] public UnityEvent OnPlayEvent { get; private set; }
        /// <summary>
        /// 필요에 따라 오버라이드할 수 있는 OnPause 콜백 메서드입니다.
        /// </summary>
        protected virtual void OnPause() { }
        /// <summary>
        /// 출력 애니메이션 일시정지 시 발생하는 이벤트입니다. 
        /// 인스펙터 또는 코드에서 리스너를 등록하여 일시정지 시 호출할 동작을 연결할 수 있습니다.
        /// </summary>
        [field: SerializeField] public UnityEvent OnPauseEvent { get; private set; }
        /// <summary>
        /// 필요에 따라 오버라이드할 수 있는 OnComplete 콜백 메서드입니다.
        /// </summary>
        protected virtual void OnComplete() { }
        /// <summary>
        /// 출력 애니메이션 완료 시 발생하는 이벤트입니다.
        /// 인스펙터 또는 코드에서 리스너를 등록하여 완료 시 호출할 동작을 연결할 수 있습니다.
        /// </summary>
        [field: SerializeField] public UnityEvent OnCompleteEvent { get; private set; }
        /// <summary>
        /// 필요에 따라 오버라이드할 수 있는 OnKill 콜백 메서드입니다.
        /// </summary>
        protected virtual void OnKill() { }
        /// <summary>
        /// 출력 애니메이션 중단 시 발생하는 이벤트입니다. 
        /// 인스펙터 또는 코드에서 리스너를 등록하여 중단 시 호출할 동작을 연결할 수 있습니다.
        /// </summary>
        [field: SerializeField] public UnityEvent OnKillEvent { get; private set; }
        [Tooltip("루프 애니메이션을 출력 애니메이션 종료 후 재생할지 여부를 결정합니다." +
                 "false일 경우 출력 애니메이션과 동시에 재생됩니다." +
                 "true일 경우 출력 애니메이션 종료 후 재생됩니다.")]
        [field: SerializeField] private bool PlayLoopAfterPrint;
        /// <summary>
        /// 출력 애니메이션 시퀀스를 생성합니다.
        /// </summary>
        /// <returns>애니메이션 시퀀스</returns>
        protected abstract Sequence CreatePrintSequence();
        /// <summary>
        /// 루프 애니메이션 시퀀스를 생성합니다.
        /// </summary>
        /// <returns>애니메이션 시퀀스</returns>
        protected abstract Sequence CreateLoopSequence();
        /// <summary>
        /// 애니메이션 시퀀스를 생성합니다. 공통 콜백 메서드 및 이벤트를 자동으로 연결합니다.
        /// </summary>
        /// <returns>생성 시퀀스</returns>
        public Sequence CreateSequence()
        {
            OnCreate();
            OnCreateEvent.Invoke();
            
            if (Animator == null)
            {
                Animator = new(_target);
            }
            else
            {
                Animator.Refresh();
            }

            PrintSequence = CreatePrintSequence();
            LoopSequence = CreateLoopSequence();

            PrintSequence
            .OnStart(() =>
            {
                OnStart();
                OnStartEvent.Invoke();
                if (!PlayLoopAfterPrint)
                {
                    LoopSequence.Play();
                }
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
                if (PlayLoopAfterPrint)
                {
                    LoopSequence.Play();
                }
            })
            .OnKill(() =>
            {
                OnKill();
                OnKillEvent.Invoke();
                PrintSequence = null;
            });

            LoopSequence
            .OnKill(() =>
            {
                LoopSequence = null;
            });

            return PrintSequence;
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
            PrintSequence.Play();
        }
        /// <summary>
        /// 애니메이션을 재시작합니다.
        /// </summary>
        public void RestartAnimation()
        {
            if (LoopSequence != null && LoopSequence.IsPlaying())
            {
                LoopSequence.Complete();
            }

            if (PrintSequence != null)
            {
                PrintSequence.Restart();
            }
        }
        /// <summary>
        /// 애니메이션을 중단합니다.
        /// </summary>
        public void StopAnimation()
        {
            if (PrintSequence != null)
            {
                PrintSequence.Kill();
                PrintSequence = null;
            }

            if (LoopSequence != null)
            {
                LoopSequence.Kill();
                LoopSequence = null;
            }
        }
        /// <summary>
        /// 애니메이션을 즉시 완료 처리합니다.
        /// </summary>
        public void CompleteAnimation()
        {
            if (PrintSequence == null || !IsPlaying)
            {
                return;
            }
            PrintSequence.Complete();
        }
        /// <summary>
        /// 애니메이션을 일시정지/재개합니다.
        /// </summary>
        public void TogglePauseAnimation()
        {
            if (PrintSequence != null)
            {
                PrintSequence.TogglePause();
            }
            if (LoopSequence != null)
            {
                if (PlayLoopAfterPrint)
                {
                    if (PrintSequence == null)
                    {
                        LoopSequence.TogglePause();
                    }
                }
                else
                {
                    LoopSequence.TogglePause();
                }
            }
        }
        /// <summary>
        /// 애니메이션을 일시정지합니다.
        /// </summary>
        public void PauseAnimation()
        {
            if (PrintSequence != null && PrintSequence.IsPlaying())
            {
                PrintSequence.Pause();
            }

            if (LoopSequence != null && LoopSequence.IsPlaying())
            {
                LoopSequence.Pause();
            }
        }
        /// <summary>
        /// 일시정지된 애니메이션을 재개합니다.
        /// </summary>
        public void ResumeAnimation()
        {
            if (PrintSequence != null)
            {
                PrintSequence.Play();
            }
            if (LoopSequence != null)
            {
                if (PlayLoopAfterPrint)
                {
                    if (PrintSequence == null)
                    {
                        LoopSequence.Play();
                    }
                }
                else
                {
                    LoopSequence.Play();
                }
            }
        }
    }
}