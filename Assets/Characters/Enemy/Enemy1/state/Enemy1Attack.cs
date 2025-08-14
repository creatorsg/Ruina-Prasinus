using UnityEngine;

public class Enemy1Attack : State<Butterflymon>
{
    private float _attackDistance;
    private float _attackCooldown = 1.5f;
    private float _attackTimer;
    private float _moveSpeed;

    public Enemy1Attack(float attackDistance, float moveSpeed)
    {
        _attackDistance = attackDistance;
        _moveSpeed = moveSpeed;
    }

    public override void Enter(Butterflymon enemy1)
    {
        Debug.Log("어택 돌입");
        _attackTimer = _attackCooldown;
    }

    public override void Execute(Butterflymon enemy1)
    {
        if (enemy1.Enemy1MoveHandler.Distance > _attackDistance)
        {
            enemy1.ChangeState(EnemyBehavior.Move);
            return;
        }

        _attackTimer += Time.deltaTime;
        if (_attackTimer >= _attackCooldown)
        {
            _attackTimer = 0f;
            enemy1.Enemy1AttackHandler.Attack();
            Debug.Log("공격!");
        }
    }

    public override void FixedExecute(Butterflymon enemy1)
    {
        enemy1.Enemy1MoveHandler.FollowPlayer(_moveSpeed);
    }

    public override void Exit(Butterflymon enemy1)
    {

    }
}
