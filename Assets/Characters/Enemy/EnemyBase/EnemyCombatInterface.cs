using UnityEngine;

public interface EnemyCombatInterface
{
    public void Attack(float _attakPower);

    public void Damaged(float _currentHp,float _playerAttack);
}
