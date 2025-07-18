using System;
using UnityEngine;
using UnityEngine.AI;

public class InputComponent : MonoBehaviour
{
    public event Action<float> OnMove = delegate { };
    public event Action OnJump = delegate { };
    public event Action OnDash = delegate { };

    void Update()
    {
        float h = 0f;
        if (Input.GetKey(KeyCode.A))
        {
            h -= 1f;
            Debug.Log("좌측키 입력 완료");
        }
        if (Input.GetKey(KeyCode.D))
        {
            h += 1f;
            Debug.Log("우측키 입력 완료");
        }
        OnMove(h);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJump();
            Debug.Log("점프키 입력 완료");
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            OnDash();
            Debug.Log("대시키 입력 완료");
        }
    }
}