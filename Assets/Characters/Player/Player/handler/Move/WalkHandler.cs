using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class WalkHandler : MonoBehaviour
{
    private MainPlayer _player;

    private event Action<float> OnMove;
    private event Action OnDash;

    private bool _isWalking, _dashRequested;
    private int _facingDirectioin = 1;
    private float _moveInput;

    public bool IsWalking => _isWalking;
    public int FacingDirection => _facingDirectioin;
    public float MoveInput => _moveInput;

    public void Initialize(MainPlayer player)
    {
        _player = player;
    }

    private void Awake()
    {
        OnMove += dir =>
        {
            _moveInput = dir;
            _isWalking = Mathf.Abs(dir) > 0f; 
        };
        OnDash += () => _dashRequested = true;
    }


    private void Update()
    {
        MoveEvent();
        MoveDirection(_moveInput);
    }

    public void MoveDirection(float moveinput)
    {
        if (Mathf.Abs(moveinput) > Mathf.Epsilon)
        {
            _facingDirectioin = moveinput > 0f ? 1 : -  1;
        }
    }


    public void MoveEvent()
    {
        float h = 0;

        if (InputManager.GetKey("MoveLeft"))
        {
            h += 1;    
        }
        else if(InputManager.GetKey("MoveRight"))
        {
            h -= 1;
        }
        OnMove?.Invoke(h);

        if (InputManager.GetKeyDown("Dash"))
        {
            OnDash?.Invoke();
        }
    }
}
