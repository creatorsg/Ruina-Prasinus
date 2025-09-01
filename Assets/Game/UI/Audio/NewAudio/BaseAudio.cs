using UnityEngine;

public abstract class BaseAudio<T> where T : class
{
    public abstract void Play(T data);
    public abstract void ChangeMusic(T data);
    public abstract void Stop(T data);
}
