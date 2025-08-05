using Player;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnemyAction
{
    private Player_Model model = new Player_Model();

    [HideInInspector] public GameObject monster;
    [HideInInspector] public GameObject player;

    // 근접일 경우
    public void Attack(Enemy enemy, initialState player, float damage, float dist)
    {
        if(!model.isHit || !model.isInvincible && dist <= enemy.attackRange)
        {
            if(player.hp > 0)
                player.hp -= damage;    
        }
        return;
    }

    // 원거리 몬스터
    public void Shoot(Enemy enemy,Jangpung jangpung, float dist, GameObject prefab)
    {
        if (dist <= enemy.attackRange)
        {
            GameObject bullet = UnityEngine.Object.Instantiate(prefab, monster.transform.position, Quaternion.identity);
            Vector3 dir = (player.transform.position - monster.transform.position).normalized;
            
            if (bullet.TryGetComponent<Rigidbody2D>(out var rb))
            {
                rb.linearVelocity = dir * jangpung.speed;
            }
        }
    }

    public void Idle(Enemy enemy)
    {
        // 애니메이션 박아 (추가 사항 있으면 나중에 추가해야겠다.)
    }


    // 기본 플레이어를 따라다니는 움직임(바닥 몬스터의 경우)
    public void Move(Enemy enemy, float dist)
    {
        if (dist <= enemy.detectRange && enemy.attackRange <= dist)
        {
            monster.transform.position = Vector3.MoveTowards(monster.transform.position, player.transform.position, enemy.moveSpeed * Time.deltaTime);
        }
    }

}
