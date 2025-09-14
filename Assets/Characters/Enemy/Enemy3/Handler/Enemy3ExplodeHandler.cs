using System;
using UnityEngine;

public class Enemy3ExplodeHandler : MonoBehaviour
{
    private PurpleMushroom _enemy3;
    public event Action<bool> _isRush;
    
    public void Initialize(PurpleMushroom enemy3)
    {
        _enemy3 = enemy3;
    }
}
