using System;
using UnityEngine;

public class InputComponent : MonoBehaviour
{
    public event Action<float> OnMove = delegate { };
    public event Action OnJump = delegate { };
    public event Action OnDash = delegate { };

    void Update()
    {
        float h = 0f;
        if (InputManager.GetKey("MoveLeft"))
        {
            h -= 1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            h += 1f;
        }
        OnMove(h);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            OnDash();
        }
    }
}