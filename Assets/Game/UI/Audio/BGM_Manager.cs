using UnityEngine;
using UnityEngine.Audio;

public class BGM_Manager : MonoBehaviour
{
    public Music music;

    public AudioSource bgm;

    public void PlayMusic(int index)
    {
        if (index < 0 || index >= music.musics.Count) return;
        bgm.clip = music.musics[index];
        bgm.Play();
    }
}
