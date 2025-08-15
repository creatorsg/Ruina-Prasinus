using System;
using System.Collections.Generic;

public class TimerHandler
{
    private class Timer
    {
        public float time;
        public float duration;
        public Action callback;
        public bool repeat;

        public Timer(float duration, Action callback, bool repeat)
        {
            this.duration = duration;
            this.callback = callback;
            this.repeat = repeat;
            time = 0f;
        }
    }

    private List<Timer> timers = new List<Timer>();

    // 새로운 타이머 등록
    public void AddTimer(float duration, Action callback, bool repeat = false)
    {
        timers.Add(new Timer(duration, callback, repeat));
    }

    // 매 프레임 호출
    public void UpdateTimers(float deltaTime)
    {
        for (int i = timers.Count - 1; i >= 0; i--)
        {
            Timer t = timers[i];
            t.time += deltaTime;
            if (t.time >= t.duration)
            {
                t.callback?.Invoke();
                if (t.repeat)
                    t.time -= t.duration;
                else
                    timers.RemoveAt(i);
            }
        }
    }

    public void ClearAllTimers()
    {
        timers.Clear();
    }
}
