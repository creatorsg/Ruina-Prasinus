using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Enemy1MoveHandler : MonoBehaviour
{
    private Butterflymon _enemy1;
    private GameObject _player;
    private float _dist;

    public float Distance => _dist;
    public void Initialize(Butterflymon enemy1)
    {
        _enemy1 = enemy1;
    }

    public void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("PlayerBody");
    }

    public void Update()
    {
        _dist = Vector2.Distance(_player.transform.position, gameObject.transform.position);
    }
    
    public void IdleMove(float speed)
    {
        
    }

    public void FollowPlayer(float speed)
    {
        Vector2 nextPos = Vector2.MoveTowards(
                transform.position,
                _player.transform.position,
                speed * Time.deltaTime
            );
        transform.position = nextPos;
    }
}
