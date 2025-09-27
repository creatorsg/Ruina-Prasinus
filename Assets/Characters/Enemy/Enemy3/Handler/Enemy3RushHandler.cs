using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy3RushHandler : MonoBehaviour
{
    private PurpleMushroom _enemy3;
    private float _hp, _heatTimer;
    private bool _isHeating;
    private SpriteRenderer _spriteRenderer;
    public void Initialize(PurpleMushroom enemy3)
    {
        _enemy3 = enemy3;
    }

}
