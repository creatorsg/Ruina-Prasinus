using UnityEngine;

public class Enemy1Attack : State<Butterflymon>
{
    private float _attackDistance;
    private float _attackCooldown = 1.0f;
    private float _attackTimer;
    private float _moveSpeed;

    private bool _canAttack, _cooldown;

    public Enemy1Attack(float attackDistance, float moveSpeed)
    {
        _attackDistance = attackDistance;
        _moveSpeed = moveSpeed;
    }

    public override void Enter(Butterflymon enemy1)
    {
        Debug.Log("어택 돌입");
        _canAttack = true;
    }

    public override void Execute(Butterflymon enemy1)
    {
        if (enemy1.Enemy1MoveHandler.Distance > _attackDistance)
        {
            enemy1.ChangeState(EnemyBehavior.Move);
            return;
        }

        if(_cooldown && _attackTimer > 0f)
        {
            _attackTimer -= Time.deltaTime;
            if(_attackTimer < 0f)
            {
                _canAttack = true;
            }
        }

        if (_canAttack)
        { 
            enemy1.Enemy1AttackHandler.Attack();
            if(enemy1.Enemy1AttackHandler.Attacking)
            {
                _canAttack = false;
                _attackTimer = _attackCooldown;
                _cooldown = true;
                enemy1.Enemy1AttackHandler.AttackEnd();
            }
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
