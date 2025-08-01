using System;
using UnityEngine;

public class InputComponent : MonoBehaviour
{
    public event Action<float> OnMove;
    public event Action OnJump;
    public event Action OnDash;

    void Update()
    {
        float h = 0f;
        if (InputManager.GetKey("MoveLeft"))
        {
            h -= 1f;
        }
        if (InputManager.GetKey("MoveRight"))
        {
            h += 1f;
        }
        OnMove(h);

        if (InputManager.GetKeyDown("Jump"))
        {
            OnJump();
        }

        if (InputManager.GetKeyDown("Dash"))
        {
            OnDash();
        }
    }
}