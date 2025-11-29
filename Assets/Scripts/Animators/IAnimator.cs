using UnityEngine.Events;
using Runtime.Utilities.Animators.Composite;

namespace Runtime.Utilities.Animators
{
    public interface IAnimator : ISequenceCreator
    {
        /// <summary>
        /// 애니메이션이 현재 재생 중인지 여부를 반환합니다.
        /// </summary>
        bool IsPlaying { get; }
        /// <summary>
        /// 애니메이션을 재생합니다.
        /// </summary>
        void PlayAnimation();
        /// <summary>
        /// 애니메이션을 재시작합니다.
        /// </summary>
        void RestartAnimation();
        /// <summary>
        /// 애니메이션을 중단합니다.
        /// </summary>
        void StopAnimation();
        /// <summary>
        /// 애니메이션을 즉시 완료 처리합니다.
        /// </summary>
        void CompleteAnimation();
        /// <summary>
        /// 애니메이션을 일시정지/재개합니다.
        /// </summary>
        void TogglePauseAnimation();
        /// <summary>
        /// 애니메이션을 일시정지합니다.
        /// </summary>
        void PauseAnimation();
        /// <summary>
        /// 일시 정지된 애니메이션을 재개합니다.
        /// </summary>
        void ResumeAnimation();
        /// <summary>
        /// 애니메이션 생성 시 발생하는 이벤트입니다. 
        /// 주로 합성 애니메이션 하위 시퀀스로 생성 시 사용합니다.
        /// 인스펙터 또는 코드에서 리스너를 등록하여 시작 시 호출할 동작을 연결할 수 있습니다.
        /// </summary>
        UnityEvent OnCreateEvent { get; }
        /// <summary>
        /// 애니메이션 시작 시 발생하는 이벤트입니다. 
        /// 인스펙터 또는 코드에서 리스너를 등록하여 시작 시 호출할 동작을 연결할 수 있습니다.
        /// </summary>
        UnityEvent OnStartEvent { get; }
        /// <summary>
        /// 애니메이션 재생 시 발생하는 이벤트입니다. 일시정지 후 재개 시에도 발생합니다.
        /// 인스펙터 또는 코드에서 리스너를 등록하여 재생 시 호출할 동작을 연결할 수 있습니다.
        /// </summary>
        UnityEvent OnPlayEvent { get; }
        /// <summary>
        /// 애니메이션 완료 시 발생하는 이벤트입니다. 
        /// 인스펙터 또는 코드에서 리스너를 등록하여 완료 시 호출할 동작을 연결할 수 있습니다.
        /// </summary>
        UnityEvent OnCompleteEvent { get; }
        /// <summary>
        /// 애니메이션 중단 시 발생하는 이벤트입니다. 
        /// 인스펙터 또는 코드에서 리스너를 등록하여 중단 시 호출할 동작을 연결할 수 있습니다.
        /// </summary>
        UnityEvent OnKillEvent { get; }
    }
}