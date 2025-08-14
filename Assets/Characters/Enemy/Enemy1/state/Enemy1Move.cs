using UnityEngine;

public class Enemy1Move : State<Butterflymon>
{
    private float _movespeed, _attackDistance;
    public Enemy1Move(float movespeed, float attackDistance)
    {
        _movespeed = movespeed;
        _attackDistance = attackDistance;
    }
    public override void Enter(Butterflymon enemy1)
    {
        Debug.Log("무브 상태 진입");
    }

    public override void Execute(Butterflymon enemy1)
    {
        if (enemy1.Enemy1MoveHandler.Distance < _attackDistance)
        {
            enemy1.ChangeState(EnemyBehavior.Attack);
        }
    }
    public override void FixedExecute(Butterflymon enemy1)
    {
        enemy1.Enemy1MoveHandler.FollowPlayer(_movespeed);
    }
    public override void Exit(Butterflymon enemy1)
    {

    }
}
