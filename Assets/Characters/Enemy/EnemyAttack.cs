using UnityEngine;

public class EnemyAttack
{
    public void Attack(initialState state, EnemyState enemy)
    {
        if (enemy == null) return;

        if (state.hp > 0)
            Debug.Log($"플레이어에게 {enemy.attackPower}의 피해를 입혔습니다.");
        state.hp -= enemy.attackPower;
    }
}
