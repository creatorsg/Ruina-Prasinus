using System;
using UnityEngine;

public class study : MonoBehaviour
{
    Action<string> g = name => Debug.Log(name);
    Func<int, int, int> add = (x, y) => x + y;
    Predicate<int> isEven = num => num % 2 == 0;

    public delegate int Calculate(int a, int b);

    static int Add(int a, int b) => a + b;

    void Start()
    {
        // ����ִ� �Լ��� ��� �ֱ� ���� 

        Calculate calc = Add;
        Debug.Log(calc(5, 3));
    }
}
