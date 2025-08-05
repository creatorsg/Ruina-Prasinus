using System.Threading;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Enemy enemy;

    private EnemyAction action;
    private GameObject monster, player;

    private void Awake()
    {
        action = new EnemyAction();

        monster = gameObject;
        player = GameObject.FindGameObjectWithTag("Player");

        action.monster = monster;
        action.player = player;
    }
    private void Update()
    {
        float dist = Vector3.Distance(monster.transform.position, player.transform.position);

        action.Move(enemy, dist);
    }
}
