using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class WalkHandler : MonoBehaviour
{
    private MainPlayer _player;
    private InputHandler _inputHandler;
    private event Action<float> OnMove;
    private event Action OnDash;

    private bool _isWalking;
    private int _facingDirectioin = 1;
    private float _remainSpeed = 0f;
    public bool IsWalking => _isWalking;
    public int FacingDirection => _facingDirectioin;
    public float RemainSpeed => _remainSpeed;

    public void Initialize(MainPlayer player)
    {
        _player = player;
    }

    private void Awake()
    {
        _inputHandler = GetComponent<InputHandler>();
    }


    private void Update()
    {
        MoveDirection(_inputHandler.MoveInput);
        _isWalking = _inputHandler.MoveInput != 0 ? true : false;
    }

    public void MoveDirection(float moveinput)
    {
        if (Mathf.Abs(moveinput) > Mathf.Epsilon)
        {
            _facingDirectioin = moveinput > 0f ? 1 : -1;
        }
    }

    public void WalkWhileDashing(float speed, float maxWalkSpeed)
    {
        _remainSpeed = speed;
        _remainSpeed = RemainSpeed > maxWalkSpeed ? maxWalkSpeed : _remainSpeed;
    }
}
