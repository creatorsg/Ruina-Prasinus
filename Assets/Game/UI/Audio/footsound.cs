using FMODUnity;
using UnityEngine;

public class footsound : MonoBehaviour
{
    string _walkSound = MusicStorage.GetSE("WalkSound");
    [SerializeField] EventReference _dashSound;
    [SerializeField] EventReference _riverSound;
    [SerializeField] float _rate, _riverRate;

    private bool _riverSoundPlayed;
    private BoxCollider2D _boxCollider2D;
    private GameObject _player;
    private GameObject _river;
    private MoveHandler _playermove;
    private float _time, _riverTime;
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
        if (_playermove.IsWalking)
        {
            if (_time >= _rate)
            {
                PlayWalkSound();
                _time = 0f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_boxCollider2D.CompareTag("Player") && !_riverSoundPlayed)
        {
            RuntimeManager.PlayOneShotAttached(_riverSound, _river);
            _riverSoundPlayed = true;
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
