using FMODUnity;
using UnityEngine;

public class footsound : MonoBehaviour
{
    [SerializeField] EventReference _walkSound;
    [SerializeField] EventReference _dashSound;
    [SerializeField] float _rate;

    private GameObject _player;
    private MoveHandler _playermove;
    private float _time;
    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        if (_player != null)
        {
            _playermove = _player.GetComponent<MoveHandler>();
        }
    }

    private void Update()
    {
        _time += Time.deltaTime;
        if (_playermove.IsWalking)
        {
            if (_time >= _rate)
            {
                PlayWalkSound();
                Debug.Log("재생되고 있음");
                _time = 0f;
            }
        }

        if(_playermove.IsDashing)
        {
            PlayDashSound();
        }
    }

    public void PlayWalkSound()
    {
        RuntimeManager.PlayOneShotAttached(_walkSound, _player);
    }
    
    public void PlayDashSound()
    {
        RuntimeManager.PlayOneShotAttached(_dashSound, _player);
    }
}
