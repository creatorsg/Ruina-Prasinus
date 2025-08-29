using System.Runtime.CompilerServices;
using UnityEngine;

public class DetectHandler : MonoBehaviour
{
    private Preston _boss1;
    private GameObject _player;
    private float _direction;
    public GameObject Player => _player;
    public void Initialize(Preston boss1)
    {
        _boss1 = boss1;
    }

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }



    private void DetectPlayer()
    {

    }
}
