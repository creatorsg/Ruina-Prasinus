using FMODUnity;
using UnityEngine;

public class footsound : MonoBehaviour
{
    private MainPlayer _Mainplayer;
    string _walkSound = MusicStorage.GetSE("WalkSound");
    [SerializeField] EventReference _dashSound;
    [SerializeField] EventReference _attackSound;
    [SerializeField] EventReference _attackSound2;  
    [SerializeField] EventReference _riverSound;
    [SerializeField] float _rate, _riverRate;

    private bool _riverSoundPlayed;
    private BoxCollider2D _boxCollider2D;
    private GameObject _player;
    private GameObject _river;
    private MoveHandler _playermove;
    private float _time, _riverTime;

    private bool _wasDashing;

    public void Initialize(MainPlayer player)
    {
        _Mainplayer = player;
    }
    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        if (_player != null)
        {
            _playermove = _player.GetComponent<MoveHandler>();
        }

        _river = GameObject.FindGameObjectWithTag("River");
        RuntimeManager.PlayOneShotAttached(_riverSound, _river);

        if(_river == null)
        {
            Debug.Log("_river를 찾을 수 없습니다.");
        }
    }

    private void Update()
    {
        _time += Time.deltaTime;

        if (_playermove.IsWalking && _Mainplayer.MoveStatusHandler.CanJump)
        {
            if (_time >= _rate)
            {
                PlayWalkSound();
                _time = 0f;
            }
        }
        _wasDashing = _playermove.IsDashing;
    }



    public void PlayWalkSound()
    {
        RuntimeManager.PlayOneShotAttached(_walkSound, _player);
    }
    
    public void PlayDashSound()
    {
        RuntimeManager.PlayOneShotAttached(_dashSound, _player);
    }

    public void PlayAttackSound(float a)
    {
        if (a == 0)
        {
            RuntimeManager.PlayOneShotAttached(_attackSound, _player);
        }
        else if (a == 1)
        {
            RuntimeManager.PlayOneShotAttached(_attackSound2, _player);
        }
    }
}
