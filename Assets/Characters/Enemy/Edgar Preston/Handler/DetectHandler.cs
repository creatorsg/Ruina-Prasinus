using System.Runtime.CompilerServices;
using UnityEngine;

public class DetectHandler : MonoBehaviour
{
    private Preston _boss1;
    private GameObject _player;
    private float _direction;
    private LayerMask _groundMask;
    private RaycastHit2D _hit;
    public GameObject Player => _player;

    public RaycastHit2D Hit => _hit;

    public LayerMask groundMask => _groundMask;
    public void Initialize(Preston boss1)
    {
        _boss1 = boss1;
    }

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _groundMask = LayerMask.GetMask("Ground");
    }

    private void Update()
    {

    }

}
