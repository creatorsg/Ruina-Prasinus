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
            Debug.Log("����Ű �Է� �Ϸ�");
        }
        if (Input.GetKey(KeyCode.D))
        {
            h += 1f;
            Debug.Log("����Ű �Է� �Ϸ�");
        }
        OnMove(h);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJump();
            Debug.Log("����Ű �Է� �Ϸ�");
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            OnDash();
            Debug.Log("���Ű �Է� �Ϸ�");
        }
    }
}